using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherApplication.Models.Classes
{
    public class Application
    {
        public int ApplicationID { get; set; }
        public string Head { get; set; }
        public int Degree { get; set; }
        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public string Text3 { get; set; }
        public string Image { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        //NavigationProperty
        public List<Quality> Qualities { get; set; }
    }
}