using System;
using System.DirectoryServices;
using System.Linq;

namespace BergerMsfaApi.ActiveDirectory
{
    public class ADUserDetail
    {
        private String _firstName;
        private String _middleName;
        private String _lastName;
        private String _loginName;
        private String _loginNameWithDomain;
        private String _streetAddress;
        private String _city;
        private String _state;
        private String _postalCode;
        private String _country;
        private String _homePhone;
        private String _extension;
        private String _mobile;
        private String _fax;
        private String _emailAddress;
        private String _title;
        private String _company;
        private String _manager;
        private String _managerName;
        private String _department;

        public String Department
        {
            get { return _department; }
        }

        public String FirstName
        {
            get { return _firstName; }
        }

        public String MiddleName
        {
            get { return _middleName; }
        }

        public String LastName
        {
            get { return _lastName; }
        }

        public String LoginName
        {
            get { return _loginName; }
        }

        public String LoginNameWithDomain
        {
            get { return _loginNameWithDomain; }
        }

        public String StreetAddress
        {
            get { return _streetAddress; }
        }

        public String City
        {
            get { return _city; }
        }

        public String State
        {
            get { return _state; }
        }

        public String PostalCode
        {
            get { return _postalCode; }
        }

        public String Country
        {
            get { return _country; }
        }

        public String HomePhone
        {
            get { return _homePhone; }
        }

        public String Extension
        {
            get { return _extension; }
        }

        public String Mobile
        {
            get { return _mobile; }
        }

        public String Fax
        {
            get { return _fax; }
        }

        public String EmailAddress
        {
            get { return _emailAddress; }
        }

        public String Title
        {
            get { return _title; }
        }

        public String Company
        {
            get { return _company; }
        }

        public ADUserDetail Manager
        {
            get
            {
                if (!String.IsNullOrEmpty(_managerName))
                {
                    ActiveDirectoryServices ad = new ActiveDirectoryServices();
                    return ad.GetUserByFullName(_managerName);
                }
                return null;
            }
        }

        public String ManagerName
        {
            get { return _managerName; }
        }


        private ADUserDetail(DirectoryEntry directoryUser)
        {

            String domainAddress;
            String domainName;
            _firstName = GetProperty(directoryUser, AdModel.FIRSTNAME);
            _middleName = GetProperty(directoryUser, AdModel.MIDDLENAME);
            _lastName = GetProperty(directoryUser, AdModel.LASTNAME);
            _loginName = GetProperty(directoryUser, AdModel.LOGINNAME);
            String userPrincipalName = GetProperty(directoryUser, AdModel.USERPRINCIPALNAME);
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
            _loginNameWithDomain = String.Format(@"{0}\{1}", domainName, _loginName);
            _streetAddress = GetProperty(directoryUser, AdModel.STREETADDRESS);
            _city = GetProperty(directoryUser, AdModel.CITY);
            _state = GetProperty(directoryUser, AdModel.STATE);
            _postalCode = GetProperty(directoryUser, AdModel.POSTALCODE);
            _country = GetProperty(directoryUser, AdModel.COUNTRY);
            _company = GetProperty(directoryUser, AdModel.COMPANY);
            _department = GetProperty(directoryUser, AdModel.DEPARTMENT);
            _homePhone = GetProperty(directoryUser, AdModel.HOMEPHONE);
            _extension = GetProperty(directoryUser, AdModel.EXTENSION);
            _mobile = GetProperty(directoryUser, AdModel.MOBILE);
            _fax = GetProperty(directoryUser, AdModel.FAX);
            _emailAddress = GetProperty(directoryUser, AdModel.EMAILADDRESS);
            _title = GetProperty(directoryUser, AdModel.TITLE);
            _manager = GetProperty(directoryUser, AdModel.MANAGER);
            if (!String.IsNullOrEmpty(_manager))
            {
                String[] managerArray = _manager.Split(',');
                _managerName = managerArray[0].Replace("CN=", "");
            }
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

        public static ADUserDetail GetUser(DirectoryEntry directoryUser)
        {
            return new ADUserDetail(directoryUser);
        }
    }
}
