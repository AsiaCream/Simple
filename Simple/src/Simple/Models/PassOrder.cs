using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simple.Models
{
    public class PassOrder
    {
        public int Id { get; set; }
        public DateTime PassTime { get; set; }

        [ForeignKey("PreOrder")]
        public int PreOrderId { get; set; }
        public virtual PreOrder PreOrder { get; set; }
    }
}
