using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Application.Controllers;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;

namespace Application.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void CanUseRepository()
        {
            //Arrange
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(p => p.Products).Returns((new Product[]
            {
                new Product{ProductID = 1, Name="P1"},
                new Product{ProductID=2, Name="P2"}
            }).AsQueryable<Product>());
            HomeController controller = new HomeController(mock.Object);

            //Act
            IEnumerable<Product> result = (controller.Index() as ViewResult).ViewData.Model as IEnumerable<Product>;

            //Assert
            Product[] prodArray = result.ToArray();
            Assert.True(prodArray.Length == 2);
            Assert.Equal("P1", prodArray[0].Name);
            Assert.Equal("P2", prodArray[1].Name);
        }
    }
}
