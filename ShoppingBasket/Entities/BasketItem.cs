using ShoppingBasket.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasket.Entities
{
    public class BasketItem
    {
        public Item Item{ get; set; }
        public int Quantity { get; set; }

        public BasketItem(Item item)
        {
            this.Item = item;
            this.Quantity = 1;
        }
    }
}
