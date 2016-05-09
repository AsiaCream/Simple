using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simple.Models
{
    public class LockHelpfulOrder
    {
        public int Id { get; set; }
        public IsFinish IsFinish { get; set; }
        //操作对应的helpful订单
        [ForeignKey("HelpfulPreOrder")]
        public int HelpfulPreOrderId { get; set; }
        public virtual HelpfulPreOrder HelpfulPreOrder { get; set; }

        //操作管理员的ID
        [ForeignKey("User")]
        public string AdminId { get; set; }
        public virtual User Admin { get; set; }
    }
}
