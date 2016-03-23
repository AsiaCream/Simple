using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simple.Models
{
    public class Poundage
    {
        public int Id { get; set; }
        public double OrderCost { get; set; }//下单手续费 30
        public double AddressCost { get; set; }//改地址手续费5
        public double SearchCost { get; set; }//搜索手续费10
        public double ImageCost { get; set; }//图片评价手续费20
        public double TotalCost { get; set; }

        [ForeignKey("PreOrder")]
        public int PreOrderId { get; set; }
        public PreOrder PreOrder { get; set; }
    }
}
