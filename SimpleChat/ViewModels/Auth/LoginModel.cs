using System.ComponentModel.DataAnnotations;

namespace SimpleChat.ViewModels.Auth
{
    public class LoginModel
    {
        [Required]
        public string Login { get; set; }

        
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
    }
}
