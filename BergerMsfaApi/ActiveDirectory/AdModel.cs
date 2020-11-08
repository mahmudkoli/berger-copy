using System;

namespace BergerMsfaApi.ActiveDirectory
{
    public class AdModel
    {
        public string Manager { get; set; }
        public string EmployeeId { get; set; }

        public String Department { get; set; }

        public String FirstName { get; set; }

        public String MiddleName { get; set; }

        public String LastName { get; set; }

        public String LoginName { get; set; }

        public String LoginNameWithDomain { get; set; }

        public String StreetAddress { get; set; }

        public String City { get; set; }

        public String State { get; set; }

        public String PostalCode { get; set; }

        public String Country { get; set; }

        public String HomePhone { get; set; }

        public String Extension { get; set; }

        public String Mobile { get; set; }

        public String Fax { get; set; }

        public String EmailAddress { get; set; }

        public String Title { get; set; }

        public String Company { get; set; }

        //public AdModel GetManager
        //{
        //    get
        //    {
        //        if (!String.IsNullOrEmpty(ManagerName))
        //        {
        //            ActiveDirectoryServices ad = new ActiveDirectoryServices();
        //            return ad.GetUserByUserName(ManagerName);
        //        }
        //        return null;
        //    }
        //    set => throw new NotImplementedException();
        //}

        public String ManagerName { get; set; }
        public String ManagerId { get; set; }


        //private AdModel(DirectoryEntry directoryUser)
        //{

        //    String domainAddress;
        //    String domainName;
        //    FirstName = GetProperty(directoryUser, AdProperties.FIRSTNAME);
        //    MiddleName = GetProperty(directoryUser, AdProperties.MIDDLENAME);
        //    LastName = GetProperty(directoryUser, AdProperties.LASTNAME);
        //    LoginName = GetProperty(directoryUser, AdProperties.LOGINNAME);
        //    String userPrincipalName = GetProperty(directoryUser, AdProperties.USERPRINCIPALNAME);
        //    if (!string.IsNullOrEmpty(userPrincipalName))
        //    {
        //        domainAddress = userPrincipalName.Split('@')[1];
        //    }
        //    else
        //    {
        //        domainAddress = String.Empty;
        //    }

        //    if (!string.IsNullOrEmpty(domainAddress))
        //    {
        //        domainName = domainAddress.Split('.').First();
        //    }
        //    else
        //    {
        //        domainName = String.Empty;
        //    }
        //    LoginNameWithDomain = $@"{domainName}\{LoginName}";
        //    StreetAddress = GetProperty(directoryUser, AdProperties.STREETADDRESS);
        //    City = GetProperty(directoryUser, AdProperties.CITY);
        //    State = GetProperty(directoryUser, AdProperties.STATE);
        //    PostalCode = GetProperty(directoryUser, AdProperties.POSTALCODE);
        //    Country = GetProperty(directoryUser, AdProperties.COUNTRY);
        //    Company = GetProperty(directoryUser, AdProperties.COMPANY);
        //    Department = GetProperty(directoryUser, AdProperties.DEPARTMENT);
        //    HomePhone = GetProperty(directoryUser, AdProperties.HOMEPHONE);
        //    Extension = GetProperty(directoryUser, AdProperties.EXTENSION);
        //    Mobile = GetProperty(directoryUser, AdProperties.MOBILE);
        //    Fax = GetProperty(directoryUser, AdProperties.FAX);
        //    EmailAddress = GetProperty(directoryUser, AdProperties.EMAILADDRESS);
        //    Title = GetProperty(directoryUser, AdProperties.TITLE);
        //    Manager = GetProperty(directoryUser, AdProperties.MANAGER);
        //    EmployeeId = GetProperty(directoryUser, AdProperties.EmployeeId);
        //    if (!String.IsNullOrEmpty(Manager))
        //    {
        //        String[] managerArray = Manager.Split(',');
        //        ManagerName = managerArray[0].Replace("CN=", "");
        //    }
        //}


        //private static String GetProperty(DirectoryEntry userDetail, String propertyName)
        //{
        //    if (userDetail.Properties.Contains(propertyName))
        //    {
        //        return userDetail.Properties[propertyName][0].ToString();
        //    }
        //    else
        //    {
        //        return string.Empty;
        //    }
        //}

        //public static AdModel GetUser(DirectoryEntry directoryUser)
        //{
        //    return new AdModel(directoryUser);
        //}
    }
}
