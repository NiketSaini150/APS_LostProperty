using APS_LostProperty.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using APS_LostProperty.Models;

namespace APS_LostProperty.Areas.Identity.Data;

public class DBContext : IdentityDbContext<User>
{
    public DBContext(DbContextOptions<DBContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

public DbSet<APS_LostProperty.Models.LostItem> LostItem { get; set; } = default!;

public DbSet<APS_LostProperty.Models.Location> Location { get; set; } = default!;

public DbSet<APS_LostProperty.Models.Claim> Claim { get; set; } = default!;

public DbSet<APS_LostProperty.Models.Category> Category { get; set; } = default!;
}
