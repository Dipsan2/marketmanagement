IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [categories] (
    [id] int NOT NULL IDENTITY,
    [name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_categories] PRIMARY KEY ([id])
);

CREATE TABLE [products] (
    [id] int NOT NULL IDENTITY,
    [title] nvarchar(max) NOT NULL,
    [barcode] nvarchar(max) NOT NULL,
    [price] decimal(18,2) NOT NULL,
    [quantityinstock] int NOT NULL,
    [expiryorrestockdate] datetime2 NOT NULL,
    [categoryid] int NOT NULL,
    [supplierid] int NOT NULL,
    CONSTRAINT [PK_products] PRIMARY KEY ([id])
);

CREATE TABLE [saleitems] (
    [id] int NOT NULL IDENTITY,
    [saleid] int NOT NULL,
    [productid] int NOT NULL,
    [quantity] int NOT NULL,
    [unitprice] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_saleitems] PRIMARY KEY ([id])
);

CREATE TABLE [sales] (
    [Id] int NOT NULL IDENTITY,
    [Transsactiondate] datetime2 NOT NULL,
    [totalamount] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_sales] PRIMARY KEY ([Id])
);

CREATE TABLE [stocks] (
    [id] int NOT NULL IDENTITY,
    [productid] int NOT NULL,
    [lowstockthreshold] int NOT NULL,
    [status] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_stocks] PRIMARY KEY ([id])
);

CREATE TABLE [suppliers] (
    [id] int NOT NULL IDENTITY,
    [name] nvarchar(max) NOT NULL,
    [contactinfo] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_suppliers] PRIMARY KEY ([id])
);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260622145120_initial_db_setup', N'10.0.9');

COMMIT;
GO

