using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using APS_LostProperty.Areas.Identity.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("LostPropertyContextConnection") ?? throw new InvalidOperationException("Connection string 'LostPropertyContextConnection' not found.");;

builder.Services.AddDbContext<DBContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<DBContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();



// ===== Seed Database =====
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DBContext>();
    var userManager = services.GetRequiredService<UserManager<User>>();

    try
    {
        // Apply migrations
        context.Database.Migrate();

        // Call your DBInitializer
        DBInilitizer.Initialize(context, userManager);

        Console.WriteLine("Database seeded successfully!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Seeding failed: {ex.Message}");
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
