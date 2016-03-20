using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simple.Models
{
    public class MyWallet
    {
        public int Id { get; set; }
        public double Update { get; set; }
        public double Rest { get; set; }
        public double Cost { get; set; }
        public double Account { get; set; }
        public double PayFor { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
