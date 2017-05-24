using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Model.Models
{
    public class CommonFunctions
    {




        public string GetUnique(string Name)
        {
            string FileName = "";
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 3)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            string date = DateTime.Now.ToString("yyMMddHHmmssff");
            FileName = result + date + Path.GetExtension(Name);
            return FileName;
        }


    }
}