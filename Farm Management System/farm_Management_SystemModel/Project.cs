using System;
using System.Collections.Generic;

#nullable disable

namespace Farm_Management_System.farm_Management_SystemModel
{
    public partial class Project
    {
        public Project()
        {
            Financials = new HashSet<Financial>();
            Growths = new HashSet<Growth>();
            Tasks = new HashSet<Task>();
        }

        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int AdminCreate { get; set; }
        public DateTime ProjectCreateTime { get; set; }

        public virtual Admin AdminCreateNavigation { get; set; }
        public virtual ICollection<Financial> Financials { get; set; }
        public virtual ICollection<Growth> Growths { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
