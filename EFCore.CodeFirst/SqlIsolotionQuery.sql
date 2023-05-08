SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
BEGIN TRANSACTION MyTransaction
begin try
select * from Products
update Products set Price = 50 where Id = 1
Commit TRANSACTION MyTransaction
PRINT 'Transaction başarılı'
end try
begin catch
ROLLBACK TRANSACTION MyTransaction
PRINT 'Transaction Fail'
end catch