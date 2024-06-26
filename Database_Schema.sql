-- Create the database

-- DROP DATABASE EmpowerIDDB;
CREATE DATABASE EmpowerIDDB;
GO

-- Use the newly created database
USE EmpowerIDDB;
GO

-- Create table Categories
CREATE TABLE Categories(
	category_id INT PRIMARY KEY IDENTITY(1,1),
	category_name NVARCHAR(250) NOT NULL
);

CREATE INDEX IDX_category_name ON Categories(category_name);
GO
-- Create table Products
CREATE TABLE Products (
    product_id INT PRIMARY KEY IDENTITY(1,1),
    product_name NVARCHAR(250) NOT NULL,
    price DECIMAL NOT NULL,
	[description] NVARCHAR (1000),
	image_url NVARCHAR(250),
	date_added DATETIME DEFAULT GETDATE(),
	category_id INT NOT NULL,
	date_updated DATETIME DEFAULT GETDATE(),
	CONSTRAINT FK_Category_Product FOREIGN KEY (category_id) REFERENCES Categories(category_id),
);

CREATE INDEX IDX_product_name ON Products(product_name);
CREATE INDEX IDX_product_price ON Products(price);
CREATE INDEX IDX_product_description ON Products([description]);
CREATE INDEX IDX_product_date ON Products(date_added);
CREATE INDEX IDX_product_category ON Products(category_id);

-- Create table Orders
CREATE TABLE Orders(
	order_id INT PRIMARY KEY IDENTITY(1,1),
	order_date DATETIME DEFAULT GETDATE(),
	customer_name NVARCHAR (250)
);

CREATE INDEX IDX_order_date ON Orders(order_date);
CREATE INDEX IDX_order_customer ON Orders(customer_name);


-- Create table to handle many-to-many relation between product and orders
CREATE TABLE OrderItems(
	order_item_id INT PRIMARY KEY IDENTITY(1,1),
	order_id INT NOT NULL,
	product_id INT NOT NULL

	CONSTRAINT FK_Order_OrderItem FOREIGN KEY (order_id) REFERENCES Orders(order_id),
	CONSTRAINT FK_Product_OrderItem FOREIGN KEY (product_id) REFERENCES Products(product_id)
);

-- Create store procedure to search products
GO
CREATE PROCEDURE SearchProducts
    @productName NVARCHAR(250) = NULL,
    @categoryId INT = NULL,
    @minPrice DECIMAL = NULL,
    @maxPrice DECIMAL = NULL,
    @description NVARCHAR(1000) = NULL,
    @dateAddedStart DATETIME = NULL,
    @dateAddedEnd DATETIME = NULL,
	@pageSize INT = NULL,
    @pageNumber INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @startRowIndex INT, @endRowIndex INT;
    SET @startRowIndex = (@pageNumber - 1) * @pageSize + 1;
    SET @endRowIndex = @pageNumber * @pageSize;

    WITH OrderedProducts AS (
        SELECT *,
               ROW_NUMBER() OVER (ORDER BY product_id) AS RowNum
        FROM Products
        WHERE
            (@productName IS NULL OR product_name LIKE '%' + @productName + '%')
            AND (@categoryId IS NULL OR category_id = @categoryId)
            AND (@minPrice IS NULL OR price >= @minPrice)
            AND (@maxPrice IS NULL OR price <= @maxPrice)
            AND (@description IS NULL OR [description] LIKE '%' + @description + '%')
            AND (@dateAddedStart IS NULL OR date_added >= @dateAddedStart)
            AND (@dateAddedEnd IS NULL OR date_added <= @dateAddedEnd)
    )
    SELECT *
    FROM OrderedProducts
    WHERE RowNum BETWEEN @startRowIndex AND @endRowIndex;
END;
GO

-- Create View , join products with category and create view for azure ai search
CREATE VIEW vw_ProductsWithCategory AS
SELECT p.product_id, 
	   p.product_name,
	   CAST(p.price AS FLOAT) AS price,
	   p.[description],
	   p.image_url,
	   p.date_added,
	   p.date_updated,
	   c.category_id,
	   c.category_name

FROM Products p
JOIN Categories c ON p.category_id = c.category_id;
GO

-- create trigger to update date_updated column 
CREATE TRIGGER trg_UpdateLastUpdate
ON Products
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Products
    SET date_updated = GETDATE()
    FROM Products p
    INNER JOIN Inserted i ON p.product_id = i.product_id;
END;
GO

-- Create SP to upsert category
CREATE PROCEDURE usp_UpsertCategory 
@CategoryName NVARCHAR(100),
@CategoryId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Id INT;

    SELECT @Id = category_id
    FROM Categories
    WHERE category_name = @CategoryName;

    IF @Id IS NULL
    BEGIN
        INSERT INTO Categories (category_name)
        VALUES (@CategoryName);

        SET @CategoryId = SCOPE_IDENTITY();
    END
    ELSE
    BEGIN
        SET @CategoryId = @Id;
    END
END

go

-- Create SP to Insert products
CREATE PROCEDURE usp_UpsertProduct
@ProductName NVARCHAR(100),
@RetailPrice DECIMAL(18, 2),
@CategoryId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Products WHERE product_name = @ProductName)
    BEGIN
        -- Update the existing product
        UPDATE Products
        SET price = @RetailPrice,
            category_id = @CategoryId
        WHERE product_name = @ProductName;
    END
    ELSE
    BEGIN
        -- Insert a new product
        INSERT INTO Products (
            product_name,
            price,
            category_id
        )
        VALUES (
            @ProductName,
            @RetailPrice,
            @CategoryId
        );
    END
END

-- enable CDC in database

EXEC sys.sp_cdc_enable_db;
GO

EXEC sys.sp_cdc_enable_table
    @source_schema = 'dbo',
    @source_name = N'products',
    @role_name = NULL,
    @supports_net_changes = 1
GO
