using Farm_Management_System.farm_Management_SystemModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farm_Management_System.Models
{
    public class project_Detail_Model
    {
        public Project Projects { get; set; }
        public IEnumerable<farm_Management_SystemModel.Task> Tasks { get; set; }
        public IEnumerable<Growth> Growths { get; set; }
        public IEnumerable<Financial> Financials { get; set; }
    }
}
