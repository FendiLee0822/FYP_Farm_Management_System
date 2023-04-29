using System;
using System.Collections.Generic;

#nullable disable

namespace Farm_Management_System.farm_Management_SystemModel
{
    public partial class Inventory
    {
        public int InventoryId { get; set; }
        public string InventoryName { get; set; }
        public int InventoryAmount { get; set; }
        public int UserUpdate { get; set; }
        public DateTime InventoryUpdateTime { get; set; }

        public virtual User UserUpdateNavigation { get; set; }
    }
}
