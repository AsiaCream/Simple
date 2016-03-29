using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simple.Models
{
    public class HelpfulPreOrder
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public string Country { get; set; }
        public string Url { get; set; }
        public int Times { get; set; }//单子次数
        public string Review1 { get; set; } //Review标题1-10
        public string Review2 { get; set; }
        public string Review3 { get; set; }
        public string Review4 { get; set; }
        public string Review5 { get; set; }
        public string Review6 { get; set; }
        public string Review7 { get; set; }
        public string Review8 { get; set; }
        public string Review9 { get; set; }
        public string Review10 { get; set; }
        public bool HelpfulType { get; set; } //Helpful类型
        public bool IsCollection { get; set; }//是否收藏产品
        public double PayFor { get; set; }//需要支付金额
        public DateTime PostTime { get; set; }//提交时间
        public DateTime DrawTime { get; set; }//审核时间

        public State State { get; set; } //是否锁定
        public Draw Draw { get; set; }//是否审核


        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User User { get; set; }
        
    }
}
