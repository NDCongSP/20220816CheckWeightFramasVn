select TOP (100) * from tblScanData 
--where BarcodeString ='OCB3702,6111322101-N053-2504,36,7,P,9/9,162610,1/1|1,201476.2023,1,1,3'
--where IdLabel = '18724.2023'
--Where OcNo = 'OPRT8594' AND BoxNo ='1/1'
--where Station = 0
where DAY(CreatedDate) = DAY(GETDATE()) and  month(CreatedDate) = month(GETDATE())
order by CreatedDate desc;

select top(100)* from tblApprovedPrintLabel 
--where QRLabel ='OCB3702,6111322101-N053-2504,36,7,P,9/9,162610,1/1|1,201476.2023,1,1,3'
--where IdLabel = '169292.2023'
--Where OC = 'OPRT8594' AND BoxNo ='1/1'
where DAY(CreatedDate) = DAY(GETDATE()) and  month(CreatedDate) = month(GETDATE())
order by CreatedDate desc;

--select * from tblMetalScanResult order by createddate  desc
--select * from tblScanDataReject order by createddate  desc

--delete tblScanData where Id	= '9D870A7A-1441-47D0-9987-3C50E2EDD7B7'
--delete tblApprovedPrintLabel where Id = '995C059C-A271-48D7-B72C-54EC0C1182F7'

--update tblScanData set ActualDeviationPairs =10 where id ='51785199-ABC4-418E-8C33-53513285E57C'
--update tblScanData set Status=1 where id ='D29A25A7-3C19-4D92-B3A2-4FE672C744A3'
--update tblScanData set Actived=0 where id ='77827CF3-772B-4A44-A25D-7F58C53126EC'

--code approved: D7BEF08B-C830-4C67-9F2F-39D52AE178EE


--select * from tblWinlineProductsInfo  where ProductNumber ='6111010702-ADSN-2354' order by CreatedDate desc
--select * from tblCoreDataCodeItemSize order by CreatedDate desc
--select * from tblScanData order by CreatedDate desc

--truncate table tblScanData
--truncate table tblApprovedPrintLabel