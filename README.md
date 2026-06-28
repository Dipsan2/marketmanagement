# market Management System

## Overview

This project is about the market management system for a small shop.
this is a C# .NET console application that allows users to manage products, suppliers, stock levels and sales.
 I used SQL Server to store the data also entity framework Core handles the database operations and
custom Binary Search Tree also here to make product searching more efficient.


## Features

The system currently includes:

 Add, edit and delete products
 Search products by barcode
 Search products by product name
 Register suppliers
 Shopping cart and checkout
 Record sales transactions
 Update stock quantities
 Low stock alerts
 View inventory reports
 SQL Server database integration using Entity Framework Core

## Technologies Used

C#
.NET 8
SQL Server
Entity Framework Core
Visual Studio 2022


## Project Structure

```
marketmanagement
│
├── models
├── datastructures
├── data
├── Migrations
├── SQL
│   ├── DatabaseScript.sql
│   └── SeedData.sql
└── Program.cs
```

## Database Setup

1. Create a SQL Server database.
2. Update the connection string in `data/marketdbcontext.cs`.
3. Apply the Entity Framework migration using

```
Update-Database
```

The SQL folder also contains:

DatabaseScript.sql – creates the database tables.
SeedData.sql – inserts sample data for testing.


## Running the Application

1. Open the solution in Visual Studio.
2. Build the project.
3. Run the application.

The console menu allows you to:

 Scan product barcodes
 Search for products
 Add, update or delete products
 Register suppliers
 Restock products
 View reports
 Process customer purchases


## Validation

it also performs validation checks like:

 Preventing duplicate barcodes
 Checking that required fields are entered
 Ensuring prices are greater than zero
 Preventing negative stock quantities
 

## Search Algorithm

Products are stored in a custom Binary Search Tree.

The Binary Search Tree is used to speed up product searches by barcode.
Whenever a product is added updated or removed the tree is updated so it stays with the SQL Server database.


## Author

Dipsan Dhakal

CST2550 Coursework
