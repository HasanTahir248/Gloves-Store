create database GlovesSystem;
use GlovesSystemDB;
create table Admin (AdminID int primary key identity(1,1), Admin_Name varchar(25), Login_Email varchar(40), Password varchar(40));
insert into Admin values ('Sarim Shaikh','sheikhsarim52@gmail.com','77141714');
insert into Admin values ('Syeda Anmol','anmolzehra05@gmail.com','0900786');
insert into Admin values ('Hassan Tahir','hasantahir248@gmail.com','7539510','03295876730');
select * from Admin;

create table Categories (CategoryID int primary key identity(1,1), CategoryName varchar(30), CategoryDescription varchar(500));
drop table Categories;
alter table Categories add CategoryImage varchar(max);
insert into Categories values ('Winter','Winter Gloves','~/Images/10.png.jpg');
insert into Categories values ('Leather','Simple Gloves', '~/Images/10.png.jpg');
insert into Categories values ('Leather','Simple Gloves', '~\Images\brown_gloves.jpg');
insert into Products values ('IJ124', 'hand Gloves', 'Winter Hand Gloves','1000', 2, '~/Images/10.png');

create table Products (ProductID int primary key identity(1,1), ArticleNo varchar(50),ProductName varchar(50), ProductDescription varchar(500),
ProductPrice varchar(50), CategoryID int foreign key references Categories(CategoryID),ImageURL varchar(100));


create table Contact(ContactID int primary key identity(1,1), ContactName varchar(50), PhoneNumber varchar(15), ContactEmail varchar(50), ContactMessage varchar(500));

create table Shipping(ShippingID int primary key identity(1,1), FirstName varchar(30), LastName varchar(30), ShippingAddress varchar(500), Country Varchar(30), ZipCode varchar(30), City varchar(30), State varchar(30), Email varchar(50), PhoneNumber varchar(20));

create table Payment(PaymentID int primary key identity(1,1), CardNumber Varchar(50), OwnerName varchar(50), ExpiryDate varchar(50), CVV varchar(10));

select * from Contact;
select * from Admin;
select * from Categories;
select * from Products;
select * from Shipping;
select * from Payment;


CREATE PROCEDURE Insert_CONTACT_VALUES
    @NAME VARCHAR(50),
    @PHONE VARCHAR(50),
    @EMAIL VARCHAR(50),
    @MESSAGE VARCHAR(2)
AS
BEGIN
    INSERT INTO Contact VALUES (@NAME, @PHONE, @EMAIL, @MESSAGE)
END;


CREATE TABLE Reviews (
    ReviewID INT PRIMARY KEY IDENTITY(1,1),
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    Email VARCHAR(50),
    ContactNo VARCHAR(15),
    CategoryId INT,
    ProductId INT,
    ReviewText VARCHAR(1000),
    FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryID),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductID)
);
select * from Reviews;

---------------------------------------------------------------
CREATE TABLE Cart (
    CartID INT PRIMARY KEY IDENTITY(1,1),
    CustomerID int Foreign key references Customer(CustomerID),
);

create table CartProducts(
	ID int primary key identity(1,1),
	ProductID INT FOREIGN KEY REFERENCES Products(ProductID),
	ProductName varchar(100),
	ProductPrice int,
	ImageURL varchar(100),
	Quantity int,
    CartID int foreign key references Cart(CartID),
	CustomerID int foreign key references Customer(CustomerID)
);

drop table CartProducts;

select * from CartProducts;
select * from Cart;
delete from CartProducts where id = ;
insert into cartProducts values(1,'gloves',900,'~/Images/10.png',10,1,1);
insert into cartProducts values(1,'leathergloves',1500,'~/Images/10.png',12,1,1);
---------------------------------------------------------------
Create table Customer (
	CustomerID int primary key identity(1,1),
	UserName varchar(50),
	Email Varchar(50),
	PhoneNumber varchar(20),
	UserPassword varchar(50)
);

insert into Customer values('Hassan','hasan@gmail.com','03265876730','abc123');
select * from Customer;

select * from Categories;
select * from Products;

select p.ArticleNo, p.ProductName, p.ProductPrice, c.CategoryName, p.ImageURL from Products p join Categories c on c.CategoryID = p.ProductID;


