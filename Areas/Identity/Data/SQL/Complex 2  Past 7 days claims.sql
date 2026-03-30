SELECT ClaimID, ItemName, DateSubmitted
FROM Claim
WHERE DateSubmitted >= DATEADD(DAY, -7, GETDATE());