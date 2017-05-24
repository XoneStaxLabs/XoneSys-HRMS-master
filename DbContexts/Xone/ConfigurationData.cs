
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;


namespace DbContexts.Xone
{
    internal sealed class ConfigurationData : DbMigrationsConfiguration<XoneContext>
    {
        public ConfigurationData()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
         
        protected override void Seed(XoneContext context)
        {

            //context.EM_Language.AddOrUpdate(

            //     new EM_Language { Lng_ID = 1, Lng_Name = "English", Lng_Alignment = "Left", Lng_Isactive = true, Lng_Isdeleted = false },
            //     new EM_Language { Lng_ID = 2, Lng_Name = "Chineese", Lng_Alignment = "Left", Lng_Isactive = true, Lng_Isdeleted = false },
            //     new EM_Language { Lng_ID = 3, Lng_Name = "Arabic", Lng_Alignment = "Right", Lng_Isactive = true, Lng_Isdeleted = false }

            //     );


        }
    }
}