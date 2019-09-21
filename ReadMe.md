# Cash Registry - Lab for APR0400

## Introduction

For this assignment I chose to do the cash registry for a grocery store.

There's 3 linked tables with full crud for two of them and only CRD for the `Transaction` table. This is because of the security issue of letting people change the transactions that already has been booked.

There is some redundancy between the `Product` and `TransactionComponent` table. This is due to the fact that we don't want all the historical transactions to be modified whenever we update the price of a Product.

## Notes

I think that I'm justified to a C. See explanation below.

### D

_"For this grade you need to create 3 tables. The tables should have a 1:M relationship. "_

I have the `Staff`, `Transaction` and `TransactionComponent` tables that are linked 1:N. All with CRUD except `Transaction`, see why above.

_"Make a menu for selecting which operation to use._" - See the `Menu` class and some inside the Handlers.

### C

_"To obtain a C you
follow the instructions for grade D and add
CRUD operations for two of the tables._"
This can be seen in `Staff`, `Transaction` and `Product` although products aren't linked to any of the other tables with foregin keys.

### B

_"For this grade you include an M:M relation in your data model. You hould ave CRUD operations for the tables in the M:M relation. "_
Did not implement this because it felt forced and not really logical. But if I would have implemented it I would have made a new table with id's from the tables i wanted to link as foreign keys. Then to implement it in the code it's just to create the new objects and save the changes to db.

### A

The only extra stuff that I implemented is some Linq magic in the fetching of transactions that I tried out just for fun.

## SQL

### Data model

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
