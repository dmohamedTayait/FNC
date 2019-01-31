using System.ServiceModel;
using System;
namespace TayaIT.Enterprise.EMadbatah.EParlimentService
{
    public class Program
    {
        private ServiceHost host;

        static void Main(string[] args)
        {
            new Program();
        }

        public Program()
        {
            host = new ServiceHost(typeof(EPService));

            host.Open();

            Console.WriteLine("Server Started!");

            Console.ReadKey();
        }
    }


}
