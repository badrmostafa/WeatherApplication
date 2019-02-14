using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WeatherApplication.Models.Classes
{
    public class Download
    {
        public int DownloadID { get; set; }
        public string Head { get; set; }
        public string Description { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}