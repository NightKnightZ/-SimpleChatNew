using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleChat.Messaging.Database.Sqlite;
using SimpleChat.Messaging.Database.Sqlite.Interfaces;
using SimpleChat.Messaging.Entities;
using SimpleChat.ViewModels.Auth;

namespace SimpleChat.Pages
{
    public class IndexModel : PageModel
    {
        IDatabaseSettings dbSettings;
        MessagingRepository<User> repos;

        [BindProperty]
        [Required]
        public LoginModel Model { get; set; }

        public IndexModel()
        {
            dbSettings = Moq.Mock.Of<IDatabaseSettings>();
            dbSettings.ConnectionString = "A:\\db.db3";
            repos = new MessagingRepository<User>(dbSettings);
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
       
        public async Task<IActionResult> OnGetAsync()
        {
            if (ModelState.IsValid)
            {
                for (int i = 0; i < repos.Count(); i++)
                {
                    User user = repos.GetItem(i);
                    if (Model.Login == user.Login)
                    {
                        if (Model.Password == user.Pass)
                        {
                            //login
                            await Authenticate(Model.Login); // аутентификация
                            return RedirectToPage("/chat");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Wrong Login or Password");
                            return Page();
                        }
                    }
                }
                ModelState.AddModelError("", "Wrong Login or Password");
                return Page();
            }
            else
                return Page();
        }
    }
}