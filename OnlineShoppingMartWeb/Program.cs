using OnlineShoppingMartWeb.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
IConfiguration configuration = null;
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(120);

});

var app = builder.Build();
app.UseSession();
configuration = app.Configuration;
Configuration.ApiAuthToken = configuration.GetValue<string>("ApiAuthToken");
Configuration.OsmApiUrl = configuration.GetValue<string>("OsmApiUrl");
Configuration.ShippingCharge = configuration.GetValue<decimal>("ShippingCharge");
Configuration.PaypalClientId = configuration.GetValue<string>("PaypalClientId");
Configuration.PaypalClientSecret = configuration.GetValue<string>("PaypalClientSecret");
Configuration.PaypalMode = configuration.GetValue<string>("PaypalMode");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/User/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Index}/{id?}");

app.Run();
