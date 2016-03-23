using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.Models
{
    public class System
    {
        public int Id { get; set; }
        public string PlatType { get; set; }
        public string OrderType { get; set; }
        public double SetCost { get; set; }
        public bool Extension { get; set; }
    }
}
