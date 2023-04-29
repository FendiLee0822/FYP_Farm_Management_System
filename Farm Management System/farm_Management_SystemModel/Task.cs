using System;
using System.Collections.Generic;

#nullable disable

namespace Farm_Management_System.farm_Management_SystemModel
{
    public partial class Task
    {
        public int TaskId { get; set; }
        public string TaskDescription { get; set; }
        public int? TaskProject { get; set; }
        public int? TaskField { get; set; }
        public int? TaskPlant { get; set; }
        public string TaskStatus { get; set; }
        public int EmployeeIncharge { get; set; }
        public int AdminAssigned { get; set; }
        public DateTime TaskCreateTime { get; set; }
        public DateTime TaskUpdateTime { get; set; }

        public virtual Admin AdminAssignedNavigation { get; set; }
        public virtual Employee EmployeeInchargeNavigation { get; set; }
        public virtual Field TaskFieldNavigation { get; set; }
        public virtual Plant TaskPlantNavigation { get; set; }
        public virtual Project TaskProjectNavigation { get; set; }
    }
}
