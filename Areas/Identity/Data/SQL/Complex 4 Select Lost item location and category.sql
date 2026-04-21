SELECT item.ItemName, item.Description, item.DateFound, loc.LocationName, cat.Name AS Category
FROM LostItem item
JOIN Location loc ON item.LocationID = loc.LocationID
JOIN Category cat ON item.CategoryID = cat.CategoryID;