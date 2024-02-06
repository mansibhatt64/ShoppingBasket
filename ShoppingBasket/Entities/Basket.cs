using ShoppingBasket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasket.Model
{
    public class Basket
    {
        public Guid Id { get; set; }
        public Dictionary<Guid, BasketItem> Items { get; set; }

        public Basket()
        {
            Items = new Dictionary<Guid, BasketItem>();
            Id = Guid.NewGuid();
        }
    }
}
