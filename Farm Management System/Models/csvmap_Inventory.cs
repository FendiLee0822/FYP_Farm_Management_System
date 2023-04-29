using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Farm_Management_System.farm_Management_SystemModel;
using System;
using System.Globalization;

public sealed class csvmap_Inventory : ClassMap<Inventory>
{
    /*public csvmap_Inventory()
    {
        string format = "dd/MM/yyyy HH:mm:ss";
        var msMY = CultureInfo.GetCultureInfo("ms-MY");

        Map(m => m.InventoryId);
        Map(m => m.InventoryName);
        Map(m => m.InventoryAmount);
        Map(m => m.UserUpdate);
        Map(m => m.InventoryUpdateTime)
            .TypeConverter<DateTimeConverter>()
            .TypeConverterOption.Format(format)
            .TypeConverterOption.CultureInfo(msMY)
            .Index(4);
    }*/

    public csvmap_Inventory()
    {
        string format = "dd/MM/yyyy HH:mm:ss";
        var msMY = CultureInfo.GetCultureInfo("ms-MY");

        Map(m => m.InventoryId);
        Map(m => m.InventoryName);
        Map(m => m.InventoryAmount);
        Map(m => m.UserUpdate);
        Map(m => m.InventoryUpdateTime)
            .TypeConverterOption.Format(format);
    }
}
