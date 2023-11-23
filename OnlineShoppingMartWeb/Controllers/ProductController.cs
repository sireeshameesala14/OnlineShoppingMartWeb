using Microsoft.AspNetCore.Mvc;
using OnlineShoppingMartWeb.Models;
using OnlineShoppingMartWeb.ResponseModels;
using OnlineShoppingMartWeb.Utility;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace OnlineShoppingMartWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        public IActionResult SearchResult(string searchText, string productCategeory)
        {
            SearchResultViewModel searchResultViewModel = new SearchResultViewModel();
            try
            {
                
                    if (!string.IsNullOrEmpty(searchText) && !string.IsNullOrEmpty(productCategeory))
                    {
                        string apiResponse = ApiClient.GetAjaxResponse("Product/GetProductSearchResult?searchText=" + searchText + "&productCategoryId=" + productCategeory, "Get", string.Empty);
                        //"/Product/GetProductSearchResult?searchText=wireless&productCategoryId=0"
                        if (!string.IsNullOrEmpty(apiResponse))
                        {
                            List<ProductSearchResult> searchResult = JsonSerializer.Deserialize<List<ProductSearchResult>>(apiResponse);
                            searchResultViewModel.ProductSearchResult = new List<SearchResult>();
                            foreach (var result in searchResult)
                            {
                                SearchResult resultObj = new SearchResult();
                                resultObj.ProductId = result.ProductId;
                                resultObj.ProductName = result.ProductName;
                                resultObj.ProductPrice = result.ProductPrice;
                                resultObj.ProductType = result.ProductType;
                            resultObj.ProductColor = result.ProductColor.Split(",")[0];
                            resultObj.ProductImages = new Dictionary<string, List<string>>();
                                foreach (var img in result.Images)
                                {
                                    if (resultObj.ProductImages.ContainsKey(img.ImageSize))
                                    {
                                        var newImages = resultObj.ProductImages[img.ImageSize];
                                        newImages.Add(img.ImageName);
                                        resultObj.ProductImages[img.ImageSize] = newImages;
                                    }
                                    else
                                    {
                                        List<string> imgs = new List<string>();
                                        imgs.Add(img.ImageName);
                                        resultObj.ProductImages.Add(img.ImageSize, imgs);
                                    }

                                }
                                searchResultViewModel.ProductSearchResult.Add(resultObj);
                            }


                        }
                    }
                
            }
            catch (Exception ex)
            {

            }
            return View(searchResultViewModel);
        }
        public IActionResult ProductDetail(string productId)
        {
            ProductDetailViewModel productDetailVM = new ProductDetailViewModel();
            try
            {
                if (!string.IsNullOrEmpty(productId))
                {
                    string apiResponse = ApiClient.GetAjaxResponse("Product/GetProductDetail?productId=" + Convert.ToInt64(productId), "Get", string.Empty);

                    if (!string.IsNullOrEmpty(apiResponse))
                    {
                        ProductDetail pDetail = JsonSerializer.Deserialize<ProductDetail>(apiResponse);
                        if (pDetail != null)
                        {
                            productDetailVM.ProductPrice = pDetail.ProductPrice;
                            productDetailVM.ProductName = pDetail.ProductName;
                            productDetailVM.ProductId = pDetail.ProductId;
                            productDetailVM.Brand = pDetail.Brand;
                            productDetailVM.ProductCount = pDetail.ProductCount;
                            productDetailVM.ProductCategory = pDetail.ProductCategory;
                            productDetailVM.ProductType = pDetail.ProductType;
                            productDetailVM.ProductSpecification = pDetail.ProductSpecification;
                            productDetailVM.Tax = pDetail.Tax;
                            productDetailVM.ShippingCharge = pDetail.ShippingCharge;
                            productDetailVM.ProductImages = new Dictionary<string, List<string>>();
                            foreach (var img in pDetail.Images)
                            {
                                if (productDetailVM.ProductImages.ContainsKey(img.ImageSize))
                                {
                                    var newImages = productDetailVM.ProductImages[img.ImageSize];
                                    newImages.Add(img.ImageName);
                                    productDetailVM.ProductImages[img.ImageSize] = newImages;
                                }
                                else
                                {
                                    List<string> imgs = new List<string>();
                                    imgs.Add(img.ImageName);
                                    productDetailVM.ProductImages.Add(img.ImageSize, imgs);
                                }
                            }
                            productDetailVM.ProductReviews = new List<ProductReview>();
                            foreach (var review in pDetail.ProductReviews)
                            {
                                ProductReview pReview = new ProductReview();
                                pReview.Rating = review.Rating;
                                pReview.ReviewComments = review.ReviewComments;
                                pReview.UserId = review.UserId;
                                productDetailVM.ProductReviews.Add(pReview);
                            }
                            productDetailVM.ProductColor = new List<string>();
                            productDetailVM.ProductColor = pDetail.ProductColor;
                        }

                    }
                }
            }
            catch (Exception ex)
            {

            }


            return View(productDetailVM);
        }
        public IActionResult Cart(string productId, string productColor, string productQuantity)
        {
            List<ProductDetailViewModel> productDetails = new List<ProductDetailViewModel>();
            ProductDetailViewModel productDetailVM = new ProductDetailViewModel();
            try
            {
                if (!string.IsNullOrEmpty(productColor))
                {
                    if (!string.IsNullOrEmpty(productId))
                    {
                        string apiResponse = ApiClient.GetAjaxResponse("Product/GetProductDetail?productId=" + Convert.ToInt64(productId), "Get", string.Empty);

                        if (!string.IsNullOrEmpty(apiResponse))
                        {
                            ProductDetail pDetail = JsonSerializer.Deserialize<ProductDetail>(apiResponse);
                            if (pDetail != null)
                            {
                                productDetailVM.ProductPrice = pDetail.ProductPrice;
                                productDetailVM.ProductName = pDetail.ProductName;
                                productDetailVM.ProductId = pDetail.ProductId;
                                productDetailVM.Brand = pDetail.Brand;
                                productDetailVM.ProductCount = pDetail.ProductCount;
                                productDetailVM.ProductCategory = pDetail.ProductCategory;
                                productDetailVM.ProductType = pDetail.ProductType;
                                productDetailVM.ProductSpecification = pDetail.ProductSpecification;
                                productDetailVM.Tax = pDetail.Tax;
                                productDetailVM.ShippingCharge = pDetail.ShippingCharge;
                                productDetailVM.SelectedProductColor = productColor;
                                productDetailVM.SelectedProductQuantity = productQuantity;
                                productDetailVM.ProductImages = new Dictionary<string, List<string>>();
                                foreach (var img in pDetail.Images)
                                {
                                    if (productDetailVM.ProductImages.ContainsKey(img.ImageSize))
                                    {
                                        var newImages = productDetailVM.ProductImages[img.ImageSize];
                                        newImages.Add(img.ImageName);
                                        productDetailVM.ProductImages[img.ImageSize] = newImages;
                                    }
                                    else
                                    {
                                        List<string> imgs = new List<string>();
                                        imgs.Add(img.ImageName);
                                        productDetailVM.ProductImages.Add(img.ImageSize, imgs);
                                    }
                                }
                                productDetailVM.ProductReviews = new List<ProductReview>();
                                foreach (var review in pDetail.ProductReviews)
                                {
                                    ProductReview pReview = new ProductReview();
                                    pReview.Rating = review.Rating;
                                    pReview.ReviewComments = review.ReviewComments;
                                    pReview.UserId = review.UserId;
                                    productDetailVM.ProductReviews.Add(pReview);
                                }
                                productDetailVM.ProductColor = new List<string>();
                                productDetailVM.ProductColor = pDetail.ProductColor;
                            }

                            if (HttpContext.Session.Keys.Contains("ProductDetail"))
                            {
                                var products = HttpContext.Session.GetObjectFromJson<List<ProductDetailViewModel>>("ProductDetail");
                                products.Add(productDetailVM);
                                HttpContext.Session.SetObjectAsJson("ProductDetail", JsonSerializer.Serialize(products));
                            }
                            else
                            {
                                productDetails.Add(productDetailVM);
                                HttpContext.Session.SetObjectAsJson("ProductDetail", JsonSerializer.Serialize(productDetails));
                            }

                        }
                    }
                }
                else if (HttpContext.Session.Keys.Contains("ProductDetail"))
                {
                    var products = HttpContext.Session.GetObjectFromJson<List<ProductDetailViewModel>>("ProductDetail");
                    var product = products.Where(m => m.ProductId == Convert.ToInt64(productId)).FirstOrDefault();
                    if (products.Remove(product))
                    {
                        HttpContext.Session.SetObjectAsJson("ProductDetail", JsonSerializer.Serialize(products));
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}