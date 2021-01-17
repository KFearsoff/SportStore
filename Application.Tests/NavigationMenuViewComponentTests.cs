using Application.Components;
using Application.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Application.Tests
{
    public class NavigationMenuViewComponentTests
    {
        [Fact]
        public void CanSelectCategories()
        {
            //Arrange
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(r => r.Products).Returns((new Product[]
            {
                new Product { ProductID = 1, Name = "P1", Category = "Apples" },
                new Product { ProductID = 2, Name = "P2", Category = "Apples" },
                new Product { ProductID = 3, Name = "P3", Category = "Plums" },
                new Product { ProductID = 4, Name = "P4", Category = "Oranges" },
            }).AsQueryable());
            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);

            //Act
            IEnumerable<string> result = (IEnumerable<string>)(target.Invoke() as ViewViewComponentResult).ViewData.Model;

            //Assert
            string[] resultArray = result.ToArray();
            Assert.True(Enumerable.SequenceEqual(new string[] { "Apples", "Oranges", "Plums" }, result));
        }

        [Fact]
        public void IndicatesSelectedCategory()
        {
            //Arrange - Mock Repository
            string categoryToSelect = "Apples";
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(r => r.Products).Returns((new Product[]
            {
                new Product { ProductID = 1, Name = "P1", Category = "Apples" },
                new Product { ProductID = 4, Name = "P2", Category = "Oranges" }
            }).AsQueryable());
            //Arrange - Target
            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object)
            {
                ViewComponentContext = new ViewComponentContext
                {
                    ViewContext = new ViewContext { RouteData = new Microsoft.AspNetCore.Routing.RouteData() }
                }
            };
            target.RouteData.Values["category"] = categoryToSelect;

            //Act
            string result = (string)(target.Invoke() as ViewViewComponentResult).ViewData["SelectedCategory"];

            //Assert
            Assert.Equal(categoryToSelect, result);
        }
    }
}
