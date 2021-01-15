using Application.Controllers;
using Application.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Application.Tests
{
    public class OrderControllerTests
    {
        [Fact]
        public void CannotCheckoutEmptyCart()
        {
            //Arrange - Mock Repository
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            //Arrange - Empty Cart
            Cart cart = new Cart();
            //Arrange - Order
            Order order = new Order();
            //Arrange - Controller
            OrderController target = new OrderController(mock.Object, cart);

            //Act - Checkout
            ViewResult result = target.Checkout(order) as ViewResult;

            //Assert - order hasn't been stored
            mock.Verify(r => r.SaveOrder(It.IsAny<Order>()), Times.Never);
            //Assert - method returns the default view
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            //Assert - the model is invalid
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void CannotCheckoutInvalidShippingDetails()
        {
            //Arrange - Mock Repository
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            //Arrange - Cart with 1 item
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            //Arrange - Controller
            OrderController target = new OrderController(mock.Object, cart);
            //Arrange - ModelError
            target.ModelState.AddModelError("error", "error");

            //Act - Checkout
            ViewResult result = target.Checkout(new Order()) as ViewResult;

            //Assert - the order hasn't been stored
            mock.Verify(r => r.SaveOrder(It.IsAny<Order>()), Times.Never);
            //Assert - method returns the default view
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            //Assert - the model is invalid
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void CanCheckoutOrder()
        {
            //Arrange - Mock Repository
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            //Arrange - Cart with 1 item
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            //Arrange - Controller
            OrderController target = new OrderController(mock.Object, cart);

            //Act - Checkout
            RedirectToPageResult result = target.Checkout(new Order()) as RedirectToPageResult;

            //Assert - the order has been stored
            mock.Verify(r => r.SaveOrder(It.IsAny<Order>()), Times.Once);
            //Assert - the method redirects to the Completed action
            Assert.Equal("/Completed", result.PageName);
        }
    }
}
