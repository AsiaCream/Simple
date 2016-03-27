using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.Models
{
    public class FindType
    {
        public int Id { get; set; }
        public string Type { get; set; }//查找店铺方式，如搜索进入，连接进入
        public double Price { get; set; }//查找店铺方式价格
        public string Note { get; set; }//价格说明
    }
}
