alter table OrderItems
add AuditInfo_SubSubSource text null;

alter table OrderItems
add AuditInfo_Date datetime null;

alter table Suppliers
add Phone text null;