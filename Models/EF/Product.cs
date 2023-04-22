using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopLearning.Models.EF
{
    [Table("tb_Product")]
    public class Product:CommonAbtract
    {
        public Product()
        {
            this.ProductImage = new HashSet<ProductImage>();
            this.OrderDetails = new HashSet<OrderDetail>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Title { get; set; }
        [StringLength(150)]
        public string Alias { get; set; }
        [StringLength(150)]
        public string Description { get; set; }
        
        public string ProductCode { get; set; }
        [StringLength(150)]
        public string SeoTitle { get; set; }
        [StringLength(150)]
        public string SeoDescription { get; set; }
        [AllowHtml]
        public string Detail { get; set; }
        [StringLength(150)]
        public string SeoKeywords { get; set; }
        [StringLength(150)]
        public string Image { get; set; }
        public decimal Price { get; set; }
        public decimal? PriceSale { get; set; }
        public bool IsHome { get; set; }
        public bool IsFeature { get; set; }
        public bool IsHot { get; set; }
        public bool IsSale { get; set; }
        public int Quantity { get; set; }
        public int ViewCount { get; set; }
        public int ProductCategoryId { get; set; }
       
        public bool IsActive { get; set; }
        public virtual ProductCategory ProductCategories { get; set; }
        public virtual ICollection<ProductImage> ProductImage { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}