using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace WeatherApplication.Models.Classes
{
    public class WeatherContext:DbContext
    {
        //ConnectionString
        public WeatherContext():base("WeatherContext")
        { }
        //DbSet<>
        public DbSet<Application> Applications { get; set; }
        public DbSet<Quality> Qualities { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Slide> Slides { get; set; }
        public DbSet<Description> Descriptions { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Download> Downloads { get; set; }
    }
}