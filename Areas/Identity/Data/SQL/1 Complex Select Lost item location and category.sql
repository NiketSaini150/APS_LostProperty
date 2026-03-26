Select item.ItemName, item.Description, item.DateFound, loc.LocationName, cat.Name as Category
from LostItem item, Location loc, Category cat
where item.LocationID = loc.LocationID