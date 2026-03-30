using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using APS_LostProperty.Areas.Identity.Data;
using APS_LostProperty.Migrations;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("LostPropertyContextConnection") ?? throw new InvalidOperationException("Connection string 'LostPropertyContextConnection' not found.");;

builder.Services.AddDbContext<DBContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<DBContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();



//// ===== Seed Database =====
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var context = services.GetRequiredService<DBContext>();
//    var userManager = services.GetRequiredService<UserManager<User>>();
 


//}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<DBContext>();
        var userManager = services.GetRequiredService<UserManager<User>>();

        DBInilitizer.Initialize(context, userManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating or seeding the DB.");
    }
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapRazorPages();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
