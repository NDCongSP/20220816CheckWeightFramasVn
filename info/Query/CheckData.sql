select TOP (100) * from tblScanData 
--where BarcodeString ='OCB3702,6111322101-N053-2504,36,7,P,9/9,162610,1/1|1,201476.2023,1,1,3'
--where IdLabel = '219829.2023'
--Where OcNo = 'OPRT10069' AND BoxNo ='1/1'
--where Station = 0
--where Id in ('451A8FA6-2435-4DE4-BB55-39EF146E954A','F904148E-2764-4055-B1CB-B40CA2C94056')
where DAY(CreatedDate) = DAY(GETDATE()) and  month(CreatedDate) = month(GETDATE())
order by CreatedDate desc;

select top(100)* from tblApprovedPrintLabel 
--where QRLabel ='OCB3702,6111322101-N053-2504,36,7,P,9/9,162610,1/1|1,201476.2023,1,1,3'
--where IdLabel = '169292.2023'
--Where OC = 'OPRT8594' AND BoxNo ='1/1'
where DAY(CreatedDate) = DAY(GETDATE()) and  month(CreatedDate) = month(GETDATE())
--where ScanDataId = 'E9A6D475-6294-413F-923C-37A0EF92692A'
order by CreatedDate desc;

--select * from tblMetalScanResult order by createddate  desc
--select * from tblScanDataReject order by createddate  desc

--delete tblScanData where Id	= '5B7AFCD0-CD11-4778-BB48-8C21DC7FCBD0'
--delete tblApprovedPrintLabel where Id = '5772C27A-4B33-42EB-BBB7-284C9B2E4E3E'

--update tblScanData set ActualDeviationPairs =0 where id ='ECA374BF-3BAF-42EC-8B53-53F77A530060'
--update tblScanData set Status=1 where id ='D29A25A7-3C19-4D92-B3A2-4FE672C744A3'
--update tblScanData set Actived=0 where id ='E9A6D475-6294-413F-923C-37A0EF92692A'

--code approved: D7BEF08B-C830-4C67-9F2F-39D52AE178EE


--select * from tblWinlineProductsInfo  where ProductNumber ='6111010702-ADSN-2354' order by CreatedDate desc
--select * from tblCoreDataCodeItemSize order by CreatedDate desc
--select * from tblScanData order by CreatedDate desc



