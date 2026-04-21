SELECT DATENAME(MONTH, DateFound) AS MonthName,
       COUNT(*) AS TotalItems
FROM LostItem
GROUP BY DATENAME(MONTH, DateFound), MONTH(DateFound)
ORDER BY MONTH(DateFound);