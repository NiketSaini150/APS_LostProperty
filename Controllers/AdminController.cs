using APS_LostProperty.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;

namespace APS_LostProperty.Controllers
{
    // Admin dashboard controller handles system stats + SQL execution
    public class AdminController : Controller
    {
        private readonly DBContext _context;

        public AdminController(DBContext context)
        {
            _context = context;
        }

        // ---------------- DASHBOARD PAGE ----------------
        public IActionResult Index()
        {
            // live system counters for dashboard display
            ViewBag.TotalItems = _context.LostItem.Count();
            ViewBag.TotalClaims = _context.Claim.Count();
            ViewBag.TotalCategories = _context.Category.Count();
            ViewBag.TotalLocations = _context.Location.Count();

            // list of SQL queries shown to admin
            ViewBag.Queries = new List<object>
            {
                new
                {
                    Name = "UnclaimedItems",
                    Description = "Displays all lost items that have NOT been claimed yet, including their category and location details.",
                    Sql = @"
SELECT 
    LI.ItemName, 
    LI.Description, 
    LI.DateFound, 
    L.LocationName, 
    C.Name AS Category,
    CASE 
        WHEN LI.IsClaimed = 1 THEN 'Claimed' 
        ELSE 'Not Claimed' 
    END AS IsClaimedStatus
FROM LostItem AS LI
JOIN Location AS L ON LI.LocationID = L.LocationID
JOIN Category AS C ON LI.CategoryID = C.CategoryID
WHERE LI.IsClaimed = 0"
                },

                new
                {
                    Name = "MostItemsByLocation",
                    Description = "Shows which location has the highest number of lost items reported in the system.",
                    Sql = @"
SELECT TOP 1
    L.LocationName,
    COUNT(LI.LostItemID) AS TotalLostItems
FROM Location L
JOIN LostItem LI ON L.LocationID = LI.LocationID
GROUP BY L.LocationName
ORDER BY TotalLostItems DESC"
                },

                new
                {
                    Name = "ItemsPerLocation",
                    Description = "Displays the total number of lost items found at each location in the school.",
                    Sql = @"
SELECT 
    L.LocationName,
    COUNT(LI.LostItemID) AS TotalItemsFound
FROM Location L
JOIN LostItem LI ON L.LocationID = LI.LocationID
GROUP BY L.LocationName
ORDER BY TotalItemsFound DESC"
                },

                new
                {
                    Name = "LocationRiskAnalysis",
                    Description = "Identifies high-risk locations where lost item frequency exceeds a threshold (useful for admin monitoring).",
                    Sql = @"
DECLARE @SearchLocation NVARCHAR(50)
SET @SearchLocation = 'Library'

SELECT 
    L.LocationName,
    COUNT(LI.LostItemID) AS TotalItemsLost,
    CASE 
        WHEN COUNT(LI.LostItemID) > 3 THEN 'Risk'
        ELSE 'Normal'
    END AS RiskLevel
FROM Location L
JOIN LostItem LI ON L.LocationID = LI.LocationID
WHERE L.LocationName LIKE '%' + @SearchLocation + '%'
GROUP BY L.LocationName"
                }
            };

            return View();
        }

        // ---------------- RUN QUERY ----------------
        [HttpPost]
        public IActionResult RunQuery(string sql)
        {
            DataTable table = new DataTable();

            // open SQL connection using EF Core connection string
            using (var conn = new SqlConnection(_context.Database.GetConnectionString()))
            {
                conn.Open();

                using (var cmd = new SqlCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        table.Load(reader);
                    }
                }
            }

            // send query results to view
            ViewBag.ResultTable = table;

            // reload dashboard counters
            ViewBag.TotalItems = _context.LostItem.Count();
            ViewBag.TotalClaims = _context.Claim.Count();
            ViewBag.TotalCategories = _context.Category.Count();
            ViewBag.TotalLocations = _context.Location.Count();

            // reload query list (same as index)
            ViewBag.Queries = new List<object>
            {
                new
                {
                    Name = "UnclaimedItems",
                    Description = "Displays all lost items that have NOT been claimed yet, including their category and location details.",
                    Sql = @"SELECT 
    LI.ItemName, 
    LI.Description, 
    LI.DateFound, 
    L.LocationName, 
    C.Name AS Category,
    CASE 
        WHEN LI.IsClaimed = 1 THEN 'Claimed' 
        ELSE 'Not Claimed' 
    END AS IsClaimedStatus
FROM LostItem AS LI
JOIN Location AS L ON LI.LocationID = L.LocationID
JOIN Category AS C ON LI.CategoryID = C.CategoryID
WHERE LI.IsClaimed = 0"
                },

                new
                {
                    Name = "MostItemsByLocation",
                    Description = "Shows which location has the highest number of lost items reported in the system.",
                    Sql = @"SELECT TOP 1
    L.LocationName,
    COUNT(LI.LostItemID) AS TotalLostItems
FROM Location L
JOIN LostItem LI ON L.LocationID = LI.LocationID
GROUP BY L.LocationName
ORDER BY TotalLostItems DESC"
                },

                new
                {
                    Name = "ItemsPerLocation",
                    Description = "Displays the total number of lost items found at each location in the school.",
                    Sql = @"SELECT 
    L.LocationName,
    COUNT(LI.LostItemID) AS TotalItemsFound
FROM Location L
JOIN LostItem LI ON L.LocationID = LI.LocationID
GROUP BY L.LocationName
ORDER BY TotalItemsFound DESC"
                },

                new
                {
                    Name = "LocationRiskAnalysis",
                    Description = "Identifies high-risk locations where lost item frequency exceeds a threshold (useful for admin monitoring).",
                    Sql = @"DECLARE @SearchLocation NVARCHAR(50)
SET @SearchLocation = 'Library'

SELECT 
    L.LocationName,
    COUNT(LI.LostItemID) AS TotalItemsLost,
    CASE 
        WHEN COUNT(LI.LostItemID) > 3 THEN 'Risk'
        ELSE 'Normal'
    END AS RiskLevel
FROM Location L
JOIN LostItem LI ON L.LocationID = LI.LocationID
WHERE L.LocationName LIKE '%' + @SearchLocation + '%'
GROUP BY L.LocationName"
                }
            };

            return View("Index");
        }
    }
}