﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShopLearning.Models.EF
{
    [Table("tb_SystemSetting")]
    public class SystemSetting:CommonAbtract
    {
        [Key]
        [StringLength(150)]
        public string SettingKey { get; set; }
        [StringLength(4000)]
        public string SettingValue { get; set; }
        [StringLength(4000)]
        public string SettingDescription { get; set; }
         
        
    }
}