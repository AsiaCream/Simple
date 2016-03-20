using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simple.Models
{
    public class FailureOrder
    {
        public int Id { get; set; }
        public string Note { get; set; } //备注为何不通过审核

        [ForeignKey("PreOrder")]
        public int PreOrderId { get; set; }
        public virtual PreOrder PreOrder { get; set; }
    }
}
