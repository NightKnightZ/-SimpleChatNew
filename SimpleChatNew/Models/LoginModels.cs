using System.ComponentModel.DataAnnotations;

namespace SimpleChatNew.Models
{
    public class LoginModels
    {
        [Required(ErrorMessage ="Login is required")]
        public string Login { get; set; }
        
        [Required(ErrorMessage ="Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
