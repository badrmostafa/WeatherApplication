using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WeatherApplication.Models.Classes
{
    public class Slide
    {
        public int SlideID { get; set; }
      
        public string BackgroundImage1 { get; set; }
        public string BackgroundImage2 { get; set; }
        public string BackgroundImage3 { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set;}
    }
}