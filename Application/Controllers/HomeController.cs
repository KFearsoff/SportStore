using Application.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Controllers
{
    public class HomeController : Controller
    {
        private IStoreRepository _repository;
        public HomeController(IStoreRepository repository) => _repository = repository;
        public IActionResult Index() => View(_repository.Products);
    }
}
