using APS_LostProperty.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using APS_LostProperty.Models;

namespace APS_LostProperty.Areas.Identity.Data;

// This class represents the main database context for the application
// It connects the application models to the database using Entity Framework Core
// It also inherits from IdentityDbContext to include ASP.NET Identity tables for user authentication
public class DBContext : IdentityDbContext<User>
{
    // Constructor that receives database configuration options
    // These options are usually set in Program.cs or Startup.cs
    public DBContext(DbContextOptions<DBContext> options)
        : base(options)
    {
    }

    // This method is used to configure relationships and database rules
    // It runs when the Entity Framework model is being created
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Calls the default ASP.NET Identity configuration
        base.OnModelCreating(builder);


        // Configures the relationship between Claim and LostItem
        // A Claim can be linked to one LostItem
        // A LostItem can have many Claims
        // If the LostItem is deleted, related Claims will also be deleted (Cascade delete)
        builder.Entity<Claim>()
             .HasOne(c => c.MatchedLostItem)
             .WithMany(l => l.Claims)
             .HasForeignKey(c => c.MatchedLostItemID)
             .OnDelete(DeleteBehavior.Cascade);

        // Configures the relationship between Claim and User
        // Each Claim belongs to one User
        // One User can submit multiple Claims
        // If the User is deleted, their Claims will also be deleted (Cascade delete)
        builder.Entity<Claim>()
            .HasOne(c => c.IdentityUser)
            .WithMany(u => u.Claims)
            .HasForeignKey(c => c.UserID)
            .OnDelete(DeleteBehavior.Cascade);
    }

    // Customize the ASP.NET Identity model and override the defaults if needed.
    // For example, you can rename the ASP.NET Identity table names and more.
    // Add your customizations after calling base.OnModelCreating(builder);


    // DbSet representing the LostItem table in the database
    // Entity Framework will create and manage this table
    public DbSet<APS_LostProperty.Models.LostItem> LostItem { get; set; } = default!;

    // DbSet representing the Location table
    // Stores all locations where lost items were found
    public DbSet<APS_LostProperty.Models.Location> Location { get; set; } = default!;

    // DbSet representing the Claim table
    // Stores all claims submitted by users
    public DbSet<APS_LostProperty.Models.Claim> Claim { get; set; } = default!;

    // DbSet representing the Category table
    // Stores the different categories that lost items can belong to
    public DbSet<APS_LostProperty.Models.Category> Category { get; set; } = default!;
}