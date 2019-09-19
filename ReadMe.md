# Cash Registry - Lab for APR0400

## Introduction

## Notes

## SQL

### Product table

To create the Product table this query was used:

```sql
CREATE TABLE [dbo].[Product]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[Name] VARCHAR(100) NOT NULL,
	[Price] FLOAT NOT NULL,
	[Category] VARCHAR(50) NOT NULL
)
```

### Staff table

To create the Staff table the following query was used:

```sql
CREATE TABLE [dbo].[Staff]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[FirstName] VARCHAR(50) NOT NULL,
	[LastName] VARCHAR(50) NOT NULL,
	[SocialSecurityNumber] VARCHAR(12) NOT NULL
)
```

### Transaction table

```sql
CREATE TABLE [dbo].[Transaction]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[TimeOfPurchase] DATETIME NOT NULL,
	[StaffMember] INT NOT NULL,
	[PaymentMethod] VARCHAR(50) NOT NULL,
	CONSTRAINT [FK_Transaction_Staff] FOREIGN KEY ([StaffMember]) REFERENCES [Staff]([Id])
)
```

Here we are using the `StaffMember` as a foregin key to be able to see what person in the staff was handling the purchase.

### TransactionComponent table

```sql
CREATE TABLE [dbo].[TransactionComponent] (
    [Id]              INT          IDENTITY (1, 1) NOT NULL,
    [Quantity]        FLOAT (53)   NOT NULL,
    [TransactionId]   INT          NOT NULL,
    [ProductName]     VARCHAR (50) NOT NULL,
    [ProductPrice]    FLOAT (53)   NOT NULL,
    [ProductCategory] VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TransactionComponent_Transaction] FOREIGN KEY ([TransactionId]) REFERENCES [dbo].[Transaction] ([Id])
);
```

This table is almost a duplicate of the `Product` table. This is because we don't want to link to the actual product here because if the linked
product for instance get an updated price all the historical transactions with the product within them will get updated and will kill all serious
attempts of recordkeeping.
