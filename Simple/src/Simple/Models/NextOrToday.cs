using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.Models
{
    public class NextOrToday
    {
        public int Id { get; set; }
        public string Type { get; set; }//隔天下单，当天下单属性
        public double Price { get; set; }//对应价格
        public string Note { get; set; }//说明
    }
}
