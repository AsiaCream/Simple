using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.Models
{
    public class MemberLevel
    {
        public int Id { get; set; }
        public string Level { get; set; }
        public int OrderMax { get; set; }//Ebay订单最大成交量
        public int OrderMin { get; set; }
        public int HelpfulMax { get; set; }
        public int HelpfulMin { get; set; }//Helpful订单最小成交量
    }
}
