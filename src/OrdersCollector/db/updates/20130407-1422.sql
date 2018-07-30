create table OpenIdAccounts
(
	Id integer not null primary key autoincrement,
	User_Id integer not null constraint FK_OpenIdAccounts_UserId_Users references Users(Id),
	IsActive integer not null,
	CreationDate datetime not null,
	ActivationToken text null,
	Identifier text not null
);
