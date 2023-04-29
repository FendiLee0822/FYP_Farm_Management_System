using System;
using System.Collections.Generic;

#nullable disable

namespace Farm_Management_System.farm_Management_SystemModel
{
    public partial class Field
    {
        public Field()
        {
            Growths = new HashSet<Growth>();
            Tasks = new HashSet<Task>();
        }

        public int FieldId { get; set; }
        public string FieldName { get; set; }
        public double FieldLong { get; set; }
        public double FieldWidth { get; set; }

        public virtual ICollection<Growth> Growths { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
