using System;
using System.Collections.Generic;

#nullable disable

namespace Farm_Management_System.farm_Management_SystemModel
{
    public partial class Admin
    {
        public Admin()
        {
            Financials = new HashSet<Financial>();
            Plants = new HashSet<Plant>();
            Projects = new HashSet<Project>();
            Tasks = new HashSet<Task>();
        }

        public int AdminId { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Financial> Financials { get; set; }
        public virtual ICollection<Plant> Plants { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
