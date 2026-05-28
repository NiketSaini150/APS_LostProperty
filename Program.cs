using APS_LostProperty.Areas.Identity.Data;
using APS_LostProperty.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Get connection string from appsettings.json
var connectionString =
    builder.Configuration.GetConnectionString("LostPropertyContextConnection")
    ?? throw new InvalidOperationException("Connection string 'LostPropertyContextConnection' not found.");

// Register database context
builder.Services.AddDbContext<DBContext>(options =>
    options.UseSqlServer(connectionString));

// Identity setup with roles enabled
// IMPORTANT: Uses custom User class (not IdentityUser)
builder.Services.AddDefaultIdentity<User>(options =>
{
    // Basic password rules (can be changed)
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<DBContext>();

// Add MVC and Razor Pages services
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddScoped<IEmailSender, EmailSender>();


var app = builder.Build();

// Seed roles and default users on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<DBContext>();
        var userManager = services.GetRequiredService<UserManager<User>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Initialize database (create roles + users)
        DBInilitizer.Initialize(context, userManager, roleManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error seeding database.");
    }
}

// Configure HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// Authentication must come before Authorization
app.UseAuthentication();
app.UseAuthorization();

// Map Razor Pages
app.MapRazorPages();

// Static assets support
app.MapStaticAssets();

// Default MVC route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Run application
app.Run();
// to do list 
// make it so user who is logged in is only able to see their cliam and they can edit and delete it. but the admin can see all cliams.
// add auto email so if the enum is changed to found auto emial is sent to the email that made tahta claim
// add phone number to regist. 