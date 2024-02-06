using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingBasket.Entities;
using ShoppingBasket.Model;
using ShoppingBasket.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasket.Services.Tests
{
    [TestClass()]
    public class BasketServiceTest
    {
        private IBasketService basketService;
        [TestInitialize]
        public void Setup()
        {
            List<Offer> offers = new List<Offer>();
            basketService = new BasketService(offers);
        }

        [TestMethod()]
        public void AddBasket_NewBasket_ReturnsTrue()
        {
            bool actualValue = basketService.AddBasket(new Basket());

            Assert.IsTrue(actualValue);
        }

        [TestMethod]
        public void AddBasket_BasketAlreadyExists_ReturnsFalse()
        {
            Basket basket = new Basket();
            basketService.AddBasket(basket);

            bool actualValue = basketService.AddBasket(basket);

            Assert.IsFalse(actualValue);
        }

        [TestMethod()]
        public void RemoveBasket_ValidBasket_ReturnsTrue()
        {
            Basket basket = new Basket();
            basketService.AddBasket(basket);
          
            bool actualValue = basketService.RemoveBasket(basket.Id);

            Assert.IsTrue(actualValue);
        }

        [TestMethod]
        public void RemoveBasket_InvalidBasket_ReturnsFalse()
        {
            bool actualValue = basketService.RemoveBasket(new Guid());

            Assert.IsFalse(actualValue);
        }

        [TestMethod()]
        public void AddToBasket_NewItem_ReturnsTrue()
        {
            Basket basket = new Basket();
            basketService.AddBasket(basket);
            Item item = new Item("Banana", 20);
          
            bool actualValue = basketService.AddToBasket(basket.Id, item);

            Assert.IsTrue(actualValue);
        }

        [TestMethod()]
        public void AddToBasket_SameItemMultipleTimes_ReturnsTrue()
        {
            Basket basket = new Basket();
            basketService.AddBasket(basket);
            Item item = new Item("Banana", 20);
            basketService.AddToBasket(basket.Id, item);
          
            bool actualValue = basketService.AddToBasket(basket.Id, item);

            Assert.IsTrue(actualValue);
        }

        [TestMethod]
        public void AddToBasket_InvalidBasket_ReturnsFalse()
        {
            Basket basket = new Basket();
          
            bool actualValue = basketService.AddToBasket(basket.Id, new Item("Banana", 20));

            Assert.IsFalse(actualValue);
        }

        [TestMethod()]
        public void RemoveFromBasket_InvalidBasket_ReturnsFalse()
        {
            Basket basket = new Basket();
          
            bool actualValue = basketService.RemoveFromBasket(basket.Id, new Item("banana", 30));

            Assert.IsFalse(actualValue);
        }

        [TestMethod()]
        public void RemoveFromBasket_ValidBasket_ReturnsTrue()
        {
            Basket basket = new Basket();
            basketService.AddBasket(basket);
            Item item = new Item("banana", 20);
            basketService.AddToBasket(basket.Id, item);
          
            bool actualValue = basketService.RemoveFromBasket(basket.Id, item);

            Assert.IsTrue(actualValue);
        }

        [TestMethod]
        public void GetAllItems_ValidBasket_ReturnsCorrectItems()
        {
            Item orange = new Item("Orange", 30.50);
            Item grapes = new Item("Grapes", 30.50);
            Item pomegranate = new Item("Pomegranate", 30.50);

            Basket basket = new Basket();
            basketService.AddBasket(basket);
            basketService.AddToBasket(basket.Id, orange);
            basketService.AddToBasket(basket.Id, grapes);
            basketService.AddToBasket(basket.Id, pomegranate);

            List<BasketItem> basketItems = basketService.GetAllItems(basket.Id);

            Assert.AreEqual(orange, basketItems[0].Item);
            Assert.AreEqual(grapes, basketItems[1].Item);
            Assert.AreEqual(pomegranate, basketItems[2].Item);
        }

        [TestMethod]
        public void GetAllItems_InvalidBasket_ReturnsNull()
        {
            Basket basket = new Basket();

            List<BasketItem> actualValue = basketService.GetAllItems(basket.Id);

            Assert.IsNull(actualValue);
        }

        [TestMethod()]
        public void GetTotalPrice_ProductsWithSingleQuantityWithoutOffer_ReturnsCorrectTotalPrice()
        {
            List<Offer> lstOffer = new List<Offer>();
            IBasketService basketService = new BasketService(lstOffer);

            Basket basket = new Basket();
            basketService.AddBasket(basket);
            basketService.AddToBasket(basket.Id, new Item("Apple", 35));
            basketService.AddToBasket(basket.Id, new Item("Banana", 20));
            basketService.AddToBasket(basket.Id, new Item("Melon", 50));
            basketService.AddToBasket(basket.Id, new Item("Lime", 15));
            
            double totalPrice = basketService.GetTotalPrice(basket.Id);

            Assert.AreEqual(120, totalPrice);
        }

        [TestMethod()]
        public void GetTotalPrice_ProductWithSingleAndMultipleQuantityWithoutOffer_ReturnsCorrectTotalPrice()
        {
            List<Offer> lstOffer = new List<Offer>();
            IBasketService basketService = new BasketService(lstOffer);

            Basket basket = new Basket();
            basketService.AddBasket(basket);
            basketService.AddToBasket(basket.Id, new Item("Apple", 35));
            basketService.AddToBasket(basket.Id, new Item("Apple", 35));
            basketService.AddToBasket(basket.Id, new Item("Melon", 50));
            basketService.AddToBasket(basket.Id, new Item("Lime", 15));
            basketService.AddToBasket(basket.Id, new Item("Lime", 15));

            double totalPrice = basketService.GetTotalPrice(basket.Id);

            Assert.AreEqual(150, totalPrice);
        }

        [TestMethod()]
        public void GetTotalPrice_AllItemsWithOfferedPrice_ReturnsTotalWithOfferApplied()
        {
            // create items
            Item apple = new Item("Apple", 35);
            Item banana = new Item("Banana", 20);
            Item melon = new Item("Melon", 50);
            Item lime = new Item("Lime", 15);

            // offer - buy one get one free on Melons
            Offer buyOneGetOneOffer = new Offer(melon.Id, 2, 50);
            // offer - three for the price of two for limes
            Offer threeItemOffer = new Offer(lime.Id, 3, 30);
            // offer on 1 bananas
            Offer singleItemOffer = new Offer(banana.Id, 1, 15);
            List<Offer> lstOffer = new List<Offer>(){ singleItemOffer, buyOneGetOneOffer, threeItemOffer };

            IBasketService basketService = new BasketService(lstOffer);

            Basket basket = new Basket();
            basketService.AddBasket(basket);
            basketService.AddToBasket(basket.Id, banana);
            basketService.AddToBasket(basket.Id, melon);
            basketService.AddToBasket(basket.Id, melon);
            basketService.AddToBasket(basket.Id, lime);
            basketService.AddToBasket(basket.Id, lime);
            basketService.AddToBasket(basket.Id, lime);

            double totalPrice = basketService.GetTotalPrice(basket.Id);

            Assert.AreEqual(95, totalPrice);
        }
        [TestMethod()]
        public void GetTotalPrice_ItemWithAndWithoutOffers_ReturnsCorrectTotalPrice()
        {
            // create items
            Item apple = new Item("Apple", 35);
            Item banana = new Item("Banana", 20);
            Item melon = new Item("Melon", 50);
            Item lime = new Item("Lime", 15);
            Item kiwi = new Item("Kiwi", 40);

            // offer - buy one get one free on Melons
            Offer buyOneGetOneOffer = new Offer(melon.Id, 2, 50);
            // offer - three for the price of two for limes
            Offer threeItemOffer = new Offer(lime.Id, 3, 30);
            // offer on 1 bananas
            Offer singleItemOffer = new Offer(banana.Id, 1, 15);
            //offer on 4 Kiwi
            Offer fourItemOffer = new Offer(kiwi.Id, 4, 100);
            List<Offer> lstOffer = new List<Offer>() { singleItemOffer, buyOneGetOneOffer, threeItemOffer, fourItemOffer };

            IBasketService basketService = new BasketService(lstOffer);

            Basket basket = new Basket();
            basketService.AddBasket(basket);
            // No Offer - 35
            basketService.AddToBasket(basket.Id, apple);
            //Offer on one banana - 15+15 = 30
            basketService.AddToBasket(basket.Id, banana);
            basketService.AddToBasket(basket.Id, banana);
            // Offer on 2 melon + 1 melon = 50 
            basketService.AddToBasket(basket.Id, melon);
            basketService.AddToBasket(basket.Id, melon);
            // offer on 3 lime - 30
            basketService.AddToBasket(basket.Id, lime);
            basketService.AddToBasket(basket.Id, lime);
            basketService.AddToBasket(basket.Id, lime);

            // 35+ 30+ 50 + 30 = 145
            double totalPrice = basketService.GetTotalPrice(basket.Id);

            Assert.AreEqual(145, totalPrice);
        }

        [TestMethod()]
        public void GetTotalPrice_ItemWithOfferOnPartialQuantity_ReturnsCorrectTotalPrice()
        {
            // create item
            Item melon = new Item("Melon", 50);
            // offer - buy one get one free on Melons
            Offer buyOneGetOneOffer = new Offer(melon.Id, 2, 50);
            List<Offer> lstOffer = new List<Offer>() { buyOneGetOneOffer };
            IBasketService basketService = new BasketService(lstOffer);
            Basket basket = new Basket();
            basketService.AddBasket(basket);
            // Offer on 2 melon + 1  melon without offer = 50 + 50 = 100
            basketService.AddToBasket(basket.Id, melon);
            basketService.AddToBasket(basket.Id, melon);
            basketService.AddToBasket(basket.Id, melon);

            double totalPrice = basketService.GetTotalPrice(basket.Id);

            Assert.AreEqual(100, totalPrice);
        }
    }
}