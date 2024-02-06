using ShoppingBasket.Model;
using ShoppingBasket.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasket
{
    internal class Program
    {

        public static void Main(string[] args)
        {
            BasketClient();
        }

        private static void BasketClient()
        {
            // create items
            Item apple = new Item("Apple", 35);
            Item banana = new Item("Banana", 20);
            Item melon = new Item("Melon", 50);
            Item lime = new Item("Lime", 15);

            // add offers
            List<Offer> offers = new List<Offer>();
            // offer - buy one get one free on Melons
            Offer buyOneGetOneOffer = new Offer(melon.Id, 2, 50);
            // offer - three for the price of two for limes
            Offer threeItemOffer = new Offer(lime.Id, 3, 30);
            // offer on 1 bananas
            Offer singleItemOffer = new Offer(banana.Id, 1, 15);

            IBasketService basketService = new BasketService(
                                            new List<Offer> { singleItemOffer, buyOneGetOneOffer, threeItemOffer });

            // Create a basket
            Basket basket = new Basket();
            basketService.AddBasket(basket);
            basketService.AddToBasket(basket.Id, banana);
            basketService.AddToBasket(basket.Id, banana);
            basketService.AddToBasket(basket.Id, melon);
            basketService.AddToBasket(basket.Id, lime);
            Console.WriteLine("Basket total price - " + basketService.GetTotalPrice(basket.Id));
            Console.ReadLine();
        }
    }
}
