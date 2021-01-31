using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using BergerMsfaApi.Models.Common;
using Microsoft.Extensions.Options;

namespace BergerMsfaApi.ActiveDirectory
{
    public class ActiveDirectoryServices: IActiveDirectoryServices
    {
        private DirectoryEntry _directoryEntry;
        private readonly IOptions<AppActiveDirectorySettingsModel> _settings;

        public ActiveDirectoryServices(IOptions<AppActiveDirectorySettingsModel> settings)
        {
            _settings = settings;

        }
        private String LDAPDomain => $"{_settings.Value.Domain}";
        private String LDAPPath => $"LDAP://{_settings.Value.Domain}";
        private String username => $"{_settings.Value.Username}";
        private String password => $"{_settings.Value.Password}";
        
        private DirectoryEntry SearchRoot
        {
            get
            {
                if (_directoryEntry == null)
                {
                    
                        _directoryEntry = new DirectoryEntry(LDAPPath, _settings.Value.Username, _settings.Value.Password);//, LDAPUser, LDAPPassword, AuthenticationTypes.Secure);
                   
                }
                return _directoryEntry;
            }
        }
        

        public bool AuthenticateUser(string loginUsername, string loginPassword)
        {
            bool ret;

            try
            {
                DirectoryEntry de = new DirectoryEntry(LDAPPath, loginUsername, loginPassword);
                DirectorySearcher dsearch = new DirectorySearcher(de);

                dsearch.FindOne();

                ret = true;
            }
            catch (Exception)
            {
                ret = false;
            }

            return ret;
        }

        public AdModel GetUserByUserName(string searchUsername)
        {
            try
            {
               
                _directoryEntry = null;
                    DirectorySearcher directorySearch = new DirectorySearcher(SearchRoot);
                    directorySearch.PropertiesToLoad.Add("employeeid");
                    directorySearch.Filter = "(&(objectClass=user)(SAMAccountName=" + searchUsername + "))";
                    //directorySearch.Filter = "(&(objectClass=user)(EmployeeId=253))"; 
                    SearchResult results = directorySearch.FindOne();

                    if (results != null)
                    {
                        DirectoryEntry user = new DirectoryEntry(results.Path,username, password);// LDAPUser, LDAPPassword);
                        return MapUserToModel(user);

                    }
                    else
                    {
                        return null;
                    }
                
            }
            catch (Exception)
            {
                return null;
            }
        }

