using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherApplication.Models.Classes
{
    public class Feature
    {
        public int FeatureID { get; set; }
        public string Head1 { get; set; }
        public string Head2 { get; set; }
        public string Icon { get; set; }
        public string Head3 { get; set; }
        public string Description { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        //NavigationProperty
        public List<Quality> Qualities { get; set; }
    }
}