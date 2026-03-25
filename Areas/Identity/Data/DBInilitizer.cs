using APS_LostProperty.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using APS_LostProperty.Areas.Identity.Data;


namespace APS_LostProperty.Areas.Identity.Data
{
    public static class DBInilitizer
    {
        public static void Initialize(DBContext context)
        {

            {
                context.Database.Migrate();

                if (context.LostItem.Any())
                {
                    return;
                }

                var lostItems = new LostItem[]
                {
                new LostItem { ItemName = "Black Backpack", Description = "Nike backpack", Location = "Library", DateLost = DateTime.Now.AddDays(-1) },
                new LostItem { ItemName = "Blue Water Bottle", Description = "Metal bottle", Location = "Gym", DateLost = DateTime.Now.AddDays(-2) },
                new LostItem { ItemName = "iPhone 12", Description = "Black case", Location = "Cafeteria", DateLost = DateTime.Now.AddDays(-3) },
                new LostItem { ItemName = "AirPods", Description = "White case", Location = "Hallway", DateLost = DateTime.Now.AddDays(-4) },
                new LostItem { ItemName = "Math Book", Description = "Year 12 calculus", Location = "Classroom A1", DateLost = DateTime.Now.AddDays(-5) },
                new LostItem { ItemName = "Laptop Charger", Description = "Dell charger", Location = "Computer Lab", DateLost = DateTime.Now.AddDays(-6) },
                new LostItem { ItemName = "Sports Shoes", Description = "Adidas runners", Location = "Gym", DateLost = DateTime.Now.AddDays(-7) },
                new LostItem { ItemName = "Wallet", Description = "Brown leather", Location = "Bus Stop", DateLost = DateTime.Now.AddDays(-8) },
                new LostItem { ItemName = "Keys", Description = "House keys with red tag", Location = "Car Park", DateLost = DateTime.Now.AddDays(-9) },
                new LostItem { ItemName = "Glasses", Description = "Black frame", Location = "Library", DateLost = DateTime.Now.AddDays(-10) }
            };

                context.LostItem.AddRange(LostItems;
                context.SaveChanges();
            }
        }
    }
}


    

