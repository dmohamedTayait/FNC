using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TayaIT.Enterprise.EMadbatah.Model
{
    public class EMadbatahUser
    {
        public EMadbatahUser() { }
        public EMadbatahUser(long id, UserRole role, string name, string domainUserName, string email, bool isActive) 
        {
            ID = id;
            Role = role;
            Name = name;
            DomainUserName = domainUserName;
            Email = email;
            IsActive = isActive;
        }

        public EMadbatahUser(UserRole role, string name, string domainUserName, string email, bool isActive)
        {
            Role = role;
            Name = name;
            DomainUserName = domainUserName;
            Email = email;
            IsActive = isActive;
        }

        public long ID { get; set; }
        public UserRole Role { get; set; }
        public string Name { get; set; }
        public string DomainUserName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }

    }
}
