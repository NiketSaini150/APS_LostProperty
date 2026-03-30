SELECT 
    L.LostItemID,
    L.ItemName,
    C.Name AS Category,
    LO.LocationName,
    L.DateFound,
    L.IsClaimed
FROM LostItem L
INNER JOIN Category C ON L.CategoryID = C.CategoryID
INNER JOIN Location LO ON L.LocationID = LO.LocationID
ORDER BY L.DateFound DESC;