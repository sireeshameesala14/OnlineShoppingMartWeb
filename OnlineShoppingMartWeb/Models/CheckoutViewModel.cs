using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingMartWeb.Models
{
    public class CheckoutViewModel
    {
        public AddressDetail BillingAddress { get; set; }

        public AddressDetail ShippingAddress { get; set; }

        public bool IsShippingAnotherAddress { get; set; }

        public string OrderNotes { get; set; }

        [Required(ErrorMessage = "Please accept the terms and conditions.")]
        public bool IsTermsAccepted { get; set; }

        public decimal TotalOrderPrice { get; set; }
        public List<ProductDetailViewModel> Products { get; set; }

        public string PaymentMode { get; set; }

        public LoginViewModel Login { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

    }
}