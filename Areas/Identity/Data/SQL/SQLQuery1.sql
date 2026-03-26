-- =======================
-- 1️⃣ Locations (30)
-- =======================
INSERT INTO Location (LocationName) VALUES
('Library'),('Gym'),('Cafeteria'),('Hallway'),('Computer Lab'),
('Art Room'),('Sports Field'),('Entrance'),('Bus Stop'),('Classroom A1'),
('Classroom B2'),('Classroom C3'),('Classroom D4'),('Classroom E5'),('Classroom F6'),
('Music Room'),('Science Lab'),('Storage Room'),('Media Room'),('Common Room'),
('Bathroom'),('Bike Rack'),('Locker Area'),('Auditorium'),('Playground'),
('Staff Room'),('Principal Office'),('Nurse Office'),('Canteen'),('Outdoor Court');

-- =======================
-- 2️⃣ Categories (10)
-- =======================
INSERT INTO Category (Name) VALUES
('Electronics'),('Clothing'),('Stationery'),('Accessories'),('Sports Equipment'),
('Books'),('Bags'),('Jewelry'),('Food & Drink'),('Miscellaneous');

-- =======================
-- 3️⃣ Users (5 sample Identity users)
-- =======================
INSERT INTO AspNetUsers
(Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, FirstName, LastName, DateRegistered)
VALUES
('11111111-1111-1111-1111-111111111111', 'student1', 'STUDENT1', 'student1@school.nz', 'STUDENT1@SCHOOL.NZ', 1,
 'AQAAAAEAACcQAAAAEG6E5dI3d1TqvH5yDqFh6sK7yQG+2lJx1UoQ2zMxCJ0I6V1JYbN2tq/NVQF0T/7OXg==',
 'STAMP1111', NEWID(), 0, 0, 0, 0, 'Niket', 'Saini', GETDATE()),
('22222222-2222-2222-2222-222222222222', 'student2', 'STUDENT2', 'student2@school.nz', 'STUDENT2@SCHOOL.NZ', 1,
 'AQAAAAEAACcQAAAAEG6E5dI3d1TqvH5yDqFh6sK7yQG+2lJx1UoQ2zMxCJ0I6V1JYbN2tq/NVQF0T/7OXg==',
 'STAMP2222', NEWID(), 0, 0, 0, 0, 'Jane', 'Doe', GETDATE()),
('33333333-3333-3333-3333-333333333333', 'student3', 'STUDENT3', 'student3@school.nz', 'STUDENT3@SCHOOL.NZ', 1,
 'AQAAAAEAACcQAAAAEG6E5dI3d1TqvH5yDqFh6sK7yQG+2lJx1UoQ2zMxCJ0I6V1JYbN2tq/NVQF0T/7OXg==',
 'STAMP3333', NEWID(), 0, 0, 0, 0, 'John', 'Smith', GETDATE()),
('44444444-4444-4444-4444-444444444444', 'student4', 'STUDENT4', 'student4@school.nz', 'STUDENT4@SCHOOL.NZ', 1,
 'AQAAAAEAACcQAAAAEG6E5dI3d1TqvH5yDqFh6sK7yQG+2lJx1UoQ2zMxCJ0I6V1JYbN2tq/NVQF0T/7OXg==',
 'STAMP4444', NEWID(), 0, 0, 0, 0, 'Alice', 'Brown', GETDATE()),
('55555555-5555-5555-5555-555555555555', 'student5', 'STUDENT5', 'student5@school.nz', 'STUDENT5@SCHOOL.NZ', 1,
 'AQAAAAEAACcQAAAAEG6E5dI3d1TqvH5yDqFh6sK7yQG+2lJx1UoQ2zMxCJ0I6V1JYbN2tq/NVQF0T/7OXg==',
 'STAMP5555', NEWID(), 0, 0, 0, 0, 'Bob', 'Taylor', GETDATE());

