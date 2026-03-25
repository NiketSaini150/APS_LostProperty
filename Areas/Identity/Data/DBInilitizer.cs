using APS_LostProperty.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using APS_LostProperty.Areas.Identity.Data;


namespace APS_LostProperty.Areas.Identity.Data
{
    public class DBInilitizer
    {



        public static async Task InitializeAsync(DBContext context, UserManager<User> userManager)
        {
            // Apply migrations
            context.Database.Migrate();

            var rnd = new Random();

            // ---------- Seed Users ----------
            if (!context.Users.Any())
            {
                var users = new List<User>();
                for (int i = 1; i <= 10; i++)
                {
                    var user = new User
                    {
                        UserName = $"student{i}@school.com",
                        Email = $"student{i}@school.com",
                        FirstName = $"Student{i}",
                        LastName = $"Test{i}",
                        DateRegistered = DateTime.Now
                    };
                    await userManager.CreateAsync(user, "Password123!");
                    users.Add(user);
                }
            }

            // ---------- Seed Categories ----------
            if (!context.Category.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Stationery" },
                    new Category { Name = "Electronics" },
                    new Category { Name = "Clothing" },
                    new Category { Name = "Accessories" },
                    new Category { Name = "Other" },
                    new Category { Name = "Books" },
                    new Category { Name = "Sports Gear" },
                    new Category { Name = "Bags" },
                    new Category { Name = "Uniforms" },
                    new Category { Name = "Misc" }
                };
                context.Category.AddRange(categories);
                await context.SaveChangesAsync();
            }

            // ---------- Seed Locations ----------
            if (!context.Location.Any())
            {
                var locations = new List<Location>
                {
                    new Location { LocationName = "A Block" },
                    new Location { LocationName = "B Block" },
                    new Location { LocationName = "C Block" },
                    new Location { LocationName = "Library" },
                    new Location { LocationName = "Cafeteria" },
                    new Location { LocationName = "Gym" },
                    new Location { LocationName = "Playground" },
                    new Location { LocationName = "Lab 1" },
                    new Location { LocationName = "Lab 2" },
                    new Location { LocationName = "Admin Office" }
                };
                context.Location.AddRange(locations);
                await context.SaveChangesAsync();
            }

            // ---------- Seed Lost Items ----------
            if (!context.LostItem.Any())
            {
                var categories = context.Category.ToList();
                var locations = context.Location.ToList();
                var users = context.Users.ToList();

                for (int i = 1; i <= 10; i++)
                {
                    var lostItem = new LostItem
                    {
                        ItemName = $"LostItem{i}",
                        Description = $"Description for LostItem{i}",
                        DateFound = DateTime.Now.AddDays(-rnd.Next(1, 30)),
                        CategoryID = categories[rnd.Next(categories.Count)].CategoryID,
                        LocationID = locations[rnd.Next(locations.Count)].LocationID,
                        IsClaimed = false
                    };
                    context.LostItem.Add(lostItem);
                }
                await context.SaveChangesAsync();
            }

            // ---------- Seed Claims ----------
            if (!context.Claim.Any())
            {
                var lostItems = context.LostItem.ToList();
                var users = context.Users.ToList();

                for (int i = 1; i <= 10; i++)
                {
                    var claim = new Claim
                    {
                        UserID = users[rnd.Next(users.Count)].Id,
                        ItemName = lostItems[rnd.Next(lostItems.Count)].ItemName,
                        Description = "I believe this is mine",
                        DateLost = DateTime.Now.AddDays(-rnd.Next(1, 30)),
                        DateSubmitted = DateTime.Now.AddDays(-rnd.Next(0, 10)),
                        Status = ClaimStatus.Submitted,
                        MatchedLostItemID = lostItems[rnd.Next(lostItems.Count)].LostItemID
                    };
                    context.Claim.Add(claim);
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
