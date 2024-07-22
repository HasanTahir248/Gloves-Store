create database GlovesSystem;
use GlovesSystem;
create table Admin (AdminID int primary key identity(1,1), Admin_Name varchar(25), Login_Email varchar(40), Password varchar(40));
insert into Admin values ('Sarim Shaikh','sheikhsarim52@gmail.com','77141714');
insert into Admin values ('Syeda Anmol','anmolzehra05@gmail.com','0900786');
select * from Admin;

create table Categories (CategoryID int primary key identity(1,1), CategoryName varchar(30), CategoryDescription varchar(500));

create table Products (ProductID int primary key identity(1,1), ArticleNo varchar(50),ProductName varchar(50), ProductDescription varchar(500),
ProductPrice varchar(50), CategoryID int foreign key references Categories(CategoryID),ImageURL varchar(100));

--drop table Products;
select * from Categories;
select * from Products;
