using LMS_Assig_.Models;
using Microsoft.AspNetCore.Identity;

namespace LMS_Assig_.ViewModel
{
    public class ViewModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public ViewModel(UserManager<ApplicationUser> userManager)
        {

            _userManager = userManager;

        }
        public RoleManager<ApplicationUser> UserManager { get; set; }
    }
}
