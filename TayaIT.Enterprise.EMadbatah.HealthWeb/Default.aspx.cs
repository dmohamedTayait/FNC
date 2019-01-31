using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TayaIT.Enterprise.EMadbatah.Config;
using TayaIT.Enterprise.EMadbatah.Model;
using System.Web.Configuration;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Data.SqlClient;
using System.Text;

namespace TayaIT.Enterprise.EMadbatah.Web
{
    public partial class AdminAPPConfig : BasePage
    {

        ServiceReference1.EPServiceClient _client = null;
        public void InitializeService(string serviceURL)
        {


            dynamic binding = new BasicHttpBinding();
            binding.Name = "BasicHttpBinding_IEPService";
            binding.CloseTimeout = TimeSpan.FromMinutes(2);
            binding.OpenTimeout = TimeSpan.FromMinutes(2);
            binding.ReceiveTimeout = TimeSpan.FromMinutes(10);
            binding.SendTimeout = TimeSpan.FromMinutes(10);
            binding.AllowCookies = false;
            binding.BypassProxyOnLocal = false;
            binding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
            binding.MaxBufferSize = 65536;
            binding.MaxBufferPoolSize = 524288;
            binding.MessageEncoding = WSMessageEncoding.Text;
            binding.TextEncoding = System.Text.Encoding.UTF8;
            binding.TransferMode = TransferMode.Buffered;
            binding.UseDefaultWebProxy = true;

            binding.ReaderQuotas.MaxDepth = 32;
            binding.ReaderQuotas.MaxStringContentLength = 2097152;
            binding.ReaderQuotas.MaxArrayLength = 2097152;
            binding.ReaderQuotas.MaxBytesPerRead = 4096;
            binding.ReaderQuotas.MaxNameTableCharCount = 16384;

            binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
            binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
            binding.Security.Transport.Realm = "";
            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            binding.Security.Message.AlgorithmSuite = SecurityAlgorithmSuite.Default;
            //binding.Security.
            //            binding.
            //            <security mode="TransportCredentialOnly">
            //  <transport clientCredentialType="Windows" proxyCredentialType="None" realm="" />
            //  <message clientCredentialType="UserName" algorithmSuite="Default" />
            //</security>


            // binding.Security.Message.AlgorithmSuite = System.Security.SecurityAlgorithmSuite.Default;

            //Define the endpoint address'
            dynamic endpointStr = serviceURL;// AppConfig.GetInstance().EPrlimentServerURL;
            dynamic endpoint = new EndpointAddress(endpointStr);
            //Instantiate the SOAP client using the binding and endpoint'
            //that were defined above'
            _client = new ServiceReference1.EPServiceClient(binding, endpoint);

        }
        public bool IsMadbatahAccessible()
        {
            try
            {

                string EMadbatahURL = WebConfigurationManager.AppSettings["EMadbatahURL"].ToString();
                string EMadbatahUserName = WebConfigurationManager.AppSettings["EMadbatahUserName"].ToString();
                string EMadbatahPassword = WebConfigurationManager.AppSettings["EMadbatahPassword"].ToString();

                // Create a new webrequest to the mentioned URL.
                WebRequest myWebRequest = WebRequest.Create(EMadbatahURL);

                // Set 'Preauthenticate'  property to true.  Credentials will be sent with the request.
                myWebRequest.PreAuthenticate = true;

                //Console.WriteLine("\nPlease Enter ur credentials for the requested Url");
                //Console.WriteLine("UserName");
                //string UserName = Console.ReadLine();
                //Console.WriteLine("Password");
                //string Password = Console.ReadLine();

                // Create a New 'NetworkCredential' object.
                NetworkCredential networkCredential = new NetworkCredential(EMadbatahUserName, EMadbatahPassword);

                // Associate the 'NetworkCredential' object with the 'WebRequest' object.
                myWebRequest.Credentials = CredentialCache.DefaultCredentials; ;// networkCredential;

                // Assign the response object of 'WebRequest' to a 'WebResponse' variable.
                WebResponse myWebResponse = myWebRequest.GetResponse();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool CheckDatabaseConnectivity(string connectionString, string query)
        {
            bool isQuerable = false;
            try
            {
                //
                // The name we are trying to match.
                //
                string dogName = "Fido";
                //
                // Use preset string for connection and open it.
                //
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    //
                    // Description of SQL command:
                    // 1. It selects all cells from rows matching the name.
                    // 2. It uses LIKE operator because Name is a Text field.
                    // 3. @Name must be added as a new SqlParameter.
                    //
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        //
                        // Add new SqlParameter to the command.
                        //
                        //command.Parameters.Add(new SqlParameter("Name", dogName));
                        //
                        // Read in the SELECT results.
                        //
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            isQuerable = true;
                            //int weight = reader.GetInt32(0);
                            //string name = reader.GetString(1);
                            //string breed = reader.GetString(2);
                            //Console.WriteLine("Weight = {0}, Name = {1}, Breed = {2}", weight, name, breed);
                        }
                    }
                }
                return isQuerable;
            }
            catch {
                return false;
            }

        }
        public bool CheckServerAvailablity(string serverIPAddress, int port)
        {
            try
            {
                IPHostEntry ipHostEntry = Dns.Resolve(serverIPAddress);
                IPAddress ipAddress = ipHostEntry.AddressList[0];

                TcpClient TcpClient = new TcpClient();
                TcpClient.Connect(ipAddress, port);
                TcpClient.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }


        public string ReadEndTokens(string path, Int64 numberOfTokens, Encoding encoding, string tokenSeparator)
        {

            int sizeOfChar = encoding.GetByteCount("\n");
            byte[] buffer = encoding.GetBytes(tokenSeparator);


            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                Int64 tokenCount = 0;
                Int64 endPosition = fs.Length / sizeOfChar;

                for (Int64 position = sizeOfChar; position < endPosition; position += sizeOfChar)
                {
                    fs.Seek(-position, SeekOrigin.End);
                    fs.Read(buffer, 0, buffer.Length);

                    if (encoding.GetString(buffer) == tokenSeparator)
                    {
                        tokenCount++;
                        if (tokenCount == numberOfTokens)
                        {
                            byte[] returnBuffer = new byte[fs.Length - fs.Position];
                            fs.Read(returnBuffer, 0, returnBuffer.Length);
                            return encoding.GetString(returnBuffer);
                        }
                    }
                }

                // handle case where number of tokens in file is less than numberOfTokens
                fs.Seek(0, SeekOrigin.Begin);
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                return encoding.GetString(buffer);
            }
        }

