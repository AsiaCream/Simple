using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.Models
{
    public class LockOrder
    {
        public int Id { get; set; }
        //操作对应的helpful订单
        [ForeignKey("PreOrder")]
        public int PreOrderId { get; set; }
        public virtual PreOrder PreOrder { get; set; }

        //操作管理员的ID
        [ForeignKey("User")]
        public string AdminId { get; set; }
        public virtual User Admin { get; set; }
    }
}
