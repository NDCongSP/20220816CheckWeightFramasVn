select TOP (100) * from tblScanData 
--where BarcodeString in ('PRT000000133,6112321904-N043-2952,20,1,P,1/1,170000,1/1|1,382178.2023,,,')
--where IdLabel in ('346349.2023','349319.2023')
--where IdLabel in ('387718.2023')
Where OcNo = 'PRTA4810' AND BoxNo ='33/55'
--where Station = 0
--where Id in ('451A8FA6-2435-4DE4-BB55-39EF146E954A','F904148E-2764-4055-B1CB-B40CA2C94056')
--where DAY(CreatedDate) = DAY(GETDATE()) and  month(CreatedDate) = month(GETDATE())
order by CreatedDate desc;

select TOP (100) * from tblScanData 
--where BarcodeString in ('PRT000000133,6112321904-N043-2952,20,1,P,1/1,170000,1/1|1,382178.2023,,,')
--where IdLabel in ('346349.2023','349319.2023')
--where IdLabel in ('387718.2023')
Where OcNo = 'PRT000000141' AND BoxNo ='1/1'
--where Station = 0
--where Id in ('451A8FA6-2435-4DE4-BB55-39EF146E954A','F904148E-2764-4055-B1CB-B40CA2C94056')
--where DAY(CreatedDate) = DAY(GETDATE()) and  month(CreatedDate) = month(GETDATE())
order by CreatedDate desc;

select top(100)* from tblApprovedPrintLabel 
--where QRLabel ='OCB3702,6111322101-N053-2504,36,7,P,9/9,162610,1/1|1,201476.2023,1,1,3'
--where IdLabel in ('387718.2023')
--where IdLabel in ('344520.2023','349320.2023')
Where OC = 'PRTA4810' AND BoxNo ='33/55'
--where DAY(CreatedDate) = DAY(GETDATE()) and  month(CreatedDate) = month(GETDATE())
--where ScanDataId = 'E9A6D475-6294-413F-923C-37A0EF92692A'
order by CreatedDate desc;

--select * from tblMetalScanResult order by createddate  desc
--select * from tblScanDataReject order by createddate  desc

--delete tblScanData where Id	in ('5b365fae-0883-4ec9-a48e-d8376a71da59','3ca9c4c3-51f3-423a-859b-a635b2fa7b74')
--delete tblApprovedPrintLabel where Id in ('c65bd0c3-61fa-48a1-9828-92811e9173ed')

--update tblScanData set ActualDeviationPairs = 27 where id ='978cfe2f-76a0-48d4-97d0-a3310f65a650'
--update tblScanData set Status=1 where id ='0d98b31e-8fac-4599-95ee-61e2c0b07faa' --01b9b561-133e-4e36-923d-6930b6baa6eb
--update tblScanData set Actived=1 where id in ('f28383e0-4497-4a3c-bcc0-765103d8d219','d6fb2ed3-8d65-4f29-9821-30452930a349','67d34a66-e39b-4bce-897a-91cc1777c196','f31092e6-d78f-4fec-b31f-e19b3f57509b')

--code approved: D7BEF08B-C830-4C67-9F2F-39D52AE178EE


--select * from tblWinlineProductsInfo  where ProductNumber ='6111010702-ADSN-2354' order by CreatedDate desc
--select * from tblCoreDataCodeItemSize order by CreatedDate desc
--select * from tblScanData order by CreatedDate desc