        ///<summary>Returns the end of a text reader.</summary>
        ///<param name="reader">The reader to read from.</param>
        ///<param name="lineCount">The number of lines to return.</param>
        ///<returns>The last lneCount lines from the reader.</returns>
        public string[] Tail(string path, int lineCount)
        {
            if (!File.Exists(path))
                return new string[] { "Log File Not Found" };
            using (TextReader reader = new StreamReader(new FileStream(path,FileMode.Open,FileAccess.Read,FileShare.ReadWrite)))//File.Open(path))
            {
                char[] block = new char[3];
                reader.ReadBlock(block, 0, 3);
                Console.WriteLine(block);

                var buffer = new List<string>(lineCount);
                string line;
                for (int i = 0; i < lineCount; i++)
                {
                    line = reader.ReadLine();
                    if (line == null)
                    {
                        if(buffer.ToArray().Length==0)
                            return new string[] { "Log File is Empty" };
                        return buffer.ToArray();
                    }
                    buffer.Add(line);
                }

                int lastLine = lineCount - 1;           //The index of the last line read from the buffer.  Everything > this index was read earlier than everything <= this indes

                while (null != (line = reader.ReadLine()))
                {
                    lastLine++;
                    if (lastLine == lineCount) lastLine = 0;
                    buffer[lastLine] = line;
                }

                if (lastLine == lineCount - 1)
                {
                    if (buffer.ToArray().Length == 0)
                        return new string[] { "Log File is Empty" };
                    return buffer.ToArray();
                }
                var retVal = new string[lineCount];
                buffer.CopyTo(lastLine + 1, retVal, 0, lineCount - lastLine - 1);
                buffer.CopyTo(0, retVal, lineCount - lastLine - 1, lastLine + 1);

                if (retVal.Length == 0)
                    return new string[] { "Log File is Empty" };
                return retVal;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
            string VecSysServerPath = WebConfigurationManager.AppSettings["VecSysServerPath"].ToString();
            string AudioServerPath = WebConfigurationManager.AppSettings["AudioServerPath"].ToString();
            string EParlimrntWebServiceURL = WebConfigurationManager.AppSettings["EParlimrntWebServiceURL"].ToString();
            InitializeService(EParlimrntWebServiceURL);

            string MadbatahLogFilePath = WebConfigurationManager.AppSettings["MadbatahLogFilePath"].ToString();
            string ServiceLogFilePath = WebConfigurationManager.AppSettings["ServiceLogFilePath"].ToString();
            string WatcherLogFilePath = WebConfigurationManager.AppSettings["WatcherLogFilePath"].ToString();

            bool isServiceAccessible = false;
            bool isVecSysServerAccessible = false;
            bool isAudioServerAccessible = false;
            bool isMadbatahAccessible = false;
            bool isWatcherRunning = false;
            bool isFNCDBAcessible = false;
            bool isEMadbtahDBAcessible = false;
            try
            {
                int ret = _client.CheckHealth(225215);//any number
                if (ret == 1)
                    isServiceAccessible = true;
            }
            catch (Exception ex)
            {
                //log
            }

            try
            {
                isVecSysServerAccessible = Directory.Exists(VecSysServerPath);
            }
            catch (Exception ex)
            {
                //log
            }

            try
            {
                isAudioServerAccessible = Directory.Exists(AudioServerPath);
            }
            catch (Exception ex)
            {
                //log
            }

            System.Diagnostics.Process myproc = new System.Diagnostics.Process();
            foreach (System.Diagnostics.Process thisproc in System.Diagnostics.Process.GetProcesses())
            {
                if (thisproc.ProcessName.StartsWith("TayaDirectoryWatcherService"))
                {
                    isWatcherRunning = true;
                }
            }

            isMadbatahAccessible = IsMadbatahAccessible();
            string db = WebConfigurationManager.AppSettings["FNCDbConnectionString"].ToString();
            string query = WebConfigurationManager.AppSettings["FNCDbQuery"].ToString();

            isFNCDBAcessible = CheckDatabaseConnectivity(db, query);

            db = WebConfigurationManager.AppSettings["EmadbatahDbConnectionString"].ToString();
            query = WebConfigurationManager.AppSettings["EmadbatahDbQuery"].ToString();

            isEMadbtahDBAcessible = CheckDatabaseConnectivity(db, query);

            string trueImgPath= "~/images/n.jpg";
            string falseImgPath= "~/images/x.jpg";
            imgIsAudioServerAccessible.Src = isAudioServerAccessible? trueImgPath:falseImgPath;
            imgIsEMadbtahDBAcessible.Src = isEMadbtahDBAcessible? trueImgPath:falseImgPath;
            imgIsFNCDBAcessible.Src = isFNCDBAcessible? trueImgPath:falseImgPath;
            imgIsMadbatahAccessible.Src = isMadbatahAccessible? trueImgPath:falseImgPath;
            imgIsServiceAccessible.Src = isServiceAccessible? trueImgPath:falseImgPath;
            imgIsVecSysServerAccessible.Src = isVecSysServerAccessible? trueImgPath:falseImgPath;
            imgIsWatcherRunning.Src = isWatcherRunning? trueImgPath:falseImgPath;

            string delimeter = "\r\n";
            txtMadbatahLogFilePath.InnerText = Tail(MadbatahLogFilePath, 20).Aggregate((i, j) => i + delimeter + j); //ReadEndTokens(MadbatahLogFilePath, 500, Encoding.Default, "\r\n"); 
            txtServiceLogFilePath.InnerText = Tail(ServiceLogFilePath, 20).Aggregate((i, j) => i + delimeter + j); ;
            txtWatcherLogFilePath.InnerText = Tail(WatcherLogFilePath, 20).Aggregate((i, j) => i + delimeter + j); ;

            //isSqlServerAcessible = CheckServerAvailablity();//portal-subdb
            //CheckDatabaseConnectivity(
            //ReadEndTokens(MadbatahLogFilePath, 500, Encoding.Default, "\r\n");
        }
    }
}
