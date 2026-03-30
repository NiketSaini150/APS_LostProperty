DECLARE @SearchLocation NVARCHAR(50)

SET @SearchLocation = 'Library'

SELECT 
    L.LocationName,
    COUNT(LI.LostItemID) AS TotalItemsLost,
    CASE 
        WHEN COUNT(LI.LostItemID) > 3 THEN 'Risk'
        ELSE 'Normal'
    END AS RiskLevel
FROM Location L, LostItem LI
WHERE L.LocationID = LI.LocationID
AND L.LocationName LIKE '%' + @SearchLocation + '%'
GROUP BY L.LocationName;