create table IncrementalUpdates
(
	Id integer not null primary key autoincrement,
	Name text not null
);

create table Settings
(
	Id integer not null primary key autoincrement,
	Key text not null,
	Value text not null
);

create table Suppliers
(
	Id integer not null primary key autoincrement,
	Name text not null
);

create table SupplierAliases
(
	Id integer not null primary key autoincrement,
	Name text not null,
	Supplier_Id integer not null constraint FK_SupplierAlias_SuppierId_Suppliers references Suppliers(Id)
);



create table Orders

(

	Id integer not null primary key autoincrement,

	Supplier_Id integer not null constraint FK_SupplierAlias_SuppierId_Suppliers references Suppliers(Id),

	OrderDate date not null

);



create table Users

(

	Id integer not null primary key autoincrement,

	Name text not null,

	FullName text not null

);



create table OrderItems

(

	Id integer not null primary key autoincrement,

	Content text not null,

	Order_Id integer not null constraint FK_OrderItems_OrderId_Orders references Orders(Id),

	User_Id integer not null constraint FK_OrderItems_UserId_Users references Users(Id),

	AuditInfo_Source text null,

	AuditInfo_SubSource text null,

	AuditInfo_InvokedBy text null

);