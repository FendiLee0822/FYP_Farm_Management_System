using Farm_Management_System.farm_Management_SystemModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farm_Management_System.Models
{
    public class growth_Creation_Model
    {
        public Growth Growth { get; set; }
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<Plant> Plants { get; set; }
        public IEnumerable<Field> Fields { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}
