﻿using System.Collections.Generic;

namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        public string UserName { get; set; }
        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

        public decimal TotalPrice {
            get
            {
                decimal total = 0;
                foreach(var item in Items)
                {
                    total += item.Quantity * item.Price;
                }
                return total;
            }
        }
        public ShoppingCart()
        {

        }
        public ShoppingCart(string username)
        {
            UserName = username;
        }
    }
}
