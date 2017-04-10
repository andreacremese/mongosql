using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataObjects
{
    public class Engineer {
        public Int32 Id { get; set; }
        [Index]
        [MaxLength(40)]
        public String Name { get; set; }
        [Index]
        [MaxLength(40)]
        public String Email { get; set; }
        public ICollection<Report> Reports { get; set; }
    }
}
