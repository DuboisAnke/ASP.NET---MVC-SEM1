using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectCWeb_DuboisAnke_2ProA.Data;
using ProjectCWeb_DuboisAnke_2ProA.Helpers;
using ProjectCWeb_DuboisAnke_2ProA.Models;
using ProjectCWeb_DuboisAnke_2ProA.ViewModels;

namespace ProjectCWeb_DuboisAnke_2ProA.Controllers
{
    
    public class AccountController : Controller
    {
        UserManager<CustomIdentityUser> _userManager;
        SignInManager<CustomIdentityUser> _signinManager;
        private readonly PXLAppDbContext _context;
        public AccountController(UserManager<CustomIdentityUser> userManager,
            SignInManager<CustomIdentityUser> signinManager, PXLAppDbContext context)
        {
            _userManager = userManager;
            _signinManager = signinManager;
            _context = context;
        }
        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> LogoutAsync()
        {
            await _signinManager.SignOutAsync();
            return View("Login");
        }

        public IActionResult ErrorPage(string errorMessage)
        {
            ViewData["Error"] = errorMessage;
            return View();
        }
        public IActionResult Register()
        {
            ViewBag.Roles = new SelectList(RoleHelper.Roles);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel user)
        {
            ViewBag.Roles = new SelectList(RoleHelper.Roles);
            if (ModelState.IsValid)
            {
                
                var customIdentityUser = new CustomIdentityUser
                    { UserName = user.Email, Email = user.Email };
                customIdentityUser.tempRoleName = user.RoleName;
                var result = await _userManager.CreateAsync(customIdentityUser, user.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                    return View(user);
                }
                return View("Login");
            }
            return View(user);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                var identityUser = await _userManager.FindByEmailAsync(user.Email);
                if (identityUser != null)
                {
                    var userName = identityUser.UserName;
                    var result = await _signinManager.PasswordSignInAsync(userName, user.Password,
                        false, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid login attempt.");
                    }
                }
            }
            return View(user);
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        public async Task<IActionResult> Roles()
        {
            
            return View(_userManager.Users);
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        [HttpGet]
        public async Task<IActionResult> Edit(string email)
        {
            ViewBag.Rollen = new SelectList(RoleHelper.Roles);
            var user = await _userManager.FindByEmailAsync(email);
            return View(user);
        }

        [Authorize(Roles = RoleHelper.AdminRole)]
        [HttpPost]
        public async Task<IActionResult> EditAsync(CustomIdentityUser identityUser)
        {

            ViewBag.Rollen = new SelectList(RoleHelper.Roles);
            var user = await _userManager.FindByEmailAsync(identityUser.Email);
            if (user != null)
            {
                user.RoleName = identityUser.RoleName;
                user.tempRoleName = "";
                await _userManager.AddToRoleAsync(user, identityUser.RoleName);
               await _userManager.UpdateAsync(user);
            }

            return View("Roles", _userManager.Users);
        }

    }
}
