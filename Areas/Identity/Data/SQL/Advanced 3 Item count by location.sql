SELECT 
    LO.LocationName,
    COUNT(L.LostItemID) AS TotalItemsFound
FROM Location LO, LostItem L
WHERE LO.LocationID = L.LocationID
GROUP BY LO.LocationName
ORDER BY TotalItemsFound DESC;