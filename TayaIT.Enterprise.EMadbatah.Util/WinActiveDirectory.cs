using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Collections;
using System.DirectoryServices.AccountManagement;
using System.Threading;

namespace TayaIT.Enterprise.EMadbatah.Util
{
    public class WinActiveDirectory
    {
        private Hashtable _usersData;
        private DirectoryEntry _currentDirEntry;
        private PrincipalContext _currentDomainPrincipalContext;
        private string _currentUser = "";
        public WinActiveDirectory(string connectionString, string userName)
        {
            _currentUser = userName;
            _currentDirEntry = new DirectoryEntry();
            _currentDirEntry.Path = connectionString;
            _currentDirEntry.AuthenticationType = AuthenticationTypes.Secure;
            

            _currentDomainPrincipalContext = new PrincipalContext(ContextType.Domain);
           // _currentDomainPrincipalContext.ValidateCredentials(userName, password, ContextOptions.Negotiate | ContextOptions.SecureSocketLayer);
            _currentDomainPrincipalContext = new PrincipalContext(ContextType.Domain);//, domain, null, ContextOptions.Negotiate | ContextOptions.SecureSocketLayer);



        }

        private IEnumerable<Domain> GetDomains()
        {
            ICollection<Domain> domains = new List<Domain>();

            // Querying the current Forest for the domains within.
            foreach (Domain d in Forest.GetCurrentForest().Domains)
                domains.Add(d);

            return domains;
        }

        private string GetDomainFullName(string friendlyName)
        {
            DirectoryContext context = new DirectoryContext(DirectoryContextType.Domain, friendlyName);
            Domain domain = Domain.GetDomain(context);
            return domain.Name;
        }

        public IEnumerable<string> GetUserDomain(string userName)
        {
            foreach (Domain entry in GetDomains())
            {
                // From the domains obtained from the Forest, we search the domain subtree for the given userName.
                using (DirectoryEntry domain = new DirectoryEntry(GetDomainFullName(entry.Name)))
                {
                    using (DirectorySearcher searcher = new DirectorySearcher())
                    {
                        searcher.SearchRoot = domain;
                        searcher.SearchScope = SearchScope.Subtree;
                        searcher.PropertiesToLoad.Add("sAMAccountName");
                        // The Filter is very important, so is its query string. The 'objectClass' parameter is mandatory.
                        // Once we specified the 'objectClass', we want to look for the user whose login
                        // login is userName.
                        searcher.Filter = string.Format("(&(objectClass=user)(sAMAccountName={0}))", userName);

                        try
                        {
                            SearchResultCollection results = searcher.FindAll();

                            // If the user cannot be found, then let's check next domain.
                            if ((results == null) || (results.Count == 0))
                                continue;

                            // Here, we yield return for we want all of the domain which this userName is authenticated.
                            yield return domain.Path;
                        }
                        finally
                        {
                            searcher.Dispose();
                            domain.Dispose();
                        }
                    }
                }
            }
        }

        public bool UserExists(string username)
        {            
            DirectorySearcher deSearch = new DirectorySearcher();
            deSearch.SearchRoot = _currentDirEntry;
            deSearch.Filter = "(&(objectClass=user) (cn=" + username + "))";
            SearchResultCollection results = deSearch.FindAll();
            return results.Count > 0;
        }

        private String FindName(String userAccount)
        {
            String account = userAccount.Replace(@"Domain\", "");

            try
            {
                DirectorySearcher search = new DirectorySearcher(_currentDirEntry);
                search.Filter = "(SAMAccountName=" + account + ")";
                search.PropertiesToLoad.Add("displayName");

                SearchResult result = search.FindOne();

                if (result != null)
                {
                    return result.Properties["displayname"][0].ToString();
                }
                else
                {
                    return "Unknown User";
                }
            }
            catch (Exception ex)
            {
                string debug = ex.Message;

                return "";
            }
        }


        public List<LDAPUser> GetAllUsers()
        {
                List<LDAPUser> userstoRet = new List<LDAPUser>();
                PrincipalSearchResult<Principal> res = null;

                try
                {
                    UserPrincipal p = UserPrincipal.FindByIdentity(_currentDomainPrincipalContext, IdentityType.SamAccountName, _currentUser);
                    res = p.GetGroups();
                }
                catch
                {                    
                    Thread.Sleep(2000);
                    UserPrincipal p = UserPrincipal.FindByIdentity(_currentDomainPrincipalContext, IdentityType.SamAccountName, _currentUser);
                    res = p.GetGroups();
                }
                foreach (GroupPrincipal group in res)
                {
                    foreach (Principal pr in group.Members)
                    {
                        if (pr is UserPrincipal)
                        {
                            UserPrincipal user = (UserPrincipal)pr;

                            userstoRet.Add(new LDAPUser
                            {
                                AccountName = user.SamAccountName,
                                UserName = user.Name,
                                Email = user.EmailAddress,
                                DisplayName = user.DisplayName,
                                Description = user.Description,
                                DomainName = user.Context.Name,
                                GivenName = user.GivenName,
                                MiddleName = user.MiddleName,
                                Surname = user.Surname
                            });
                        }

                    }
                }

              

               // find your user
                //UserPrincipal user = UserPrincipal.FindByIdentity(yourDomain, username);
                //user.


                //DirectorySearcher search = new DirectorySearcher(_currentDirEntry);
                //search.PropertiesToLoad.Add("cn");
                //search.PropertiesToLoad.Add("mail");
                //search.PropertiesToLoad.Add("title");

                //SearchResultCollection allUsers = search.FindAll();

                //foreach (SearchResult result in allUsers)
                //{

                //    users.Add(new LDAPUser{ UserName = result.Properties["cn"][0].ToString(),
                //                            Email =  result.Properties["mail"][0].ToString(),
                //                             DisplayName = result.Properties["title"][0].ToString()});

            
                //}
                return userstoRet;
        }

        public class LDAPUser 
        {

            //_userData.Add("mail", "");
            //_userData.Add("cn", "");
            //_userData.Add("memberof", "");{ get; set; }
            public string AccountName { get; set; }
            public string UserName{ get; set; }
            public string DisplayName { get; set; }
            public string Description { get; set; }
            public string DomainName { get; set; }
            public string Email { get; set; }
            public string Groups { get; set; }
            public string MiddleName { get; set; }
            public string Surname { get; set; }
            public string GivenName { get; set; }
            
        }

    }
}
