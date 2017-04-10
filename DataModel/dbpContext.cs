using DataObjects;
using System.Data.Entity;

namespace DataModel {
    public class dbpContext : DbContext {

        public dbpContext() : base("name=dbPerformanceContext") { }
        public DbSet<Engineer> Engineers { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<TopicFamily> TopicFamilies { get; set; }
    }
}

