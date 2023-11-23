using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingMartWeb.Models
{
    public class AddressDetail
    {
        [Required(ErrorMessage = "Please Enter First Name.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Last Name.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Enter Address.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please Enter City.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Please Enter State.")]
        public string State { get; set; }

        [Required(ErrorMessage = "Please Enter Pin.")]
        public string Pin { get; set; }

        [Required(ErrorMessage = "Please Enter Email.")]
        [RegularExpression(@"^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,3})$", ErrorMessage = "Your email address is not in a valid format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Enter Country.")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Please Enter Mobile.")]
        public string Mobile { get; set; }

    }
}