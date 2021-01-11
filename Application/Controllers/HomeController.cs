using Application.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Models.ViewModels;

namespace Application.Controllers
{
    public class HomeController : Controller
    {
        private IStoreRepository _repository;
        public int PageSize { get; set; } = 4;

        public HomeController(IStoreRepository repository) => _repository = repository;

        public ViewResult Index(int productPage = 1)
            => View(new ProductsListViewModel
            {
                Products = _repository.Products
                                    .OrderBy(p => p.ProductID)
                                    .Skip((productPage - 1) * PageSize)
                                    .Take(PageSize),

                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = _repository.Products.Count()
                }
            });
    }
}
