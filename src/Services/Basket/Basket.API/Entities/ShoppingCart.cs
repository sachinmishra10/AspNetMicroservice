using System.Collections.Generic;

namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        public string UserName { get; set; }
        public List<ShoppingCartItems> ShoppingCartItems { get; set; } = new List<ShoppingCartItems>();

        public ShoppingCart()
        {

        }
        public ShoppingCart(string userName)
        {
            UserName = userName;

        }
        public decimal  Total { 
            get { 
                decimal total = 0;
                foreach(var item in ShoppingCartItems)
                {
                    total += item.Price;
                }
                return total;
            } 
        }
    }
}
