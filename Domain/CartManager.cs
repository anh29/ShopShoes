using ShopOnline.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopOnline.Domain
{
    public class CartManager
    {
        public List<CartItem> Items { get; set; }
        public CartManager()
        {
            this.Items = new List<CartItem>();
        }
        public void AddToCart(CartItem item, int Quantity)
        {
            var checkExits = Items.FirstOrDefault(x => x.ProductId == item.ProductId);
            if (checkExits != null)
            {
                checkExits.Quantity += Quantity;
                checkExits.TotalPrice = checkExits.Quantity * (checkExits.Product.PriceSale > 0 ? (decimal)checkExits.Product.PriceSale : checkExits.Product.Price);
            }
            else
            {
                Items.Add(item);
            }
        }
        public void Remove(int id)
        {
            var checkExits = Items.SingleOrDefault(x => x.ProductId == id);
            if (checkExits != null)
            {
                Items.Remove(checkExits);
            }
        }
        public void UpdateQuantity(int id, int quantity)
        {
            var checkExits = Items.SingleOrDefault(x => x.ProductId == id);
            if (checkExits != null)
            {
                checkExits.Quantity = quantity; 
                checkExits.TotalPrice = checkExits.Quantity * (checkExits.Product.PriceSale > 0 ? (decimal)checkExits.Product.PriceSale : checkExits.Product.Price);
            }
        }
        public decimal GetTotalPrice()
        {
            return Items.Sum(x => x.TotalPrice);
        }
        public int GetTotalQuantity()
        {
            return Items.Sum(x => x.Quantity);
        }
        public void ClearCart()
        {
            Items.Clear();
        }
    }
}