-- =======================
-- 4️⃣ LostItems (50)
-- =======================
-- Note: LocationID = 1-30, CategoryID = 1-10
INSERT INTO LostItem (ItemName, Description, DateFound, LocationID, CategoryID, IsClaimed) VALUES
('Blue Water Bottle','Metal bottle', GETDATE()-1,1,1,0),
('Black Backpack','Nike backpack', GETDATE()-2,2,7,0),
('iPhone 12','Black case', GETDATE()-3,3,1,0),
('AirPods','White case', GETDATE()-4,4,1,0),
('Math Book','Year 12 calculus', GETDATE()-5,5,6,0),
('Laptop Charger','Dell charger', GETDATE()-6,6,1,0),
('Sports Shoes','Adidas runners', GETDATE()-7,2,5,0),
('Wallet','Brown leather', GETDATE()-8,9,4,0),
('Keys','House keys with red tag', GETDATE()-9,22,4,0),
('Glasses','Black frame', GETDATE()-10,1,4,0),
('Notebook','Blue notebook', GETDATE()-11,11,3,0),
('Calculator','Casio FX82', GETDATE()-12,12,3,0),
('Umbrella','Black umbrella', GETDATE()-13,8,4,0),
('Headphones','Sony headphones', GETDATE()-14,16,1,0),
('School Jacket','APS jacket', GETDATE()-15,7,2,0),
('USB Drive','32GB USB', GETDATE()-16,6,1,0),
('Art Folder','Drawing sheets', GETDATE()-17,6,6,0),
('Basketball','Spalding ball', GETDATE()-18,7,5,0),
('Watch','Silver watch', GETDATE()-19,21,4,0),
('Tablet','Samsung tablet', GETDATE()-20,1,1,0),
('Pen Case','Red case', GETDATE()-21,12,3,0),
('Lunchbox','Plastic lunchbox', GETDATE()-22,3,9,0),
('Beanie','Black beanie', GETDATE()-23,7,2,0),
('Ring','Silver ring', GETDATE()-24,21,8,0),
('Scarf','Grey scarf', GETDATE()-25,8,2,0),
('Gloves','Winter gloves', GETDATE()-26,9,2,0),
('Laptop','HP laptop', GETDATE()-27,6,1,0),
('Camera','Canon camera', GETDATE()-28,19,1,0),
('Project Folder','Science project', GETDATE()-29,17,6,0),
('Sports Bag','Blue sports bag', GETDATE()-30,2,7,0),
('Hat','School hat', GETDATE()-31,7,2,0),
('Sunglasses','RayBan', GETDATE()-32,3,4,0),
('Phone Charger','Apple charger', GETDATE()-33,1,1,0),
('Binder','Large binder', GETDATE()-34,13,3,0),
('Textbook','Physics book', GETDATE()-35,17,6,0),
('Flash Drive','64GB USB', GETDATE()-36,6,1,0),
('Earbuds','Samsung earbuds', GETDATE()-37,4,1,0),
('Keycard','Student ID', GETDATE()-38,8,4,0),
('Gym Towel','White towel', GETDATE()-39,2,2,0),
('Water Bottle','Blue bottle', GETDATE()-40,7,1,0),
('Mouse','Logitech mouse', GETDATE()-41,6,1,0),
('Keyboard','Mechanical keyboard', GETDATE()-42,6,1,0),
('Notebook 2','Red notebook', GETDATE()-43,14,3,0),
('Drawing Tablet','Wacom tablet', GETDATE()-44,6,1,0),
('Portable Speaker','JBL speaker', GETDATE()-45,16,1,0),
('Flashlight','Small torch', GETDATE()-46,18,10,0),
('Helmet','Bike helmet', GETDATE()-47,22,5,0),
('Planner','Student planner', GETDATE()-48,15,3,0),
('Fitness Band','Fitbit band', GETDATE()-49,2,1,0),
('Gaming Controller','Xbox controller', GETDATE()-50,20,1,0);

-- =======================
-- 5️⃣ Claims (10 sample)
-- =======================
INSERT INTO Claim (UserID, ItemName, Description, DateLost, DateSubmitted, Status, MatchedLostItemID) VALUES
('11111111-1111-1111-1111-111111111111','Blue Water Bottle','Metal bottle', GETDATE()-3, GETDATE(), 0, 1),
('22222222-2222-2222-2222-222222222222','Black Backpack','Nike backpack', GETDATE()-4, GETDATE(), 0, 2),
('33333333-3333-3333-3333-333333333333','iPhone 12','Black case', GETDATE()-5, GETDATE(), 0, 3),
('44444444-4444-4444-4444-444444444444','AirPods','White case', GETDATE()-6, GETDATE(), 0, 4),
('55555555-5555-5555-5555-555555555555','Math Book','Year 12 calculus', GETDATE()-7, GETDATE(), 0, 5),
('11111111-1111-1111-1111-111111111111','Laptop Charger','Dell charger', GETDATE()-8, GETDATE(), 0, 6),
('22222222-2222-2222-2222-222222222222','Sports Shoes','Adidas runners', GETDATE()-9, GETDATE(), 0, 7),
('33333333-3333-3333-3333-333333333333','Wallet','Brown leather', GETDATE()-10, GETDATE(), 0, 8),
('44444444-4444-4444-4444-444444444444','Keys','House keys with red tag', GETDATE()-11, GETDATE(), 0, 9),
('55555555-5555-5555-5555-555555555555','Glasses','Black frame', GETDATE()-12, GETDATE(), 0, 10);