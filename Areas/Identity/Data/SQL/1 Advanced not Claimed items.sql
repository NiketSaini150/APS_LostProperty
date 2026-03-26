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
WHERE LI.IsClaimed = 0;