alter view ProductsWithCategories
as 
select p.ProductID,p.ArticleNo, p.ProductName, p.ProductPrice, c.CategoryName, p.ImageURL from Products p join Categories c on c.CategoryID = p.ProductID;
select * from ProductsWithCategories;

select * from Reviews
ALTER proc DeleteProduct @id int
as
begin 
	DELETE FROM ORDERPRODUCTS WHERE ProductID = @id;

	Delete from Products where ProductID = @id ;
end;

alter proc DeleteCategory @id int
as
begin 
	DELETE FROM ORDERPRODUCTS WHERE ProductID = @id;
	Delete from Products where CategoryID = @id;
	Delete from Categories where CategoryID = @id;
end;


create table Orders(
OrderID int Primary key identity(1,1),
CartID int foreign key references Cart(CartID),
ShippingID int foreign key references Shipping(ShippingID),
PaymentID int foreign key references Payment(PaymentID),
OrderDate datetime);
drop table Orders



CREATE PROCEDURE AddCustomerAndCreateCart
    @UserName VARCHAR(100),
    @Email VARCHAR(100),
    @PhoneNumber VARCHAR(15),
    @UserPassword VARCHAR(100)
AS
BEGIN
    DECLARE @CustomerID INT;
    INSERT INTO Customer VALUES (@UserName, @Email, @PhoneNumber, @UserPassword);
    SET @CustomerID = SCOPE_IDENTITY();
    INSERT INTO Cart (CustomerID)
    VALUES (@CustomerID);


END;

select * from Customer;
select * from Cart;
select * from products;
select * from CartProducts;
insert into CartProducts Values (1,10,1,1);

alter proc CartItems @CustomerID int
as
begin 
	SELECT c.CartID, c.CustomerID, cp.ProductID, cp.ProductName, cp.ImageURL, cp.ProductPrice, cp.Quantity  
	FROM Cart c 
	JOIN CartProducts cp ON c.CartID = cp.CartID 
	where c.CustomerID = @CustomerID;
end;

select * from Orders
Drop procedure AddPaymentWithOrders;
CREATE PROCEDURE AddPaymentWithOrders
    @CardNumber VARCHAR(50),
    @Owner VARCHAR(100),
    @Expiry Varchar(50),
    @CVV VARCHAR(4),
    @CartID INT
AS
BEGIN
    DECLARE @LastShippingID INT;
    SELECT TOP 1 @LastShippingID = ShippingID FROM Shipping ORDER BY ShippingID DESC;
    INSERT INTO Payment VALUES (@CardNumber, @Owner, @Expiry, @CVV, @LastShippingID, @CartID);
	DECLARE @LastPaymentID INT;
    SELECT TOP 1 @LastPaymentID = PaymentID FROM Payment ORDER BY PaymentID DESC;
	Insert INTO Orders Values (@CartID, @LastShippingID, @LastPaymentID, CURRENT_TIMESTAMP);
	
END;
select * from Orders

EXEC AddPaymentWithOrders
    @CardNumber = '1234567812345678', 
    @Owner = 'John Doe', 
    @Expiry = '2026-12-31', 
    @CVV = '123', 
    @CartID = 1;
select * from Payment
select * from CartProducts;
drop proc AddPaymentWithLastShipping

select * from Shipping;
select * from Payment;
select * from orders;

INSERT INTO Shipping 
VALUES 
('John', 'Doe', '4B', 'USA', '123', 'NK', 'Illinois', 'john.doe@example.com', '123-456-7890', 1);


select * from Cart;
select * from CartProducts;
select * from Categories;
select * from Contact;
select * from Customer;
select * from Orders;
select * from Payment;
select * from Products;
select * from Reviews;
select * from Shipping;
select * from Customer;
Delete from Cart;
Delete from CartProducts
select *  from Customer

SELECT c.CartID, c.CustomerID, cp.ProductID, cp.ProductName, cp.ImageURL, cp.ProductPrice, cp.Quantity  
	FROM Cart c 
	JOIN CartProducts cp ON c.CartID = cp.CartID 
	where c.CustomerID = 4;

delete from Payment
delete from Orders
delete from Shipping
delete from Customer;
select * from Customer;

create proc InsertCartItems @prodId int, @prodQuantity int, @cartId int
as 
begin 
declare @prodName varchar(50), @prodPrice varchar(50), @imageUrl varchar(100);
select @prodName = ProductName, @prodPrice = ProductPrice, @imageUrl = ImageURL from Products where ProductID = @prodId;
insert into CartProducts Values (@prodId, @prodName, @prodPrice, @imageUrl, @prodQuantity, @cartId, @cartId);
end;

