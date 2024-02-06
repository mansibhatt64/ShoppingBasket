using ShoppingBasket.Entities;
using ShoppingBasket.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasket.Services
{
    public interface IBasketService
    {
        bool AddBasket(Basket basket);
        bool RemoveBasket(Guid basketId);
        bool AddToBasket(Guid basketId, Item item);
        bool RemoveFromBasket(Guid basketId, Item item);
        double GetTotalPrice(Guid basketId);
        List<BasketItem> GetAllItems(Guid basketId);
    }
}
