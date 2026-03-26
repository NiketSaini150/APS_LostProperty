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
                // Make sure the database exists
                context.Database.Migrate();

                // ===== 1. Locations =====
                if (!context.Location.Any())
                {
                    var locations = new Location[]
                    {
                    new Location { LocationName = "Library" },
                    new Location { LocationName = "Gym" },
                    new Location { LocationName = "Cafeteria" },
                    new Location { LocationName = "Hallway" },
                    new Location { LocationName = "Computer Lab" },
                    new Location { LocationName = "Art Room" },
                    new Location { LocationName = "Sports Field" },
                    new Location { LocationName = "Entrance" },
                    new Location { LocationName = "Bus Stop" },
                    new Location { LocationName = "Classroom A1" },
                    new Location { LocationName = "Classroom B2" },
                    new Location { LocationName = "Classroom C3" },
                    new Location { LocationName = "Classroom D4" },
                    new Location { LocationName = "Classroom E5" },
                    new Location { LocationName = "Classroom F6" },
                    new Location { LocationName = "Music Room" },
                    new Location { LocationName = "Science Lab" },
                    new Location { LocationName = "Storage Room" },
                    new Location { LocationName = "Media Room" },
                    new Location { LocationName = "Common Room" },
                    new Location { LocationName = "Bathroom" },
                    new Location { LocationName = "Bike Rack" },
                    new Location { LocationName = "Locker Area" },
                    new Location { LocationName = "Auditorium" },
                    new Location { LocationName = "Playground" },
                    new Location { LocationName = "Staff Room" },
                    new Location { LocationName = "Principal Office" },
                    new Location { LocationName = "Nurse Office" },
                    new Location { LocationName = "Canteen" },
                    new Location { LocationName = "Outdoor Court" }
                    };
                    context.Location.AddRange(locations);
                    context.SaveChanges();
                }

                // ===== 2. Categories =====
                if (!context.Category.Any())
                {
                    var categories = new Category[]
                    {
                    new Category { Name = "Electronics" },
                    new Category { Name = "Clothing" },
                    new Category { Name = "Stationery" },
                    new Category { Name = "Accessories" },
                    new Category { Name = "Sports Equipment" },
                    new Category { Name = "Books" },
                    new Category { Name = "Bags" },
                    new Category { Name = "Jewelry" },
                    new Category { Name = "Food & Drink" },
                    new Category { Name = "Miscellaneous" }
                    };
                    context.Category.AddRange(categories);
                    context.SaveChanges();
                }

                // ===== 3. Users =====
                if (!userManager.Users.Any())
                {
                    var users = new User[]
                    {
                    new User { UserName = "alice@example.com", Email = "alice@example.com", FirstName = "Alice", LastName = "Smith" },
                    new User { UserName = "bob@example.com", Email = "bob@example.com", FirstName = "Bob", LastName = "Johnson" },
                    new User { UserName = "carol@example.com", Email = "carol@example.com", FirstName = "Carol", LastName = "Lee" },
                    new User { UserName = "dave@example.com", Email = "dave@example.com", FirstName = "Dave", LastName = "Brown" },
                    new User { UserName = "eve@example.com", Email = "eve@example.com", FirstName = "Eve", LastName = "Davis" }
                    };

                    foreach (var user in users)
                    {
                        var result = userManager.CreateAsync(user, "Password123!").Result;
                        if (!result.Succeeded)
                        {
                            Console.WriteLine($"Failed to create user {user.Email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                        }
                    }
                }

                var locationList = context.Location.ToList();
                var categoryList = context.Category.ToList();
                var userList = userManager.Users.ToList();

                // ===== 4. LostItems =====
                if (!context.LostItem.Any())
                {
                    var lostItemNames = new string[]
                    {
                    "Blue Water Bottle","Black Backpack","iPhone 12","AirPods","Math Book","Laptop Charger",
                    "Sports Shoes","Wallet","Keys","Glasses","Notebook","Calculator","Umbrella","Headphones",
                    "School Jacket","USB Drive","Art Folder","Basketball","Watch","Tablet","Pen Case","Lunchbox",
                    "Beanie","Ring","Scarf","Gloves","Laptop","Camera","Project Folder","Sports Bag","Hat",
                    "Sunglasses","Phone Charger","Binder","Textbook","Flash Drive","Earbuds","Keycard",
                    "Gym Towel","Water Bottle","Mouse","Keyboard","Notebook 2","Drawing Tablet","Portable Speaker",
                    "Flashlight","Helmet","Planner","Fitness Band","Gaming Controller"
                    };

                    var lostItems = lostItemNames.Select((name, index) => new LostItem
                    {
                        ItemName = name,
                        Description = $"Description for {name}",
                        DateFound = DateTime.Now.AddDays(-index),
                        LocationID = locationList[index % locationList.Count].LocationID,
                        CategoryID = categoryList[index % categoryList.Count].CategoryID,
                        IsClaimed = false
                    }).ToArray();

                    context.LostItem.AddRange(lostItems);
                    context.SaveChanges();
                }

                // ===== 5. Claims =====
                if (!context.Claim.Any())
                {
                    var lostItemList = context.LostItem.Take(10).ToList();
                    var claims = lostItemList.Select((item, i) => new Claim
                    {
                        UserID = userList[i % userList.Count].Id,
                        ItemName = item.ItemName,
                        Description = item.Description,
                        DateLost = item.DateFound.AddDays(-2),
                        MatchedLostItemID = item.LostItemID,
                        Status = ClaimStatus.Submitted
                    }).ToArray();

                    context.Claim.AddRange(claims);
                    context.SaveChanges();
                }

                Console.WriteLine("Database seeding complete!");
            }
        }
    }
