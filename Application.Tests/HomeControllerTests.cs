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
using Application.Models.ViewModels;

namespace Application.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void CanUseRepository()
        {
            //Arrange
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(r => r.Products).Returns((new Product[]
            {
                new Product { ProductID = 1, Name = "P1" },
                new Product { ProductID = 2, Name = "P2" }
            }).AsQueryable());
            HomeController controller = new HomeController(mock.Object);

            //Act
            ProductsListViewModel result = controller.Index(null).ViewData.Model as ProductsListViewModel;

            //Assert
            Product[] prodArray = result.Products.ToArray();
            Assert.True(prodArray.Length == 2);
            Assert.Equal("P1", prodArray[0].Name);
            Assert.Equal("P2", prodArray[1].Name);
        }

        [Fact]
        public void CanPaginate()
        {
            //Arrange
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(r => r.Products).Returns((new Product[]
            {
                new Product { ProductID = 1, Name = "P1" },
                new Product { ProductID = 2, Name = "P2" },
                new Product { ProductID = 3, Name = "P3" },
                new Product { ProductID = 4, Name = "P4" },
                new Product { ProductID = 5, Name = "P5" }
            }).AsQueryable());
            HomeController controller = new HomeController(mock.Object) { PageSize = 3 };

            //Act
            ProductsListViewModel result = controller.Index(null, 2).ViewData.Model as ProductsListViewModel;

            //Assert
            Product[] prodArray = result.Products.ToArray();
            Assert.True(prodArray.Length == 2);
            Assert.Equal("P4", prodArray[0].Name);
            Assert.Equal("P5", prodArray[1].Name);
        }

        [Fact]
        public void CanSendPaginationViewModel()
        {
            //Arrange
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(r => r.Products).Returns((new Product[]
            {
                new Product { ProductID = 1, Name = "P1" },
                new Product { ProductID = 2, Name = "P2" },
                new Product { ProductID = 3, Name = "P3" },
                new Product { ProductID = 4, Name = "P4" },
                new Product { ProductID = 5, Name = "P5" }
            }).AsQueryable());
            HomeController controller = new HomeController(mock.Object) { PageSize = 3 };

            //Act
            ProductsListViewModel result = controller.Index(null, 2).ViewData.Model as ProductsListViewModel;

            //Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.Equal(2, pageInfo.CurrentPage);
            Assert.Equal(3, pageInfo.ItemsPerPage);
            Assert.Equal(5, pageInfo.TotalItems);
            Assert.Equal(2, pageInfo.TotalPages);
        }

        [Fact]
        public void CanFilterProducts()
        {
            //Arrange
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(r => r.Products).Returns((new Product[]
            {
                new Product { ProductID = 1, Name = "P1", Category = "Cat1" },
                new Product { ProductID = 2, Name = "P2", Category = "Cat2" },
                new Product { ProductID = 3, Name = "P3", Category = "Cat1" },
                new Product { ProductID = 4, Name = "P4", Category = "Cat2" },
                new Product { ProductID = 5, Name = "P5", Category = "Cat3" }
            }).AsQueryable());
            HomeController controller = new HomeController(mock.Object) { PageSize = 3 };

            //Act
            ProductsListViewModel result = controller.Index("Cat2", 1).ViewData.Model as ProductsListViewModel;

            //Assert
            Product[] prodArray = result.Products.ToArray();
            Assert.Equal(2, prodArray.Length);
            Assert.True(prodArray[0].Name == "P2" && prodArray[0].Category == "Cat2");
            Assert.True(prodArray[1].Name == "P4" && prodArray[1].Category == "Cat2");
        }

        [Fact]
        public void CanGenerateCategorySpecificProductCount()
        {
            //Arrange
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(r => r.Products).Returns((new Product[]
            {
                new Product { ProductID = 1, Name = "P1", Category = "Cat1" },
                new Product { ProductID = 2, Name = "P2", Category = "Cat2" },
                new Product { ProductID = 3, Name = "P3", Category = "Cat1" },
                new Product { ProductID = 4, Name = "P4", Category = "Cat2" },
                new Product { ProductID = 5, Name = "P5", Category = "Cat3" }
            }).AsQueryable());
            HomeController target = new HomeController(mock.Object) { PageSize = 3 };
            Func<ViewResult, ProductsListViewModel> GetModel = (result => result?.ViewData?.Model as ProductsListViewModel);

            //Act
            int? res1 = GetModel(target.Index("Cat1"))?.PagingInfo.TotalItems;
            int? res2 = GetModel(target.Index("Cat2"))?.PagingInfo.TotalItems;
            int? res3 = GetModel(target.Index("Cat3"))?.PagingInfo.TotalItems;
            int? resAll = GetModel(target.Index(null))?.PagingInfo.TotalItems;

            //Assert
            Assert.Equal(2, res1);
            Assert.Equal(2, res2);
            Assert.Equal(1, res3);
            Assert.Equal(5, resAll);
        }
    }
}
