using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleChat.Messaging.Database.Sqlite;
using SimpleChat.Messaging.Database.Sqlite.Interfaces;
using SimpleChat.Messaging.Entities;
using SimpleChat.ViewModels.Auth;

namespace SimpleChat.Controllers
{
    public class AuthController : Controller
    {
        IDatabaseSettings dbSettings;
        MessagingRepository<User> repos;

        public AuthController()
        {
            dbSettings = Moq.Mock.Of<IDatabaseSettings>();
            dbSettings.ConnectionString = "A:\\db.db3";
            repos = new MessagingRepository<User>(dbSettings);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        private async Task Authenticate(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                for (int i = 0; i <= repos.Count(); i++)
                {
                    User user = repos.GetItem(i);
                    if (model.Login == user.Login)
                    {
                        if (model.Password == user.Pass)
                        {
                            //login
                            await Authenticate(model.Login); // аутентификация

                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Wrong Login or Password");
                            break;
                        }
                    }
                }
                ModelState.AddModelError("", "Wrong Login or Password");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                int c = repos.Count();
                for (int i = 0; i <= c; i++)
                {
                    User user = repos.GetItem(i);
                    if (model.Login == user.Login)
                    {
                        //await Clients.Caller.SendAsync("ReceiveMessage", "This Login already exists");
                        ModelState.AddModelError("", "This Logis already exists");
                        break;
                    }
                    if (i == c)
                    {
                        if (model.Password.Length >= 6)
                        {
                            User nuser = new User();
                            nuser.Login = model.Login;
                            nuser.Pass = model.Password;
                            nuser.Id = c++;

                            await Authenticate(model.Login); // аутентификация
                            return RedirectToAction("Index", "Home");
                        }
                        else
                            ModelState.AddModelError("", "Password must have 6 or more symbols");//await Clients.Caller.SendAsync("ReceiveMessage", "Password length must be 6 or longer");
                    }
                }
                ModelState.AddModelError("", "error");
            }
            return View(model);
        }
    }
}
