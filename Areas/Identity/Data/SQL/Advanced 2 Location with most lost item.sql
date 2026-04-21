SELECT TOP 1
    L.LocationName,
    COUNT(LI.LostItemID) AS TotalLostItems
FROM Location L, LostItem LI
WHERE L.LocationID = LI.LocationID
GROUP BY L.LocationName
ORDER BY TotalLostItems ;