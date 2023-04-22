using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShopLearning.Models.EF
{
    [Table("tb_Adv")]
    public class Adv:CommonAbtract
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        [StringLength(150)]
        public string Title { get; set; }
        [StringLength(150)]
        public string Description {get; set; }
        [StringLength(150)]
        public string Image { get; set; }
        [StringLength(150)]
        public string Link { get; set; }
        public int Type { get; set; }
    }
}
