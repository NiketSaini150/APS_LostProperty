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
            // ===== 1. Locations =====
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
    new Category { Name="Miscellaneous"}
};

            foreach (Category c in categories)
            {
                context.Category.Add(c);
            }
            context.SaveChanges();

            // ===== 2. Categories =====
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
    new Location { LocationName="School Bus Area"}
 };

            foreach (Location l in locations)
            {
                context.Location.Add(l);
            }
            context.SaveChanges();

            var lostItems = new LostItem[]
  {
    new LostItem { ItemName="Black Backpack", Description="Nike backpack", DateFound=DateTime.Parse("2026-03-01"), CategoryID=4, LocationID=1 },
    new LostItem { ItemName="Blue Jacket", Description="Adidas sports jacket", DateFound=DateTime.Parse("2026-03-02"), CategoryID=2, LocationID=2 },
    new LostItem { ItemName="Calculator", Description="Casio scientific calculator", DateFound=DateTime.Parse("2026-03-03"), CategoryID=3, LocationID=4 },
    new LostItem { ItemName="Football", Description="White training football", DateFound=DateTime.Parse("2026-03-03"), CategoryID=5, LocationID=9 },
    new LostItem { ItemName="Silver Ring", Description="Small silver ring", DateFound=DateTime.Parse("2026-03-04"), CategoryID=6, LocationID=6 },
    new LostItem { ItemName="Math Textbook", Description="Year 12 maths book", DateFound=DateTime.Parse("2026-03-05"), CategoryID=7, LocationID=1 },
    new LostItem { ItemName="Metal Bottle", Description="Blue metal water bottle", DateFound=DateTime.Parse("2026-03-05"), CategoryID=8, LocationID=7 },
    new LostItem { ItemName="Wireless Headphones", Description="Sony headphones", DateFound=DateTime.Parse("2026-03-06"), CategoryID=9, LocationID=1 },
    new LostItem { ItemName="House Keys", Description="Three keys on ring", DateFound=DateTime.Parse("2026-03-06"), CategoryID=10, LocationID=13 },
    new LostItem { ItemName="Reading Glasses", Description="Black frame glasses", DateFound=DateTime.Parse("2026-03-07"), CategoryID=11, LocationID=1 },
    new LostItem { ItemName="Brown Wallet", Description="Leather wallet", DateFound=DateTime.Parse("2026-03-07"), CategoryID=12, LocationID=7 },
    new LostItem { ItemName="Smart Phone", Description="Samsung phone", DateFound=DateTime.Parse("2026-03-08"), CategoryID=13, LocationID=8 },
    new LostItem { ItemName="Laptop Charger", Description="Dell charger", DateFound=DateTime.Parse("2026-03-08"), CategoryID=14, LocationID=12 },
    new LostItem { ItemName="Lunch Box", Description="Plastic lunch container", DateFound=DateTime.Parse("2026-03-09"), CategoryID=15, LocationID=7 },
    new LostItem { ItemName="Black Umbrella", Description="Foldable umbrella", DateFound=DateTime.Parse("2026-03-09"), CategoryID=16, LocationID=16 },
    new LostItem { ItemName="Student ID Card", Description="School ID card", DateFound=DateTime.Parse("2026-03-10"), CategoryID=17, LocationID=13 },
    new LostItem { ItemName="Running Shoes", Description="White Nike shoes", DateFound=DateTime.Parse("2026-03-10"), CategoryID=18, LocationID=17 },
    new LostItem { ItemName="Baseball Cap", Description="Black cap", DateFound=DateTime.Parse("2026-03-11"), CategoryID=19, LocationID=19 },
    new LostItem { ItemName="Notebook", Description="Blue lined notebook", DateFound=DateTime.Parse("2026-03-11"), CategoryID=3, LocationID=4 },
    new LostItem { ItemName="USB Drive", Description="16GB USB stick", DateFound=DateTime.Parse("2026-03-12"), CategoryID=1, LocationID=12 }
  };

            foreach (LostItem li in lostItems)
            {
                context.LostItem.Add(li);
            }
            context.SaveChanges();

            var claims = new Claim[]
{
    new Claim { UserID="user1", ItemName="Black Backpack", Description="Lost near library", DateLost=DateTime.Parse("2026-02-28"), Status=ClaimStatus.Submitted, MatchedLostItemID=1 },
    new Claim { UserID="user1", ItemName="Blue Jacket", Description="Lost during sports", DateLost=DateTime.Parse("2026-03-01"), Status=ClaimStatus.Approved, MatchedLostItemID=2 },
    new Claim { UserID="user1", ItemName="Calculator", Description="Left in math class", DateLost=DateTime.Parse("2026-03-01"), Status=ClaimStatus.Submitted, MatchedLostItemID=3 },
    new Claim { UserID="user1", ItemName="Football", Description="Lost after practice", DateLost=DateTime.Parse("2026-03-02"), Status=ClaimStatus.Submitted, MatchedLostItemID=4 },
    new Claim { UserID="user1", ItemName="Silver Ring", Description="Lost in hall", DateLost=DateTime.Parse("2026-03-03"), Status=ClaimStatus.Rejected, MatchedLostItemID=5 },
    new Claim { UserID="user1", ItemName="Math Textbook", Description="Forgot in library", DateLost=DateTime.Parse("2026-03-03"), Status=ClaimStatus.Approved, MatchedLostItemID=6 },
    new Claim { UserID="user1", ItemName="Metal Bottle", Description="Left in cafeteria", DateLost=DateTime.Parse("2026-03-04"), Status=ClaimStatus.Submitted, MatchedLostItemID=7 },
    new Claim { UserID="user1", ItemName="Wireless Headphones", Description="Lost while studying", DateLost=DateTime.Parse("2026-03-05"), Status=ClaimStatus.Submitted, MatchedLostItemID=8 },
    new Claim { UserID="user1", ItemName="House Keys", Description="Dropped outside office", DateLost=DateTime.Parse("2026-03-05"), Status=ClaimStatus.Collected, MatchedLostItemID=9 },
    new Claim { UserID="user1", ItemName="Reading Glasses", Description="Lost in library", DateLost=DateTime.Parse("2026-03-06"), Status=ClaimStatus.Submitted, MatchedLostItemID=10 },
    new Claim { UserID="user1", ItemName="Brown Wallet", Description="Left in cafeteria", DateLost=DateTime.Parse("2026-03-06"), Status=ClaimStatus.Submitted, MatchedLostItemID=11 },
    new Claim { UserID="user1", ItemName="Smart Phone", Description="Lost during break", DateLost=DateTime.Parse("2026-03-07"), Status=ClaimStatus.Approved, MatchedLostItemID=12 },
    new Claim { UserID="user1", ItemName="Laptop Charger", Description="Forgot in lab", DateLost=DateTime.Parse("2026-03-07"), Status=ClaimStatus.Submitted, MatchedLostItemID=13 },
    new Claim { UserID="user1", ItemName="Lunch Box", Description="Left at lunch", DateLost=DateTime.Parse("2026-03-08"), Status=ClaimStatus.Submitted, MatchedLostItemID=14 },
    new Claim { UserID="user1", ItemName="Black Umbrella", Description="Lost near bus area", DateLost=DateTime.Parse("2026-03-08"), Status=ClaimStatus.Rejected, MatchedLostItemID=15 },
    new Claim { UserID="user1", ItemName="Student ID Card", Description="Dropped near office", DateLost=DateTime.Parse("2026-03-09"), Status=ClaimStatus.Submitted, MatchedLostItemID=16 },
    new Claim { UserID="user1", ItemName="Running Shoes", Description="Lost in changing room", DateLost=DateTime.Parse("2026-03-09"), Status=ClaimStatus.Submitted, MatchedLostItemID=17 },
    new Claim { UserID="user1", ItemName="Baseball Cap", Description="Lost in quad", DateLost=DateTime.Parse("2026-03-10"), Status=ClaimStatus.Approved, MatchedLostItemID=18 },
    new Claim { UserID="user1", ItemName="Notebook", Description="Left in classroom", DateLost=DateTime.Parse("2026-03-10"), Status=ClaimStatus.Submitted, MatchedLostItemID=19 },
    new Claim { UserID="user1", ItemName="USB Drive", Description="Lost in lab", DateLost=DateTime.Parse("2026-03-11"), Status=ClaimStatus.Submitted, MatchedLostItemID=20 }
};

            foreach (Claim c in claims)
            {
                context.Claim.Add(c);
            }
            context.SaveChanges();


        }
    }
    }
