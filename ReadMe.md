# Cash Registry - Lab for APR0400

## Introduction

## Notes

## SQL

### Staff table:

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
