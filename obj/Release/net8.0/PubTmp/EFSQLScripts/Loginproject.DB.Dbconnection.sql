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
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240528132256_createaccount2'
)
BEGIN
    CREATE TABLE [Users] (
        [Id] int NOT NULL IDENTITY,
        [UserName] nvarchar(max) NOT NULL,
        [Password] nvarchar(max) NOT NULL,
        [Employeeid] nvarchar(max) NOT NULL,
        [Salesseries] nvarchar(max) NOT NULL,
        [Salesretseries] nvarchar(max) NOT NULL,
        [Counter] int NOT NULL,
        [Location] nvarchar(max) NOT NULL,
        [Branch] nvarchar(max) NOT NULL,
        [Isdayend] nvarchar(max) NOT NULL,
        [Active] nvarchar(max) NOT NULL,
        [Discount] nvarchar(max) NOT NULL,
        [Setmaster] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240528132256_createaccount2'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240528132256_createaccount2', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240606065133_createpaymod1'
)
BEGIN
    CREATE TABLE [paymods] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Post_ledger] nvarchar(max) NOT NULL,
        [Post_Ledger_id] int NOT NULL,
        [Company_id] int NOT NULL,
        CONSTRAINT [PK_paymods] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240606065133_createpaymod1'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240606065133_createpaymod1', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240606070354_createpaymod4'
)
BEGIN
    DROP TABLE [paymods];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240606070354_createpaymod4'
)
BEGIN
    CREATE TABLE [PAYMODT] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Post_ledger] nvarchar(max) NOT NULL,
        [Post_Ledger_id] int NOT NULL,
        [Company_id] int NOT NULL,
        CONSTRAINT [PK_PAYMODT] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240606070354_createpaymod4'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240606070354_createpaymod4', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240607052850_createplay2'
)
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PAYMODT]') AND [c].[name] = N'Id');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [PAYMODT] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [PAYMODT] ALTER COLUMN [Id] bigint NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240607052850_createplay2'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240607052850_createplay2', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240607054014_createplay69'
)
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PAYMODT]') AND [c].[name] = N'Post_Ledger_id');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [PAYMODT] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [PAYMODT] ALTER COLUMN [Post_Ledger_id] bigint NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240607054014_createplay69'
)
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PAYMODT]') AND [c].[name] = N'Company_id');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [PAYMODT] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [PAYMODT] ALTER COLUMN [Company_id] bigint NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240607054014_createplay69'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240607054014_createplay69', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240607062748_createplay58'
)
BEGIN
    CREATE TABLE [tblSalesdetails] (
        [Id] bigint NOT NULL IDENTITY,
        [sno] nvarchar(max) NOT NULL,
        [invdate] datetime2 NOT NULL,
        [netamount] int NOT NULL,
        [cust_id] bigint NOT NULL,
        [Salesman] int NOT NULL,
        [paymode] int NOT NULL,
        [custname] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_tblSalesdetails] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240607062748_createplay58'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240607062748_createplay58', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240607065804_createneHBCG'
)
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[tblSalesdetails]') AND [c].[name] = N'netamount');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [tblSalesdetails] DROP CONSTRAINT [' + @var3 + '];');
    ALTER TABLE [tblSalesdetails] ALTER COLUMN [netamount] decimal(18,2) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240607065804_createneHBCG'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240607065804_createneHBCG', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240607080207_createnewsale1'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240607080207_createnewsale1', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240611063934_createpo'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240611063934_createpo', N'8.0.8');
END;
GO

COMMIT;
GO

