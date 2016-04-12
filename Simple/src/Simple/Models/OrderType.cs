using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.Models
{
    public class OrderType //下单 + feedback + review
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public double Price { get; set; }
        public string Note { get; set; }
    }
}
