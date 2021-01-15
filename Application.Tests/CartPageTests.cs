﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Moq;
using Application.Models;
using Application.Pages;
using System.Text.Json;
using Xunit;

namespace Application.Tests
{
    public class CartPageTests
    {
        [Fact]
        public void CanLoadCart()
        {
            //Arrange - Products
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            //Arrange - Mock Repository
            Mock<IStoreRepository> mockRepo = new Mock<IStoreRepository>();
            mockRepo.Setup(r => r.Products).Returns((new Product[] { p1, p2 }).AsQueryable());
            //Arrange - Cart
            Cart testCart = new Cart();
            testCart.AddItem(p1, 2);
            testCart.AddItem(p2, 1);
            //Arrange - Mock Session
            Mock<ISession> mockSession = new Mock<ISession>();
            byte[] data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(testCart));
            mockSession.Setup(s => s.TryGetValue(It.IsAny<string>(), out data));
            //Arrange - Mock HttpContext
            Mock<HttpContext> mockContext = new Mock<HttpContext>();
            mockContext.Setup(c => c.Session).Returns(mockSession.Object);

            //Act
            CartModel cartModel = new CartModel(mockRepo.Object)
            {
                PageContext = new PageContext(new ActionContext
                {
                    HttpContext = mockContext.Object,
                    RouteData = new RouteData(),
                    ActionDescriptor = new PageActionDescriptor()
                })
            };
            cartModel.OnGet("myUrl");

            //Assert
            Assert.Equal(2, cartModel.Cart.Lines.Count());
            Assert.Equal("myUrl", cartModel.ReturnUrl);
        }

        [Fact]
        public void CanUpdateCart()
        {
            //Arrange - Mock Repository
            Mock<IStoreRepository> mockRepo = new Mock<IStoreRepository>();
            mockRepo.Setup(r => r.Products).Returns((new Product[] { new Product { ProductID = 1, Name = "P1" } }).AsQueryable());
            //Arrange - Cart
            Cart testCart = new Cart();
            //Arrange - Mock Session
            Mock<ISession> mockSession = new Mock<ISession>();
            mockSession.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
                .Callback<string, byte[]>((key, val) => testCart = JsonSerializer.Deserialize<Cart>(Encoding.UTF8.GetString(val)));
            //Arrange - Mock HttpContext
            Mock<HttpContext> mockContext = new Mock<HttpContext>();
            mockContext.Setup(c => c.Session).Returns(mockSession.Object);

            //Act
            CartModel cartModel = new CartModel(mockRepo.Object)
            {
                PageContext = new PageContext(new ActionContext
                {
                    HttpContext = mockContext.Object,
                    RouteData = new RouteData(),
                    ActionDescriptor = new PageActionDescriptor()
                })
            };
            cartModel.OnPost(1, "myUrl");

            //Assert
            Assert.Single(testCart.Lines);
            Assert.Equal("P1", testCart.Lines.First().Product.Name);
            Assert.Equal(1, testCart.Lines.First().Quantity);
        }
    }
}
