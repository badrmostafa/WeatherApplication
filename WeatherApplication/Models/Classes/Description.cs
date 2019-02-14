using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WeatherApplication.Models.Classes
{
    public class Description
    {
        public int DescriptionID { get; set; }
        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public string Text1 { get; set; }
        public int Degree1 { get; set; }
        public string Text2 { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Head1 { get; set; }
        public string Description1 { get; set; }
        public int Humidity { get; set; }
        public int Degree2 { get; set; }
        public string Text3 { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

    }
}