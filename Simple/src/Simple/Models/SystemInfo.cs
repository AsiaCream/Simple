using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.Models
{
    public class SystemInfo
    {
        //系统设置
        public int Id { get; set; }
        public string BigTitle { get; set; }//大标题，对应现在的Simple
        public string SmallTitle { get; set; }//小标题，对应现在缩小后的S
        public string Url { get; set; }//公司链接地址
        public string Company { get; set; }//公司名称
    }
}
