using Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Application.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly IStoreRepository _repository;
        public NavigationMenuViewComponent(IStoreRepository repository) => _repository = repository;

        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["category"];
            return View(_repository.Products
            .Select(x => x.Category)
            .Distinct()
            .OrderBy(x => x));
        }
    }
}
