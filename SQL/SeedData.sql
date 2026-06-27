
-- ==========================================
-- Sample Seed Data
-- Local Supermarket Management System
-- ==========================================

-- Category
INSERT INTO categories (name)
VALUES
('General');

-- Supplier
INSERT INTO suppliers (name, contactinfo)
VALUES
('Default Supplier', '07771697384');

-- Products
INSERT INTO products
(title, barcode, price, quantityinstock, expiryorrestockdate, categoryid, supplierid)
VALUES
('Bread', '111111', 1.45, 300, '2026-07-06', 1, 1),
('Ice Cream', '222222', 4.99, 120, '2026-07-06', 1, 1),
('Ice', '333333', 0.50, 150, '2026-07-09', 1, 1),
('Milk 2L', '50123456', 2.50, 45, '2026-06-29', 1, 1);

-- Stock
INSERT INTO stocks (productid, lowstockthreshold, status)
VALUES
(1, 50, 'Available'),
(2, 25, 'Available'),
(3, 20, 'Available'),
(4, 50, 'Low Stock');

-- Sales
INSERT INTO sales (Transsactiondate, totalamount)
VALUES
('2026-06-25 12:00:00', 0.50),
('2026-06-25 12:31:00', 70.00),
('2026-06-26 13:18:00', 4.35);

-- Sale Items
INSERT INTO saleitems (saleid, productid, quantity, unitprice)
VALUES
(1, 3, 1, 0.50),
(2, 2, 14, 5.00),
(3, 1, 3, 1.45);

