using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simple.Models
{
    public enum State
    {
        未锁定,
        锁定,
    }
    public enum Draw
    {
        待审核,
        通过,
        未通过,
    }
    public class PreOrder
    {   
        public int Id { get; set; }
        public string PreOrderNumber { get; set; }
        public double Rate { get; set; }
        public string FindType { get; set; }
        public string GoodsUrl { get; set; }
        public string ShopName { get; set; }
        public double GoodsCost { get; set; }
        public double Freight { get; set; } //运费
        public string OrderType { get; set; } 
        public string NextOrToday { get; set; }//隔天下单或者首天下单
        public bool AvoidWeekend { get; set; }//是否避开周末
        public bool Extension { get; set; }//是否免费推广
        public int CommentTime { get; set; }//评价时间
        public string Address { get; set; }
        public int FeedBackStar { get; set; }
        public string FeedBackModel { get; set; }
        public string FeedBackContent { get; set; }
        public string Note { get; set; }
        public string ImageUrl1 { get; set; }
        public string ImageUrl2 { get; set; }
        public string ImageUrl3 { get; set; }
        public double Total { get; set; } //总价
        public int Times { get; set; }//刷单次数

        public State State { get; set; } //锁定状态

        public Draw Draw { get; set; }//审核情况
        
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
