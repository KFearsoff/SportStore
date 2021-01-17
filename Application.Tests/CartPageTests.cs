using Application.Models;
using Application.Pages;
using Moq;
using System.Linq;
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

            //Act
            CartModel cartModel = new CartModel(mockRepo.Object, testCart);
            cartModel.OnGet("myUrl");

            //Assert
            Assert.Equal(2, cartModel.Cart.Lines.Count);
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

            //Act
            CartModel cartModel = new CartModel(mockRepo.Object, testCart);
            cartModel.OnPost(1, "myUrl");

            //Assert
            Assert.Single(testCart.Lines);
            Assert.Equal("P1", testCart.Lines.First().Product.Name);
            Assert.Equal(1, testCart.Lines.First().Quantity);
        }
    }
}
