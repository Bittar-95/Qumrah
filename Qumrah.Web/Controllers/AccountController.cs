using aspnetcore.ntier.DAL.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Qumrah.Web.Models.Account;
using System.Text.Encodings.Web;
using System.Text;
using aspnetcore.ntier.DAL.Entities;
using aspnetcore.ntier.BLL.Services.IServices;
using aspnetcore.ntier.DTO.DTOs;

namespace Qumrah.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {

            if (ModelState.IsValid)
            {
                var loginResult = await _accountService.LoginUser(new LoginUserDto
                {
                    Email = model.Email,
                    isPersistent = model.isPersistent,
                    Password = model.Password,
                });
                if (loginResult)
                {
                    var redirectUrl = model.ReturnUrl ?? "/";
                    return Redirect(redirectUrl);
                }
            }

            return View(model);


        }

        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignupVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.RegisterNewUser(new RegisterUserDto
                {

                    Email = model.Email,
                    Password = model.Password,
                });
                if (result.Succeeded)
                {
                    return RedirectToAction("index", "Home");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutUser();
            return RedirectToAction("Index", "Home");

        }

    }
}
