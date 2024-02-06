using ShoppingBasket.Entities;
using ShoppingBasket.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasket.Services
{
    public class BasketService : IBasketService
    {
        private Dictionary<Guid, Basket> baskets = new Dictionary<Guid, Basket>();
        private readonly List<Offer> offers;

        public BasketService(List<Offer> offers)
        {
            this.offers = offers;
        }
        public bool AddBasket(Basket basket)
        {
            if(baskets.ContainsKey(basket.Id))
            {
                Console.WriteLine("Basket already present");
                return false;
            }
            baskets[basket.Id] = basket;
            return true;
        }

        public bool RemoveBasket(Guid basketId)
        {
            if (!baskets.ContainsKey(basketId))
            {
                Console.WriteLine("invalid basket");
                return false;
            }
            baskets.Remove(basketId);
            return true;
        }

        public bool AddToBasket(Guid basketId, Item item)
        {
            if (!baskets.ContainsKey(basketId))
            {
                Console.WriteLine("invalid basket");
                return false;
            }

            BasketItem basketItem;
            if (baskets[basketId].Items.ContainsKey(item.Id))
            {
                basketItem = baskets[basketId].Items[item.Id];
                basketItem.Quantity = basketItem.Quantity + 1;
            }
            else
            {
                basketItem= new BasketItem(item);
            }
            baskets[basketId].Items[item.Id] = basketItem;
            return true;
        }

        public bool RemoveFromBasket(Guid basketId, Item item)
        {
            if (!baskets.ContainsKey(basketId))
            {
                Console.WriteLine("invalid basket");
                return false;
            }

            baskets[basketId].Items.Remove(item.Id);
            return true;
        }

        public List<BasketItem> GetAllItems(Guid basketId)
        {
            if (!baskets.ContainsKey(basketId))
            {
                Console.WriteLine("invalid basket");
                return null;
            }

            return baskets[basketId].Items.Values.ToList();
        }

        public double GetTotalPrice(Guid basketId)
        {
            double price = 0;
            Dictionary<Guid, BasketItem> items = baskets[basketId].Items;
            foreach(var item in items)
            {
                price += GetDiscountedPrice(item.Value.Item, item.Value.Quantity);
            }
            return price;
        }

        private double GetDiscountedPrice(Item item, int quantity)
        {
            double price = 0;
            List<Offer> applicableOffers = offers.Where(x => x.ItemId == item.Id && x.Quantity <= quantity).ToList<Offer>();
            foreach (var offer in applicableOffers)
            {
                price = offer.Price + GetDiscountedPrice(item, quantity - offer.Quantity);
                return price;
            }
            price += item.Price * quantity;
            return price;
        }
    }
}