        private AdModel MapUserToModel(DirectoryEntry directoryUser)
        {
            AdModel model = new AdModel();
            String domainAddress;
            String domainName;
            model.FirstName = GetProperty(directoryUser, AdProperties.FIRSTNAME);
            model.MiddleName = GetProperty(directoryUser, AdProperties.MIDDLENAME);
            model.LastName = GetProperty(directoryUser, AdProperties.LASTNAME);
            model.LoginName = GetProperty(directoryUser, AdProperties.LOGINNAME);
            String userPrincipalName = GetProperty(directoryUser, AdProperties.USERPRINCIPALNAME);
            if (!string.IsNullOrEmpty(userPrincipalName))
            {
                domainAddress = userPrincipalName.Split('@')[1];
            }
            else
            {
                domainAddress = String.Empty;
            }

            if (!string.IsNullOrEmpty(domainAddress))
            {
                domainName = domainAddress.Split('.').First();
            }
            else
            {
                domainName = String.Empty;
            }
            model.LoginNameWithDomain = $@"{domainName}\{model.LoginName}";
            model.StreetAddress = GetProperty(directoryUser, AdProperties.STREETADDRESS);
            model.City = GetProperty(directoryUser, AdProperties.CITY);
            model.State = GetProperty(directoryUser, AdProperties.STATE);
            model.PostalCode = GetProperty(directoryUser, AdProperties.POSTALCODE);
            model.Country = GetProperty(directoryUser, AdProperties.COUNTRY);
            model.Company = GetProperty(directoryUser, AdProperties.COMPANY);
            model.Department = GetProperty(directoryUser, AdProperties.DEPARTMENT);
            model.HomePhone = GetProperty(directoryUser, AdProperties.HOMEPHONE);
            model.Extension = GetProperty(directoryUser, AdProperties.EXTENSION);
            model.Mobile = GetProperty(directoryUser, AdProperties.MOBILE);
            model.Fax = GetProperty(directoryUser, AdProperties.FAX);
            model.EmailAddress = GetProperty(directoryUser, AdProperties.EMAILADDRESS);
            model.Title = GetProperty(directoryUser, AdProperties.TITLE);
            model.Manager = GetProperty(directoryUser, AdProperties.MANAGER);
            model.EmployeeId = GetProperty(directoryUser, AdProperties.EmployeeId);
            if (!String.IsNullOrEmpty(model.Manager))
            {
                String[] managerArray = model.Manager.Split(',');
                var manager = managerArray[0].Replace("CN=", "");
                model.ManagerName = manager.Substring(0, manager.LastIndexOf(" "));
                model.ManagerId = manager.Substring(manager.LastIndexOf(" ")+1);
            }

            return model;
        }
        private static String GetProperty(DirectoryEntry userDetail, String propertyName)
        {
            if (userDetail.Properties.Contains(propertyName))
            {
                return userDetail.Properties[propertyName][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }


        public AdModel GetUserByLoginName(String userName)
        {


            try
            {
                

                    // This code runs as the application pool user



                    _directoryEntry = null;
                    string nn = "LDAP://PRIME.local/DC=PRIME,DC=local";
                    DirectoryEntry directoryEntry = new DirectoryEntry(nn);

                    DirectorySearcher directorySearch = new DirectorySearcher(SearchRoot);
                    directorySearch.Filter = "(&(objectClass=user)(SAMAccountName=" + userName + "))";
                    SearchResult results = directorySearch.FindOne();

                    if (results != null)
                    {
                        DirectoryEntry user = new DirectoryEntry(results.Path);//, LDAPUser, LDAPPassword);
                        return MapUserToModel(user);
                    }
                    return null;
                
            }

            catch (Exception)
            {
                return null;
            }
        }


        public AdModel GetUserDetailsByFullName(String FirstName, String MiddleName, String LastName)
        {
            //givenName
            //    initials
            //    sn
            //(initials=" + MiddleName + ")(sn=" + LastName + ")

            try
            {
                
                    _directoryEntry = null;
                    DirectorySearcher directorySearch = new DirectorySearcher(SearchRoot);
                    //directorySearch.Filter = "(&(objectClass=user)(givenName=" + FirstName + ") ())";

                    if (FirstName != "" && MiddleName != "" && LastName != "")
                    {

                        directorySearch.Filter = "(&(objectClass=user)(givenName=" + FirstName + ")(initials=" + MiddleName + ")(sn=" + LastName + "))";
                    }
                    else if (FirstName != "" && MiddleName != "" && LastName == "")
                    {
                        directorySearch.Filter = "(&(objectClass=user)(givenName=" + FirstName + ")(initials=" + MiddleName + "))";
                    }
                    else if (FirstName != "" && MiddleName == "" && LastName == "")
                    {
                        directorySearch.Filter = "(&(objectClass=user)(givenName=" + FirstName + "))";
                    }
                    else if (FirstName != "" && MiddleName == "" && LastName != "")
                    {
                        directorySearch.Filter = "(&(objectClass=user)(givenName=" + FirstName + ")(sn=" + LastName + "))";
                    }
                    else if (FirstName == "" && MiddleName != "" && LastName != "")
                    {
                        directorySearch.Filter = "(&(objectClass=user)(initials=" + MiddleName + ")(sn=" + LastName + "))";
                    }
                    SearchResult results = directorySearch.FindOne();

                    if (results != null)
                    {
                        DirectoryEntry user = new DirectoryEntry(results.Path);//, LDAPUser, LDAPPassword);
                        return MapUserToModel(user);
                    }
                    return null;
                
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// This function will take a DL or Group name and return list of users
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public List<AdModel> GetUserFromGroup(String groupName)
        {
            List<AdModel> userlist = new List<AdModel>();
            try
            {
                
                    _directoryEntry = null;
                    DirectorySearcher directorySearch = new DirectorySearcher(SearchRoot);
                    directorySearch.Filter = "(&(objectClass=group)(SAMAccountName=" + groupName + "))";
                    SearchResult results = directorySearch.FindOne();
                    if (results != null)
                    {

                        DirectoryEntry deGroup = new DirectoryEntry(results.Path);//, LDAPUser, LDAPPassword);
                        System.DirectoryServices.PropertyCollection pColl = deGroup.Properties;
                        int count = pColl["member"].Count;


                        for (int i = 0; i < count; i++)
                        {
                            string respath = results.Path;
                            string[] pathnavigate = respath.Split("CN".ToCharArray());
                            respath = pathnavigate[0];
                            string objpath = pColl["member"][i].ToString();
                            string path = respath + objpath;


                            DirectoryEntry user = new DirectoryEntry(path);//, LDAPUser, LDAPPassword);
                            AdModel userobj = MapUserToModel(user);
                            userlist.Add(userobj);
                            user.Close();
                        }
                    }
                    return userlist;
                
            }
            catch (Exception ex)
            {
                return userlist;
            }

        }

        #region Get user with First Name

        public List<AdModel> GetUsersByFirstName(string fName)
        {
            

                //UserProfile user;
                List<AdModel> userlist = new List<AdModel>();
                string filter = "";

                _directoryEntry = null;
                DirectorySearcher directorySearch = new DirectorySearcher(SearchRoot);
                directorySearch.Asynchronous = true;
                directorySearch.CacheResults = true;
                filter = "(&(objectCategory=User)(objectClass=person))";
                //            filter = "(&(objectClass=user)(objectCategory=person)(givenName="+fName+ "*))";


                directorySearch.Filter = filter;
            
                SearchResultCollection userCollection = directorySearch.FindAll();
                foreach (SearchResult users in userCollection)
                {
                    DirectoryEntry userEntry = new DirectoryEntry(users.Path,username, password);//, LDAPUser, LDAPPassword);
                    AdModel userInfo = MapUserToModel(userEntry);

                    userlist.Add(userInfo);

                }

                directorySearch.Filter = "(&(objectClass=group)(SAMAccountName=" + fName + "*))";
                SearchResultCollection results = directorySearch.FindAll();
                if (results.Count != 0)
                {

                    foreach (SearchResult r in results)
                    {
                        DirectoryEntry deGroup = new DirectoryEntry(r.Path);//, LDAPUser, LDAPPassword);

                        AdModel agroup = MapUserToModel(deGroup);
                        userlist.Add(agroup);
                    }

                }
                return userlist;
             }

        #endregion


        #region AddUserToGroup
        public bool AddUserToGroup(string userlogin, string groupName)
        {
            try
            {
               
                    _directoryEntry = null;
                    ADManager admanager = new ADManager(LDAPDomain);//, LDAPUser, LDAPPassword);
                    admanager.AddUserToGroup(userlogin, groupName);
                    return true;
                
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region RemoveUserToGroup
        public bool RemoveUserToGroup(string userlogin, string groupName)
        {
            try
            {
                
                    _directoryEntry = null;
                    ADManager admanager = new ADManager("xxx");// LDAPUser, LDAPPassword);
                    admanager.RemoveUserFromGroup(userlogin, groupName);
                    return true;
                
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
        //string name = "";
        //using (var context = new PrincipalContext(ContextType.Domain, LDAPDomain, null, username, password))
        //{
        //    var usr = UserPrincipal.FindByIdentity(context, searchUsername);
        //    if (usr != null)
        //    {
        //        var userviewModel = usr.ToMap<UserPrincipal, AdViewModel>();
        //        name = usr.DisplayName;
        //    }

        //}

    }
}
