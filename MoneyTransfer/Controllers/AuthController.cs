using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoneyTransfer.BLL.Dtos.Auth;
using MoneyTransfer.BLL.Services;
using MoneyTransfer.Data.Entites;
using System.Security.Claims;

namespace MoneyTransfer.Presentation.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly CountryService _countryService;
        private readonly IMapper _mapper;

        public AuthController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, CountryService countryService, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _countryService = countryService;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                LoginDto model = new() { ReturnUrl = returnUrl };
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email!, model.Password!, model.RememberMe, false);
                if (result.Succeeded)
                {
                    var user = await _signInManager.UserManager.FindByEmailAsync(model.Email!);
                    await Claims(user!);
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "Invalid login attempt");
                return View(model);
            }
            return View(model);
        }


        public async Task<IActionResult> Register()
        {
            var model = new RegisterDto();
            model.countryList = await _countryService.CountryList();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (ModelState.IsValid)
            {
                var userMapData = _mapper.Map<AppUser>(model);
                var result = await _userManager.CreateAsync(userMapData, model.Password!);
                if (result.Succeeded)
                {
                    var role = new IdentityRole("User");
                    await _roleManager.CreateAsync(role);
                    var user = await _userManager.FindByEmailAsync(userMapData.Email!);
                    await _userManager.AddToRoleAsync(user!, role.Name!);
                    await _signInManager.SignInAsync(user!, false);
                    await Claims(user!);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Auth");
        }

        public async Task<bool> Claims(AppUser user)
        {
            var customClaims = new[] {
                        new Claim("UserId", user?.Id!),
                        new Claim(ClaimTypes.Email, user?.Email!),
                        new Claim("FirstName", user?.FirstName!),
                        new Claim("MiddleName",!string.IsNullOrEmpty( user!.MiddleName)?user.MiddleName:""),
                        new Claim("LastName", user?.LastName!),
                        new Claim("Country", user?.Country!),
                        new Claim("Address", user?.Address!)
                    };
            await _signInManager.SignInWithClaimsAsync(user!, true, customClaims);
            return true;
        }
    }
}
