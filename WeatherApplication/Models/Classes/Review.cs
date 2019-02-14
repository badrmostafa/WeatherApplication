using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WeatherApplication.Models.Classes
{
    public class Review
    {
        public int ReviewID { get; set; }
        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }

}