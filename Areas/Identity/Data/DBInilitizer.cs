using APS_LostProperty.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using APS_LostProperty.Areas.Identity.Data;


namespace APS_LostProperty.Areas.Identity.Data
{
    public static class DBInilitizer
    {

        public static void Initialize(DBContext context, UserManager<User> userManager)
        {
            context.Database.EnsureCreated();
            // Make sure the database exists
          //  context.Database.Migrate();

            // Look for any products.
            if (context.Category.Any())
            {
                return;   // DB has been seeded
            }
            // ===== 1. Category =====
            var categories = new Category[]
{
    new Category { Name="Electronics"},
    new Category { Name="Clothing"},
    new Category { Name="Stationery"},
    new Category { Name="Bags"},
    new Category { Name="Sports Gear"},
    new Category { Name="Jewellery"},
    new Category { Name="Books"},
    new Category { Name="Water Bottles"},
    new Category { Name="Headphones"},
    new Category { Name="Keys"},
    new Category { Name="Glasses"},
    new Category { Name="Wallets"},
    new Category { Name="Phones"},
    new Category { Name="Chargers"},
    new Category { Name="Lunch Boxes"},
    new Category { Name="Umbrellas"},
    new Category { Name="ID Cards"},
    new Category { Name="Shoes"},
    new Category { Name="Hats"},
    new Category { Name="Miscellaneous"},
    new Category { Name = "Electronics" },
        new Category { Name = "Clothing" },
        new Category { Name = "Bags" },
        new Category { Name = "Books" }
};

            foreach (Category c in categories)
            {
                context.Category.Add(c);
            }
            context.SaveChanges();

            // ===== 2. Location =====
            var locations = new Location[]
 {
    new Location { LocationName="Library"},
    new Location { LocationName="Gym"},
    new Location { LocationName="Science Block"},
    new Location { LocationName="Math Block"},
    new Location { LocationName="English Block"},
    new Location { LocationName="Hall"},
    new Location { LocationName="Cafeteria"},
    new Location { LocationName="Basketball Court"},
    new Location { LocationName="Football Field"},
    new Location { LocationName="Music Room"},
    new Location { LocationName="Art Room"},
    new Location { LocationName="Computer Lab"},
    new Location { LocationName="Front Office"},
    new Location { LocationName="Student Services"},
    new Location { LocationName="Staff Room"},
    new Location { LocationName="Parking Area"},
    new Location { LocationName="Changing Rooms"},
    new Location { LocationName="Drama Studio"},
    new Location { LocationName="Outdoor Quad"},
    new Location { LocationName="School Bus Area"},
     new Location { LocationName = "Library" },
        new Location { LocationName = "Cafeteria" },
        new Location { LocationName = "Gym" },
        new Location { LocationName = "Main Hall" },
        new Location { LocationName = "Computer Lab" }
 };

            foreach (Location l in locations)
            {
                context.Location.Add(l);
            }
            context.SaveChanges();

            var lostItems = new LostItem[]
  {



        new LostItem { ItemName="Black Phone", DateFound=DateTime.Now.AddDays(-2), LocationID=locations[0].LocationID, CategoryID=categories[0].CategoryID },
        new LostItem { ItemName="Blue Jacket", DateFound=DateTime.Now.AddDays(-5), LocationID=locations[0].LocationID, CategoryID=categories[1].CategoryID },
        new LostItem { ItemName="Laptop Bag", DateFound=DateTime.Now.AddDays(-1), LocationID=locations[0].LocationID, CategoryID=categories[2].CategoryID },
        new LostItem { ItemName="Math Textbook", DateFound=DateTime.Now.AddDays(-3), LocationID=locations[0].LocationID, CategoryID=categories[3].CategoryID },
        new LostItem { ItemName="Red Scarf", DateFound=DateTime.Now.AddDays(-4), LocationID=locations[1].LocationID, CategoryID=categories[1].CategoryID },
        new LostItem { ItemName="Water Bottle", DateFound=DateTime.Now.AddDays(-6), LocationID=locations[1].LocationID, CategoryID=categories[2].CategoryID },
        new LostItem { ItemName="Gym Shoes", DateFound=DateTime.Now.AddDays(-2), LocationID=locations[2].LocationID, CategoryID=categories[1].CategoryID },
        new LostItem { ItemName="Notebook", DateFound=DateTime.Now.AddDays(-3), LocationID=locations[2].LocationID, CategoryID=categories[3].CategoryID },
        new LostItem { ItemName="Black Backpack", Description="Nike backpack", DateFound=DateTime.Parse("2026-03-01"), CategoryID=categories[3].CategoryID, LocationID=locations[0].LocationID },
    new LostItem { ItemName="Blue Jacket", Description="Adidas sports jacket", DateFound=DateTime.Parse("2026-03-02"), CategoryID=categories[1].CategoryID, LocationID=locations[1].LocationID },
    new LostItem { ItemName="Calculator", Description="Casio scientific calculator", DateFound=DateTime.Parse("2026-03-03"), CategoryID=categories[2].CategoryID, LocationID=locations[2].LocationID },
    new LostItem { ItemName="Football", Description="White training football", DateFound=DateTime.Parse("2026-03-03"), CategoryID=categories[4].CategoryID, LocationID=locations[8].LocationID }
  };

            foreach (LostItem li in lostItems)
            {
                context.LostItem.Add(li);
            }
            context.SaveChanges();

            var claims = new Claim[]
{
    new Claim { UserID="user1", ItemName="Black Backpack", Description="Lost near library", DateLost=DateTime.Parse("2026-02-28"), Status=ClaimStatus.Submitted, MatchedLostItemID=lostItems[0].LostItemID },
    new Claim { UserID="user1", ItemName="Blue Jacket", Description="Lost during sports", DateLost=DateTime.Parse("2026-03-01"), Status=ClaimStatus.Approved, MatchedLostItemID=lostItems[1].LostItemID },
    new Claim { UserID="user1", ItemName="Calculator", Description="Left in math class", DateLost=DateTime.Parse("2026-03-01"), Status=ClaimStatus.Submitted, MatchedLostItemID=lostItems[2].LostItemID },
    new Claim { UserID="user1", ItemName="Football", Description="Lost after practice", DateLost=DateTime.Parse("2026-03-02"), Status=ClaimStatus.Submitted, MatchedLostItemID=lostItems[3].LostItemID }
    // … more claims here
};
        

            foreach (Claim c in claims)
            {
                context.Claim.Add(c);
            }
            context.SaveChanges();


        }
    }
    }
