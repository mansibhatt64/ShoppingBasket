using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasket.Model
{
    public class Item
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public Item(string name, double price)
        {
            this.Name = name;
            this.Price  = price;
            this.Id = Guid.NewGuid();
        }
    }
}
