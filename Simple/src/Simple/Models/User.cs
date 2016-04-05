﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace Simple.Models
{
    public class User:IdentityUser
    {
        [MaxLength(16)]
        public string Name { get; set; }
        public int? QQ { get; set; }//QQ
        public string Question{ get; set; }//找回问题
        public string Answer { get; set; }//找回答案
        public int Level { get; set; } //会员等级
    }
}
