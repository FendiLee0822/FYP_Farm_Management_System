/*using Farm_Management_System.farm_Management_SystemModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Farm_Management_System
{
    public sealed class csvmap_User : CsvHelper.Configuration.ClassMap<User>
    {
        public csvmap_User()
        {
            string format = "dd/MM/yyyy hh:mm";
            var msMY = CultureInfo.GetCultureInfo("ms-MY");

            Map(m => m.UserId);
            Map(m => m.UserFname);
            Map(m => m.UserLname);
            Map(m => m.UserGender);
            Map(m => m.UserDob).TypeConverterOption.Format(format)
              .TypeConverterOption.CultureInfo(msMY).Index(4);
            Map(m => m.UserPwd);
            Map(m => m.UserRole);
        }
    }
}
*/

using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Farm_Management_System.farm_Management_SystemModel;
using System;
using System.Globalization;

public sealed class csvmap_User : ClassMap<User>
{
    public csvmap_User()
    {
        string format = "dd/MM/yyyy HH:mm:ss";
        var msMY = CultureInfo.GetCultureInfo("ms-MY");

        Map(m => m.UserId);
        Map(m => m.UserFname);
        Map(m => m.UserLname);
        Map(m => m.UserGender);
        Map(m => m.UserDob)
            .TypeConverter<DateTimeConverter>()
            .TypeConverterOption.Format(format)
            .TypeConverterOption.CultureInfo(msMY)
            .Index(4);
        Map(m => m.UserPwd);
        Map(m => m.UserRole);
    }
}


