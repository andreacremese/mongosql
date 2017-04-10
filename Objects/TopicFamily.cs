using System;
using System.Collections.Generic;

namespace DataObjects {
    public class TopicFamily {
        public Int32 Id { get; set; }
        public List<Topic> Topics { get; set; }
        public String Name { get; set; }
    }
}
