﻿namespace Marketplace.Models
{
    public class CartItem
    {
        public long ProductId { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }  

        public int Quantity {  get; set; }

        public string PhotoPath { get; set; }
    }
}
