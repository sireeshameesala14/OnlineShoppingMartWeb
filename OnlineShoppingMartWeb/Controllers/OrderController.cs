using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using OnlineShoppingMartWeb.Models;
using OnlineShoppingMartWeb.ResponseModels;
using OnlineShoppingMartWeb.Utility;
using PayPal.Api;
using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text.Json;

namespace OnlineShoppingMartWeb.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;
        const string UserSession = "UserSession";
        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }

        public IActionResult OrderSuccess()
        {
            var orderResponse = HttpContext.Session.GetObjectFromJson<CheckoutSaveResponse>("OrderResponse");
            if (orderResponse != null && orderResponse.PaymentMode.ToUpper() != "COD")
            {
                var transectionUpdate = HttpContext.Session.GetObjectFromJson<UpdateTransectionDetail>("TransetionUpdate");

                if (transectionUpdate != null)
                {
                    var payload = JsonSerializer.Serialize(transectionUpdate);
                    string apiResponse = ApiClient.GetAjaxResponse("Order/UpdateTransectionDetail", "Post", payload);
                    var response = JsonSerializer.Deserialize<bool>(apiResponse);

                    if (response)
                    {
                        return View(orderResponse);
                    }
                    else
                    {
                        RedirectToAction("OrderFailure", "Order", new { area = "" });
                    }
                }
                else
                {
                    RedirectToAction("OrderFailure", "Order", new { area = "" });
                }
            }
            return View(orderResponse);

        }

        public IActionResult OrderFailure()
        {
            var orderResponse = HttpContext.Session.GetObjectFromJson<CheckoutSaveResponse>("OrderResponse");
            string apiResponse = ApiClient.GetAjaxResponse("Order/UpdateOrderStatus?orderId=" + orderResponse.OrderId + "&orderStatus=Failed", "Get", null);
            var response = JsonSerializer.Deserialize<bool>(apiResponse);
            if (response)
            {
                orderResponse.OrderStatus = "Failed";
            }
            return View(orderResponse);
        }

        public IActionResult ProcessPayment(CheckoutViewModel model)
        {
            var prodDetail = HttpContext.Session.GetObjectFromJson<List<ProductDetailViewModel>>("OrderDetail");
            model.Products = prodDetail;
            bool flag = true;
            foreach (var prod in prodDetail)
            {
                string apiResponse = ApiClient.GetAjaxResponse("Product/UpdateProductQuantity?productId=" + prod.ProductId + "&quantity=" + (prod.ProductCount - Convert.ToInt32(prod.SelectedProductQuantity)), "Get", string.Empty);

                if (!Convert.ToBoolean(apiResponse))
                {
                    flag = false;
                    break;
                }

            }
            if (flag)
            {
                var payload = JsonSerializer.Serialize(model);
                string apiResponse = ApiClient.GetAjaxResponse("Order/SaveCheckoutDetail", "Post", payload);
                HttpContext.Session.SetObjectAsJson("OrderResponse", apiResponse);

                CheckoutSaveResponse response = JsonSerializer.Deserialize<CheckoutSaveResponse>(apiResponse);

                if (response != null)
                {
                    if (!response.IsSuccess)
                    {
                        return RedirectToAction("OrderFailure", "Order", new { area = "" });
                    }
                }
            }
            else
            {
                return RedirectToAction("OrderFailure", "Order", new { area = "" });
            }

            if (model.PaymentMode.ToUpper() == "PAYPAL")
            {
                return RedirectToAction("PaymentWithPaypal", "Order", new { area = "" });
            }
            return RedirectToAction("OrderSuccess", "Order", new { area = "" });
        }

        public IActionResult PaymentWithPaypal(string Cancel = null)
        {
            //getting the apiContext
            APIContext apiContext = PaypalConfiguration.GetAPIContext(Configuration.PaypalClientId, Configuration.PaypalClientSecret, Configuration.PaypalMode);

            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal
                //Payer Id will be returned when payment proceeds or click to pay
                string payerId = Request.Query["PayerID"];

                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist
                    //it is returned by the create function call of the payment class

                    // Creating a payment
                    // baseURL is the url on which paypal sendsback the data.
                    string baseURI = Request.Scheme + "://" + Request.Host +
                                "/Order/PaymentWithPayPal?";

                    //here we are generating guid for storing the paymentID received in session
                    //which will be used in the payment execution

                    var guid = Convert.ToString((new Random()).Next(100000));

                    //CreatePayment function gives us the payment approval url
                    //on which payer is redirected for paypal account payment

                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);

                    //get links returned from paypal in response to Create function call

                    var links = createdPayment.links.GetEnumerator();

                    string paypalRedirectUrl = null;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    // saving the paymentID in the key guid
                    HttpContext.Session.SetString(guid, createdPayment.id);

                    return Redirect(paypalRedirectUrl);
                }
                else
                {

                    // This function exectues after receving all parameters for the payment

                    var guid = Request.Query["guid"];

                    var executedPayment = ExecutePayment(apiContext, payerId, HttpContext.Session.GetString(guid));

                    //If executed payment failed then we will show payment failure message to user
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return RedirectToAction("OrderFailure", "Order", new { area = "" });
                    }
                    UpdateTransectionDetail transectionDetail = new UpdateTransectionDetail();
                    transectionDetail.TransectionId = Convert.ToInt64(executedPayment.transactions[0].invoice_number);
                    transectionDetail.PayerId = payerId;
                    transectionDetail.PaymentId = HttpContext.Session.GetString(guid);
                    transectionDetail.RefrenceId = guid;
                    transectionDetail.GatewayName = "paypal";
                    transectionDetail.IsPaymentSuccessful = true;
                    HttpContext.Session.SetObjectAsJson("TransetionUpdate", JsonSerializer.Serialize(transectionDetail));

                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("OrderFailure", "Order", new { area = "" });
            }



            //on successful payment, show success page to user.
            return RedirectToAction("OrderSuccess", "Order", new { area = "" });
        }

        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }


        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            //create itemlist and add item objects to it
            var itemList = new ItemList() { items = new List<Item>() };
            decimal total = 0;
            decimal totalTax = 0;
            if (HttpContext.Session.Keys.Contains("OrderDetail"))
            {
                var productInfo = HttpContext.Session.GetObjectFromJson<List<ProductDetailViewModel>>("OrderDetail");
                if (productInfo != null && productInfo.Count > 0)
                {
                    foreach (var prod in productInfo)
                    {
                        total = total + prod.ProductPrice;
                        totalTax = totalTax + prod.Tax;
                        //Adding Item Details like name, currency, price etc
                        itemList.items.Add(new Item()
                        {
                            name = prod.ProductName,
                            currency = "USD",
                            price = prod.ProductPrice.ToString(),
                            quantity = prod.SelectedProductQuantity,
                            sku = prod.ProductId.ToString()
                        });
                    }
                }
            }

            var payer = new Payer() { payment_method = "paypal" };

            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };

            // Adding Tax, shipping and Subtotal details
            var details = new Details()
            {
                tax = totalTax.ToString(),
                shipping = Configuration.ShippingCharge.ToString(),
                subtotal = total.ToString()
            };

            //Final amount with details
            var amount = new Amount()
            {
                currency = "USD",
                total = (total + totalTax + Configuration.ShippingCharge).ToString(), // Total must be equal to sum of tax, shipping and subtotal.
                details = details
            };

            var transactionList = new List<Transaction>();

            if (HttpContext.Session.Keys.Contains("OrderResponse"))
            {
                var orderResponse = HttpContext.Session.GetObjectFromJson<CheckoutSaveResponse>("OrderResponse");

                // Adding description about the transaction
                transactionList.Add(new Transaction()
                {
                    description = "Transaction description",
                    invoice_number = orderResponse.TransectionId.ToString(), //Generate an Invoice No
                    amount = amount,
                    item_list = itemList
                });

            }
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            // Create a payment using a APIContext
            return this.payment.Create(apiContext);
        }

        public IActionResult Checkout(CheckoutViewModel model)
        {
            model.BillingAddress = new AddressDetail();

            if (!string.IsNullOrEmpty(model.UserName) && !string.IsNullOrEmpty(model.Password))
            {
                try
                {
                    string apiResponse = ApiClient.GetAjaxResponse("User/GetUserAuthStatus?userName=" + model.UserName + "&password=" + model.Password + "&userType=user", "Get", string.Empty);
                    // string apiResponse = ApiClient.GetAjaxResponse("User/GetUserAuthStatus", "Get", JsonSerializer.Serialize(login));

                    if (!string.IsNullOrEmpty(apiResponse))
                    {
                        UserAuhDetail userAuthDetail = JsonSerializer.Deserialize<UserAuhDetail>(apiResponse);
                        userAuthDetail.UserName = model.UserName;
                        if (userAuthDetail.IsAuthenticated)
                        {
                            HttpContext.Session.SetObjectAsJson(UserSession, JsonSerializer.Serialize(userAuthDetail));
                        }

                    }
                }
                catch (Exception ex)
                {

                }

            }

            if (HttpContext.Session.Keys.Contains("UserSession"))
            {
                UserAuhDetail userAuthDetail = HttpContext.Session.GetObjectFromJson<UserAuhDetail>("UserSession");
                string userDetailResponse = ApiClient.GetAjaxResponse("User/GetUserDetail?userId=" + userAuthDetail.UserId, "Get", string.Empty);
                UserDetail userDetail = JsonSerializer.Deserialize<UserDetail>(userDetailResponse);
                model.BillingAddress.FirstName = userDetail.FirstName;
                model.BillingAddress.LastName = userDetail.LastName;
                model.BillingAddress.Address = userDetail.Address;
                model.BillingAddress.State = userDetail.State;
                model.BillingAddress.City = userDetail.City;
                model.BillingAddress.Country = userDetail.Country;
                model.BillingAddress.Email = userDetail.Email;
                model.BillingAddress.Mobile = userDetail.Mobile;
            }
            if (HttpContext.Session.Keys.Contains("ProductDetail"))
            {
                model.Products = HttpContext.Session.GetObjectFromJson<List<ProductDetailViewModel>>("ProductDetail");

                HttpContext.Session.SetObjectAsJson("OrderDetail", JsonSerializer.Serialize(model.Products));

                HttpContext.Session.Remove("ProductDetail");
            }
            if (HttpContext.Session.Keys.Contains("OrderDetail"))
            {
                model.Products = HttpContext.Session.GetObjectFromJson<List<ProductDetailViewModel>>("OrderDetail");
            }

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}