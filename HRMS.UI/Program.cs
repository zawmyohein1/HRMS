var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<HRMS.UI.Helper.ApiService>();

var app = builder.Build();

// Use static files for public assets like CSS, JS, images, etc.
app.UseStaticFiles();

// Use routing and authentication/authorization if required.
app.UseRouting();
app.UseAuthentication(); // Add this line if you have authentication
app.UseAuthorization();


//app.MapControllerRoute(
//    name: "setup",
//    pattern: "Setup/{controller=Department}/{action=Index}/{id?}",
//    defaults: new { area = "Setup" });


// Define the default route pattern.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// Run the application
app.Run();