create proc emptyCustomerCartItems 
@custId int
as
begin
	delete from CartProducts where CustomerID = @custId;
end;

select * from OrderProducts;
create table OrderProducts (CartProducts);
SELECT * INTO OrderProducts FROM CartProducts WHERE 1 = 0;

drop proc InsertOrderItems
alter proc InsertOrderItems 
@cartId int,
@prodId int,
@prodName varchar(50),
@prodPrice int,
@imgUrl varchar(max),
@qty int
as
begin
	declare @orderId int;
	select @orderId = max(OrderID) from Orders where CartID = @cartId;
	insert into OrderProducts values (@prodId,@prodName,@prodPrice,@imgUrl,@qty,@cartId,@cartId, @orderId);
	delete from CartProducts where CartID = @cartId
end;
select * from CartProducts;
select * from orderProducts;
drop table OrderProducts
create table OrderProducts(
	ID int primary key identity(1,1),
	ProductID INT FOREIGN KEY REFERENCES Products(ProductID),
	ProductName varchar(100),
	ProductPrice int,
	ImageURL varchar(100),
	Quantity int,
    CartID int,
	CustomerID int,
	OrderID int foreign key references Orders(OrderID)
);

select * from Orders;
select * from Customer
select * from CartProducts
select * from OrderProducts
select * from Orders
select * from CartProducts;
select * from Shipping
create view OrdersData
as
select o.OrderID, s.FirstName, s.LastName, s.City, s.Country, o.OrderDate from Orders o join Shipping s on s.ShippingID = o.ShippingID;

select * from OrdersData

create proc DeleteCartProd @id int
as 
begin
	Delete from CartProducts where ProductID = @id;
end;

select max(OrderID) from Orders where CartID = 6;
select * from Orders;
select * from OrderProducts


create view AdminData
as
select AdminID, Admin_Name,Login_Email, Admin_Phone from Admin




-------------------------------------------------Triggers-----------------------------------------------------------
create table OrderAuditTrail(
audit_id int primary key identity(1,1), 
orderId int,
operation varchar(255), 
time datetime);
create trigger AUDIT_FOR_INSERT_ORDERS 
on orders
AFTER INSERT 
AS
BEGIN
	DECLARE @ORDERID INT, @OPERATION VARCHAR(255); 
	SET @OPERATION = 'Insert';
	SELECT @ORDERID = I.OrderID FROM INSERTED I;
	INSERT INTO OrderAuditTrail Values(@ORDERID,@OPERATION,CURRENT_TIMESTAMP);
END;
CREATE TRIGGER AUDIT_FOR_DELETE_ORDERS
ON orders 
AFTER DELETE 
AS
BEGIN
	DECLARE @ORDERID INT, @OPERATION VARCHAR(255); SET @OPERATION = 'Delete';
	SELECT @ORDERID = I.OrderID FROM DELETED I;
	INSERT INTO OrderAuditTrail Values(@ORDERID,@OPERATION,CURRENT_TIMESTAMP);
END;


create table AdminAuditTrail(
audit_id int primary key identity(1,1), 
AdminId int,
operation varchar(255), 
time datetime);
create trigger AUDIT_FOR_INSERT_ADMIN 
on Admin
AFTER INSERT 
AS
BEGIN
	DECLARE @ADMINID INT, @OPERATION VARCHAR(255); 
	SET @OPERATION = 'Insert';
	SELECT @ADMINID = I.AdminID FROM INSERTED I;
	INSERT INTO AdminAuditTrail Values(@ADMINID,@OPERATION,CURRENT_TIMESTAMP);
END;
CREATE TRIGGER AUDIT_FOR_DELETE_ADMIN
ON Admin 
AFTER DELETE 
AS
BEGIN
	DECLARE @ADMINID INT, @OPERATION VARCHAR(255); 
	SET @OPERATION = 'Delete';
	SELECT @ADMINID = I.AdminID FROM DELETED I;
	INSERT INTO AdminAuditTrail Values(@ADMINID,@OPERATION,CURRENT_TIMESTAMP);
END;

