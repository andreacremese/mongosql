using System;
using System.Collections.Generic; 

namespace DataObjects {
    public class Report {
        public Report() {
            Engineers = new List<Engineer>();
        }
        public Int32 Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public String Title { get; set; }
        public ICollection<Engineer> Engineers { get; set; }
        public Topic Topic { get; set; }
        public Partner Partner { get; set; }
    }
}
