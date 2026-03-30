SELECT AVG(ItemCount) AS AverageItemsLost
FROM
(
    SELECT COUNT(LostItemID) AS ItemCount
    FROM LostItem
    GROUP BY LocationID
) AS LocationCounts;