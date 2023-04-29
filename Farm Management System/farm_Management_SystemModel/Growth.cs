using System;
using System.Collections.Generic;

#nullable disable

namespace Farm_Management_System.farm_Management_SystemModel
{
    public partial class Growth
    {
        public int GrowthId { get; set; }
        public int GrowthProject { get; set; }
        public int GrowthPlant { get; set; }
        public int GrowthField { get; set; }
        public string GrowthStatus { get; set; }
        public int UserUpdate { get; set; }
        public DateTime GrowthUpdateTime { get; set; }

        public virtual Field GrowthFieldNavigation { get; set; }
        public virtual Plant GrowthPlantNavigation { get; set; }
        public virtual Project GrowthProjectNavigation { get; set; }
        public virtual User UserUpdateNavigation { get; set; }
    }
}
