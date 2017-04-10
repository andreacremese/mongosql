using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataObjects
{
    public class Partner {
        public Int32 Id { get; set; }
        [Index]
        [MaxLength(40)]
        public String Name { get; set; }
    }
}
