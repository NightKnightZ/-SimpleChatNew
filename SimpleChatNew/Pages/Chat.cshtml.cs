using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SimpleChatNew.Pages
{
    [Authorize]
    public class ChatModel : PageModel
    {
        [BindProperty]
        public string Name { get; set; }

        public ChatModel()
        {
            //Name = User.Identity.Name;
        }

        
        public void OnGet()
        {

        }
    }
}