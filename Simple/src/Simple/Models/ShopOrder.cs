using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simple.Models
{
    public class ShopOrder
    {
        public int Id { get; set;}
        public string Title { get; set; }
        public int MaxOneDay { get; set; }
        public int MaxOneEvaluation { get; set; }
        
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
