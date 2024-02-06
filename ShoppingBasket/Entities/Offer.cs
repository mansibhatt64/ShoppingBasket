using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasket.Model
{
    public class Offer
    {
        public Guid ItemId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        

        public Offer(Guid itemId, int quantity, double price)
        {
            this.ItemId = itemId;
            this.Quantity = quantity;
            this.Price = price;
        }
    }
}