create table ProductAuditTrail(
audit_id int primary key identity(1,1), 
ProductId int,
operation varchar(255), 
time datetime);
create trigger AUDIT_FOR_INSERT_PRODUCTS
on Products
AFTER INSERT 
AS
BEGIN
	DECLARE @PRODUCTID INT, @OPERATION VARCHAR(255); 
	SET @OPERATION = 'Insert';
	SELECT @PRODUCTID = I.ProductID FROM INSERTED I;
	INSERT INTO ProductAuditTrail Values(@PRODUCTID,@OPERATION,CURRENT_TIMESTAMP);
END;
CREATE TRIGGER AUDIT_FOR_DELETE_PRODUCTS
ON Products 
AFTER DELETE 
AS
BEGIN
	DECLARE @PRODUCTID INT, @OPERATION VARCHAR(255); 
	SET @OPERATION = 'Delete';
	SELECT @PRODUCTID = I.ProductID FROM DELETED I;
	INSERT INTO ProductAuditTrail Values(@PRODUCTID,@OPERATION,CURRENT_TIMESTAMP);
END;

create table CategoryAuditTrail(
audit_id int primary key identity(1,1), 
CategoryId int,
operation varchar(255), 
time datetime);
create trigger AUDIT_FOR_INSERT_CATEGORY
on Categories
AFTER INSERT 
AS
BEGIN
	DECLARE @CATEGORYID INT, @OPERATION VARCHAR(255); 
	SET @OPERATION = 'Insert';
	SELECT @CATEGORYID = I.CategoryID FROM INSERTED I;
	INSERT INTO CategoryAuditTrail Values(@CATEGORYID,@OPERATION,CURRENT_TIMESTAMP);
END;
CREATE TRIGGER AUDIT_FOR_DELETE_CATEGORY
ON Categories 
AFTER DELETE 
AS
BEGIN
	DECLARE @CATEGORYID INT, @OPERATION VARCHAR(255); 
	SET @OPERATION = 'Delete';
	SELECT @CATEGORYID = I.CategoryID FROM DELETED I;
	INSERT INTO CategoryAuditTrail Values(@CATEGORYID,@OPERATION,CURRENT_TIMESTAMP);
END;

create table CustomerAuditTrail(
audit_id int primary key identity(1,1), 
CustomerId int,
operation varchar(255), 
time datetime);
create trigger AUDIT_FOR_INSERT_CUSTOMER
on Customer
AFTER INSERT 
AS
BEGIN
	DECLARE @CUSTOMERID INT, @OPERATION VARCHAR(255); 
	SET @OPERATION = 'Insert';
	SELECT @CUSTOMERID = I.CustomerID FROM INSERTED I;
	INSERT INTO CustomerAuditTrail Values(@CUSTOMERID,@OPERATION,CURRENT_TIMESTAMP);
END;

create table PaymentAuditTrail(
audit_id int primary key identity(1,1), 
PaymentId int,
operation varchar(255), 
time datetime);
create trigger AUDIT_FOR_INSERT_PAYMENT
on Payment
AFTER INSERT 
AS
BEGIN
	DECLARE @PAYMENTID INT, @OPERATION VARCHAR(255); 
	SET @OPERATION = 'Insert';
	SELECT @PAYMENTID = I.PaymentID FROM INSERTED I;
	INSERT INTO PaymentAuditTrail Values(@PAYMENTID,@OPERATION,CURRENT_TIMESTAMP);
END;
select * from Orders;
select * from OrderProducts
select * from Customer;
create view ReviewsView
as
select r.ReviewID, r.FirstName, r.LastName, r.CategoryId, p.ProductName, r.ReviewText from Reviews r join Products p on p.ProductID = r.ProductId ;
SELECT * FROM REVIEWS
selEct * from categories
select * from products
INSERT INTO REVIEWS VALUES ('SARIM', 'SHAIKH' , 'sheikhsarim52@gmail.com', '03302570995', 5, 7, 'The quality was very good of the product and it was shipped on time')
alter proc SelectOrderItems @orderID int
as 
begin
	select ProductID, ProductName, ProductPrice, Quantity from OrderProducts where OrderID = @orderID;
	end;
create proc DeleteAdmin @id int
as 
begin 
	delete from Admin where AdminID = @id;
end;
SELECT * FROM ORDERPRODUCTS

create proc DeleteReview @id int
as 
begin
Delete from Reviews where ReviewID = @id;
end;
select * from OrderProducts
