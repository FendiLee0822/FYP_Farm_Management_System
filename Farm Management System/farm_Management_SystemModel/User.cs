using System;
using System.Collections.Generic;

#nullable disable

namespace Farm_Management_System.farm_Management_SystemModel
{
    public partial class User
    {
        public User()
        {
            Admins = new HashSet<Admin>();
            Employees = new HashSet<Employee>();
            Growths = new HashSet<Growth>();
            Inventories = new HashSet<Inventory>();
        }

        public int UserId { get; set; }
        public string UserFname { get; set; }
        public string UserLname { get; set; }
        public string UserGender { get; set; }
        public DateTime UserDob { get; set; }
        public string UserPwd { get; set; }
        public string UserRole { get; set; }

        public virtual ICollection<Admin> Admins { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Growth> Growths { get; set; }
        public virtual ICollection<Inventory> Inventories { get; set; }
    }
}
