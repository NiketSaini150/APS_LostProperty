-- Delete child table first
DELETE FROM Claim;

-- Then delete dependent LostItems
DELETE FROM LostItem;

-- Then delete parent tables
DELETE FROM Category;
DELETE FROM Location;

-- Optionally, reset identity columns to start from 1
DBCC CHECKIDENT ('Claim', RESEED, 0);
DBCC CHECKIDENT ('LostItem', RESEED, 0);
DBCC CHECKIDENT ('Category', RESEED, 0);
DBCC CHECKIDENT ('Location', RESEED, 0);