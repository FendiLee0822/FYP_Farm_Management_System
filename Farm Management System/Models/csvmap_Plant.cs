using Farm_Management_System.farm_Management_SystemModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Farm_Management_System
{
    public sealed class csvmap_Plant : CsvHelper.Configuration.ClassMap<Plant>
    {
        public csvmap_Plant()
        {
            string format = "dd/MM/yyyy HH:mm:ss";
            var msMY = CultureInfo.GetCultureInfo("ms-MY");

            Map(m => m.PlantId);
            Map(m => m.PlantName);
            Map(m => m.PlantBugTime);
            Map(m => m.PlantFloweringTime);
            Map(m => m.PlantSetTime);
            Map(m => m.PlantGrowthTime);
            Map(m => m.PlantRipeningTime);
            Map(m => m.AdminUpdate);
            Map(m => m.PlantUpdateTime)
                .TypeConverterOption.Format(format);
              //.TypeConverterOption.CultureInfo(msMY).Index(5);
        }
    }
}
