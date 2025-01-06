using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebEcommerce.Models;

namespace WebEcommerce.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public IActionResult Register()
        {
            if (signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(registerDTO);
            }

            var user = new ApplicationUser
            {
                UserName = registerDTO.email,
                Email = registerDTO.email,
                fisrtName = registerDTO.firstName,
                lastName = registerDTO.lastName,
                address = registerDTO.address,
                createDate = DateTime.Now.ToString()
            };

            var rs = await userManager.CreateAsync(user, registerDTO.password);

            if (rs.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "client");

                await signInManager.SignInAsync(user, false);

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in rs.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(registerDTO);
        }

        public async Task<IActionResult> Logout()
        {
            if (signInManager.IsSignedIn(User))
            {
                await signInManager.SignOutAsync();
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            if (signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(loginDTO);
            }

            var rs = await signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, 
                loginDTO.RememberMe, false);

            if (rs.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid login attempt";
            }

            return View(loginDTO);
        }
    }
}