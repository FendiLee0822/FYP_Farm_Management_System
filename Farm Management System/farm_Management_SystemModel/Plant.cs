using System;
using System.Collections.Generic;

#nullable disable

namespace Farm_Management_System.farm_Management_SystemModel
{
    public partial class Plant
    {
        public Plant()
        {
            Growths = new HashSet<Growth>();
            Tasks = new HashSet<Task>();
        }

        public int PlantId { get; set; }
        public string PlantName { get; set; }
        public string PlantBugTime { get; set; }
        public string PlantFloweringTime { get; set; }
        public string PlantSetTime { get; set; }
        public string PlantGrowthTime { get; set; }
        public string PlantRipeningTime { get; set; }
        public int AdminUpdate { get; set; }
        public DateTime PlantUpdateTime { get; set; }

        public virtual Admin AdminUpdateNavigation { get; set; }
        public virtual ICollection<Growth> Growths { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
