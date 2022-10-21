using System;
using System.ComponentModel.DataAnnotations;
namespace MVCWebApp.Models
{
    public class CookiePreferences
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Value { get; set; }

        [DataType(DataType.Date)]
        public DateTime SettingDate { get; set; }


    }
}
