using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingMartWeb.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please Enter User Name.")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Please Enter Password.")]
        public string Password { get; set; }
    }
}