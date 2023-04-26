using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShopLearning.Models.EF
{
    [Table("tb_ProductCategory")]
    public class ProductCategory:CommonAbtract
    {
        public ProductCategory()
        {
            this.Products = new HashSet<Product>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id {get;set; }
        [Required]
        [StringLength(150)]
        public string Title {get;set; }
        public string Description { get; set; }
        [Required]
        [StringLength(150)]
        public string Alias { get; set; }
        [StringLength(150)]
        public string Icon { get; set; }
        public string SeoTitle { get; set; }
        [StringLength(150)]
        public string SeoDescription { get; set; }
        [StringLength(150)]
        public string SeoKeywords { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}