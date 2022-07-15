IF NOT EXISTS( SELECT		*
					FROM		[sys].[schemas] [s]
					INNER JOIN	[sys].[tables] [t] ON [s].[schema_id] = [t].[schema_id]
					INNER JOIN	[sys].[columns] [c] ON [t].[object_id] = [c].[object_id]
					WHERE		[s].[name] = 'dbo'
					AND			[t].[name] = 'Store'
					AND			[c].[name] = 'RetailChannelId')
BEGIN
		PRINT 'RetailChannelId Added in Store table successfully';
		ALTER TABLE [dbo].[Store]
		ADD [RetailChannelId] NVARCHAR(15) NULL
END
ELSE
BEGIN
PRINT 'RetailChannelId already added in Store table';
END
GO

IF EXISTS( SELECT		*
					FROM		[sys].[schemas] [s]
					INNER JOIN	[sys].[tables] [t] ON [s].[schema_id] = [t].[schema_id]
					INNER JOIN	[sys].[columns] [c] ON [t].[object_id] = [c].[object_id]
					WHERE		[s].[name] = 'dbo'
					AND			[t].[name] = 'Store'
					AND			[c].[name] = 'RetailChannelId')
BEGIN
Create table #tempStore
 (
Name nvarchar (100) NOT NULL primary key,
ChannelId nvarchar(15) Not NULL
)
insert into #tempStore values ('Netherlands','000046')
insert into #tempStore values ('Belgium','000047')
insert into #tempStore values ('Albania','000057')
insert into #tempStore values ('Bulgaria','000058')
insert into #tempStore values ('Estonia','000059')
insert into #tempStore values ('Malta','000060')
insert into #tempStore values ('Portugal','000061')
insert into #tempStore values ('Poland','000072')
insert into #tempStore values ('Dominican Republic','000073')
insert into #tempStore values ('Ecuador','000075')
insert into #tempStore values ('Panama','000077')
insert into #tempStore values ('Puerto Rico','000079')
insert into #tempStore values ('Uruguay','000091')
insert into #tempStore values ('Costa Rica','000093')
insert into #tempStore values ('South Korea','000095')
insert into #tempStore values ('Japan','000097')
insert into #tempStore values ('Germany','000102')
insert into #tempStore values ('Switzerland','000104')
insert into #tempStore values ('Austria','000106')
insert into #tempStore values ('United States of America','000122')
insert into #tempStore values ('APPLE Store','000123')
insert into #tempStore values ('Ireland','000132')
insert into #tempStore values ('China','000141')
insert into #tempStore values ('Chile','000151')
insert into #tempStore values ('Colombia','000153')
insert into #tempStore values ('Mexico','000155')
insert into #tempStore values ('Canada','000160')
insert into #tempStore values ('India','000162')
insert into #tempStore values ('Bangladesh','000164')
insert into #tempStore values ('Maldives','000166')
insert into #tempStore values ('Srilanka','000168')
insert into #tempStore values ('Nepal','000170')
insert into #tempStore values ('Myanmar','000172')
insert into #tempStore values ('Bhutan','000175')
insert into #tempStore values ('Brazil','000177')
insert into #tempStore values ('Argentina','000186')
insert into #tempStore values ('France','000192')
insert into #tempStore values ('Luxembourg','000194')
insert into #tempStore values ('Italy','000199')
insert into #tempStore values ('Spain','000202')
insert into #tempStore values ('Norway','000203')
insert into #tempStore values ('Hungary','000204')
insert into #tempStore values ('Australia','000209')
insert into #tempStore values ('Greece','000211')
insert into #tempStore values ('Finland','000212')
insert into #tempStore values ('Sweden','000216')
insert into #tempStore values ('Cyprus','000221')
insert into #tempStore values ('Czech Republic','000222')
insert into #tempStore values ('Turkey','000223')
insert into #tempStore values ('Slovakia','000224')
insert into #tempStore values ('Slovenia','000225')
insert into #tempStore values ('Lithuania','000232')
insert into #tempStore values ('Latvia','000233')
insert into #tempStore values ('Russia','000234')
insert into #tempStore values ('Malaysia','000239')
insert into #tempStore values ('New Zealand','000240')
insert into #tempStore values ('Philippines','000241')
insert into #tempStore values ('Singapore','000242')
insert into #tempStore values ('Indonesia','000248')
insert into #tempStore values ('Taiwan','000249')
insert into #tempStore values ('Thailand','000250')
insert into #tempStore values ('Vietnam','000251')
insert into #tempStore values ('Croatia','000255')
insert into #tempStore values ('Iceland','000261')
insert into #tempStore values ('Israel','000262')
insert into #tempStore values ('Hong Kong','000266')
insert into #tempStore values ('GBP EN group','000268')
insert into #tempStore values ('CNY CN group','000270')
insert into #tempStore values ('CHF group','000277')
insert into #tempStore values ('DEF EUR EN Group','000271')
insert into #tempStore values ('AFRICA USD EN','000274')
insert into #tempStore values ('AFRICA EUR EN','000275')
insert into #tempStore values ('APAC USD EN','000282')
insert into #tempStore values ('LATAM USD EN group','000285')
insert into #tempStore values ('INR USD EN Group','000294')
insert into #tempStore values ('United Kingdom','000322')
select * into #tempClStore
from Store

declare @StoreName nvarchar(100)
declare @RetailChannelId nvarchar(15)

while exists (select * from #tempClStore)
Begin
select top 1 @StoreName = Name 
from #tempClStore
IF exists (select * from #tempStore where Name like '%'+@StoreName+'%')   
BEGIN
select  @RetailChannelId = ChannelId from #tempStore where Name like '%'+@StoreName+'%'
update Store
Set RetailChannelId  = @RetailChannelId
where [Name] = @StoreName

PRINT 'RetailChannelId :'+@RetailChannelId + ' added against store '+  @StoreName
END
ELSE
BEGIN
PRINT 'Channel not found in AX : '+@StoreName 
END

delete from #tempClStore where [Name] = @StoreName
END

drop table #tempStore
drop table #tempClStore
END







