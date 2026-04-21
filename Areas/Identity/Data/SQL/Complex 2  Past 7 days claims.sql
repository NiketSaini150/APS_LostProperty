SELECT ClaimID, ClaimedItemName, DateSubmitted
FROM Claim
WHERE DateSubmitted >= DATEADD(DAY, -7, GETDATE());