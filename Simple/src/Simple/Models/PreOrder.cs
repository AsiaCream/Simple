using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simple.Models
{
    public class PreOrder
    {
        public enum State
        {
            未锁定,
            锁定,
        }
        public enum Draw
        {
            通过,
            未通过,
        }
        public int Id { get; set; }
        public double Rate { get; set; }
        public string FindType { get; set; }
        public string GoodsUrl { get; set; }
        public string GoodsCost { get; set; }
        public string ShopName { get; set; }
        public double Freight { get; set; } //运费
        public string OrderType { get; set; } 
        public string NextOrToday { get; set; }
        public bool AvoidWeekend { get; set; }
        public bool Extension { get; set; }
        public DateTime CommentTime { get; set; }
        public string Address { get; set; }
        public string FeedBackStar { get; set; }
        public string FeedBackModel { get; set; }
        public string FeedBackContent { get; set; }
        public string Note { get; set; }
        public string ImageUrl { get; set; }
        public string Total { get; set; } //总价
        
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
