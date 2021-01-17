using Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Application.Components
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private readonly Cart _cart;
        public CartSummaryViewComponent(Cart cartService) => _cart = cartService;
        public IViewComponentResult Invoke() => View(_cart);
    }
}
