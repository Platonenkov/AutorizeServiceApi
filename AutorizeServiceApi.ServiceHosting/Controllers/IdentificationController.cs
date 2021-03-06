using System.Threading.Tasks;
using AutorizeServiceApi.Domain.Models;
using AutorizeServiceApi.ServiceHosting.ViewModels.AccountViewModels;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AutorizeServiceApi.ServiceHosting.Controllers
{
    [AllowAnonymous]
    [Route("[controller]")]
    public class IdentificationController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentificationController(
            IIdentityServerInteractionService interaction,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _interaction = interaction;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Login(string returnUrl)
        {
            return View();
        }

        [HttpPost("[action]")]

        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please. Validate your credentials and try again.");
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                ModelState.AddModelError("UserName", "User not found");
                return View(model);
            }

            var signResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (!signResult.Succeeded)
            {
                ModelState.AddModelError("", "Something went wrong");
                return View(model);
            }

            return Redirect(model.ReturnUrl);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var logout_context = await _interaction.CreateLogoutContextAsync();

            var logout = await _interaction.GetLogoutContextAsync(logout_context);
            await _signInManager.SignOutAsync();
            return Redirect(logout.PostLogoutRedirectUri);
        }
    }
}
