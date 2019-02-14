using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WeatherApplication.Models.Classes
{
    public class Quality
    {
        public int QualityID { get; set; }
        public int ApplicationID { get; set; }
        public int FeatureID { get; set; }
        //NavigationProperty
        public Application Application { get; set; }
        public Feature Feature { get; set; }
        [Timestamp]
        public byte[] RowVwrsion { get; set; }
    }
}