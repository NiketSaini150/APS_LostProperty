using APS_LostProperty.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using APS_LostProperty.Areas.Identity.Data;

namespace APS_LostProperty.Areas.Identity.Data
{
    // This class runs when the app starts and fills the database with initial test data
    public static class DBInilitizer
    {
        public static void Initialize(DBContext context, UserManager<User> userManager /*, RoleManager<IdentityRole> roleManager */)
        {
            // makes sure database exists before doing anything
            context.Database.EnsureCreated();

            // ---------------- ROLE SETUP (COMMENTED OUT FOR NOW) ----------------
            /*
            if (!roleManager.RoleExistsAsync("Staff").Result)
                roleManager.CreateAsync(new IdentityRole("Staff")).Wait();

            if (!roleManager.RoleExistsAsync("Student").Result)
                roleManager.CreateAsync(new IdentityRole("Student")).Wait();
            */
            // -------------------------------------------------------------------

            // creates a default test user if it doesn't already exist
            var user = userManager.FindByEmailAsync("user1@test.com").Result;

            if (user == null)
            {
                user = new User
                {
                    UserName = "user1@test.com",
                    Email = "user1@test.com",
                    EmailConfirmed = true
                };

                userManager.CreateAsync(user, "Password123!").Wait();
            }

            // ================= CATEGORY SEEDING =================
            // only runs if categories table is empty
            if (!context.Category.Any())
            {
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

                // adds all categories into DB
                foreach (Category c in categories)
                {
                    context.Category.Add(c);
                }
                context.SaveChanges();
            }

            // ================= LOCATION SEEDING =================
            if (!context.Location.Any())
            {
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

                // adds all locations into DB
                foreach (Location l in locations)
                {
                    context.Location.Add(l);
                }
                context.SaveChanges();
            }

            // ================= LOST ITEMS SEEDING =================
            if (!context.LostItem.Any())
            {
                var lostItems = new LostItem[]
                {
                    new LostItem { ItemName="Black Phone", DateFound=DateTime.Now.AddDays(-2), LocationID=1, CategoryID=1 },
                    new LostItem { ItemName="Blue Jacket", DateFound=DateTime.Now.AddDays(-5), LocationID=2, CategoryID=2 },
                    new LostItem { ItemName="Laptop Bag", DateFound=DateTime.Now.AddDays(-1), LocationID=3, CategoryID=4 },
                    new LostItem { ItemName="Math Textbook", DateFound=DateTime.Now.AddDays(-3), LocationID=4, CategoryID=3 },
                    new LostItem { ItemName="Red Scarf", DateFound=DateTime.Now.AddDays(-4), LocationID=5, CategoryID=2 },
                    new LostItem { ItemName="Water Bottle", DateFound=DateTime.Now.AddDays(-6), LocationID=6, CategoryID=8 },
                    new LostItem { ItemName="Gym Shoes", DateFound=DateTime.Now.AddDays(-2), LocationID=2, CategoryID=19 },
                    new LostItem { ItemName="Notebook", DateFound=DateTime.Now.AddDays(-3), LocationID=1, CategoryID=3 },

                    new LostItem { ItemName="AirPods", Description="White Apple earbuds", DateFound=DateTime.Parse("2026-03-01"), LocationID=1, CategoryID=9 },
                    new LostItem { ItemName="School Backpack", Description="Nike black backpack", DateFound=DateTime.Parse("2026-03-02"), LocationID=2, CategoryID=4 },
                    new LostItem { ItemName="Calculator", Description="Casio scientific calculator", DateFound=DateTime.Parse("2026-03-03"), LocationID=4, CategoryID=3 },
                    new LostItem { ItemName="Football", Description="White training football", DateFound=DateTime.Parse("2026-03-03"), LocationID=8, CategoryID=5 },
                    new LostItem { ItemName="Glasses", Description="Black reading glasses", DateFound=DateTime.Now.AddDays(-1), LocationID=3, CategoryID=11 },
                    new LostItem { ItemName="Water Bottle Steel", Description="Metal bottle", DateFound=DateTime.Now.AddDays(-2), LocationID=6, CategoryID=8 }
                };

                // adds all lost items into DB
                foreach (LostItem li in lostItems)
                {
                    context.LostItem.Add(li);
                }
                context.SaveChanges();
            }

            // ================= CLAIMS SEEDING =================
            if (!context.Claim.Any())
            {
                var claims = new Claim[]
                {
                    new Claim { UserID=user.Id, ItemName="Black Backpack", Description="Lost near library", DateLost=DateTime.Parse("2026-02-28"), Status=ClaimStatus.Submitted, MatchedLostItemID=1 },
                    new Claim { UserID=user.Id, ItemName="Blue Jacket", Description="Lost during sports", DateLost=DateTime.Parse("2026-03-01"), Status=ClaimStatus.Approved, MatchedLostItemID=2 },
                    new Claim { UserID=user.Id, ItemName="Calculator", Description="Left in math class", DateLost=DateTime.Parse("2026-03-01"), Status=ClaimStatus.Submitted, MatchedLostItemID=3 },
                    new Claim { UserID=user.Id, ItemName="Football", Description="Lost after practice", DateLost=DateTime.Parse("2026-03-02"), Status=ClaimStatus.Submitted, MatchedLostItemID=4 },
                    new Claim { UserID=user.Id, ItemName="AirPods", Description="Lost in cafeteria", DateLost=DateTime.Parse("2026-03-02"), Status=ClaimStatus.Submitted, MatchedLostItemID=9 },
                    new Claim { UserID=user.Id, ItemName="Glasses", Description="Left in classroom", DateLost=DateTime.Parse("2026-03-03"), Status=ClaimStatus.Submitted, MatchedLostItemID=13 }
                };

                // adds all claims into DB
                foreach (Claim c in claims)
                {
                    context.Claim.Add(c);
                }
                context.SaveChanges();
            }
        }
    }
}