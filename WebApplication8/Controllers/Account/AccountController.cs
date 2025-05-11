using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication8.Models;
using WebApplication8.ViewModels;
using WebApplication8.ViewModels.Account;

namespace WebApplication8.Controllers.Account
{
    public class AccountController : Controller
    {
        UserManager<AppUser> _userManager;
        SignInManager<AppUser> _signManager;
        RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signManager = signManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (registerVM == null)
            {
                ModelState.AddModelError("", "Bos ola bilmez");
            }
            AppUser appUser = new()
            {
                Name = registerVM.Name,
                UserName = registerVM.UserName,
                Email = registerVM.Email
            };

            var result = await _userManager.CreateAsync(appUser, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }

            return RedirectToAction("Login", "Account");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVm loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = await _userManager.FindByEmailAsync(loginVM.UserNameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(loginVM.UserNameOrEmail);
                if (user == null)
                {
                    ModelState.AddModelError("", "Username ve ya password sevhdir");
                    return View(loginVM);
                }
                var result = await _signManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, false);
            }
            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> LogOut()
        {
            await _signManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
