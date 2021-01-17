using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Application.Pages.Admin
{
    [Authorize]
    public class IdentityUsersModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        public IdentityUsersModel(UserManager<IdentityUser> userManager) => _userManager = userManager;

        public IdentityUser AdminUser { get; set; }

        public async Task OnGetAsync() => AdminUser = await _userManager.FindByNameAsync("Admin");
    }
}
