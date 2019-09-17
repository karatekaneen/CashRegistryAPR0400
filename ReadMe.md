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

Here we are using the StaffMember as a foregin key to be able to see what person in the staff was handling the purchase.

### TransactionComponent table

```sql
CREATE TABLE [dbo].[TransactionComponent]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[Product] INT NOT NULL,
	[Quantity] FLOAT NOT NULL,
	[Transaction] INT NOT NULL,
	CONSTRAINT [FK_TransactionComponent_Product] FOREIGN KEY ([Product]) REFERENCES [Product]([Id]),
	CONSTRAINT [FK_TransactionComponent_Transaction] FOREIGN KEY ([Transaction]) REFERENCES [Transaction]([Id])
)
```

In this table we are using two foreign keys. One for adding the product (and to be able to multiply the price with the quantity) and one to link to the transaction to later on be able to get all the products linked to one Transaction.
