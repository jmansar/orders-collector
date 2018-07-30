alter table Users
add IsAdmin integer not null constraint DF_Users_IsAdmin default(0);

alter table Users
add IsActive integer not null constraint DF_Users_IsActive default(1);

alter table Users
add CanBeOperator integer not null constraint DF_Users_CanBeOperator default(1);

alter table Orders
add Operator_Id integer null constraint FK_Orders_OperatorId_Users references Users(Id);

create table TaskDefs
(
	Id integer not null primary key autoincrement,
	Name text not null,
	Description text not null,
	OperatorSelectionPool text not null
);

create table Tasks
(
	Id integer not null primary key autoincrement,
	TaskDef_Id integer not null constraint FK_Tasks_TaskDefId_TaskDefs references TaskDefs(Id),
	TaskDate datetime not null,
	Operator_Id integer null constraint FK_Tasks_OperatorId_Users references Users(Id)
);

create table TaskDefsSuppliers
(
	TaskDef_Id integer not null constraint FK_TaskDefsSuppliers_TaskDefId_TaskDefs references TaskDefs(Id),
	Supplier_Id integer not null constraint FK_TaskDefsSuppliers_Supplier_Id_Suppliers references Suppliers(Id)
);