using Application.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Components
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private Cart _cart;
        public CartSummaryViewComponent(Cart cartService) => _cart = cartService;
        public IViewComponentResult Invoke() => View(_cart);
    }
}
