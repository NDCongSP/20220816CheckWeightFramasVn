USE [IDCScaleSystem]

select * from tblScanData order by CreatedDate desc

select * from tblScanData where IdLable = '46233.2022'  order by CreatedDate desc
select * from tblScanData where ProductNumber like '6112012228-%' and OcNo like 'PRT%'  order by CreatedDate desc

select * from tblItemMissingInfo order by CreatedDate desc

DECLARE	@return_value int

EXEC	@return_value = [dbo].[sp_vProductItemInfoGet]
		@ProductNumber = N'6114012218-2273-2652',
		@SpecialCase = 0

GO

select * from tblApprovedPrintLabel order by CreatedDate desc
select * from tblScanData where BarcodeString = 'LAB8552,6117012202-1807-D228,16,1,P,1/1,170000,1/1|1,,,,' order by CreatedDate desc



select * from tblWinlineProductsInfo where ProductNumber = '7112321903-NAKD-0041'

update tblScanData set Status = 1 where IdLable ='46233.2022'-- where Id ='57CB7FC1-4826-4243-8127-B1D971A6E471'

update tblScanData set Status = 1 
where ProductNumber like '6112012228-%'
		and OcNo like 'PRT%'
		and Station = 0

--delete tblScanData where id ='DAA91A10-7FBB-4E64-85EA-3C0F4706CF99'
--select * from tblCoreDataCodeItemSize where CodeItemSize in ('6116322201-N129-0041','6116322201-N129-0043','6116322201-N129-0013','6116322201-N129-0044','6116322201-N129-0042') order by CodeItemSize asc
--select * from tblCoreDataCodeItemSize where CodeItemSize in ('6116322201-*-0041','6116322201-*-0043','6116322201-*-0013','6116322201-*-0044','6116322201-*-0042') order by CodeItemSize asc


select * from tblCoreDataCodeItemSize where CodeItemSize in ('7112042007-P023-2402')
select * from tblCoreDataCodeItemSize where CodeItemSize in ('7112042007-*-2402')

--delete tblCoreDataCodeItemSize