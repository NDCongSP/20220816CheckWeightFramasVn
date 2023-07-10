select TOP (100) * from tblScanData 
--where BarcodeString in ('PBF000000128,6112042101-P672-2502,10,1,P,1/1,170000,1/1|1,402842.2023,,,')
--where IdLabel in ('346349.2023','349319.2023')
--where IdLabel in ('402842.2023')
Where OcNo = 'OCC2572' AND BoxNo ='79/100'
--where Station = 0
--where Id in ('451A8FA6-2435-4DE4-BB55-39EF146E954A','F904148E-2764-4055-B1CB-B40CA2C94056')
--where DAY(CreatedDate) = DAY(GETDATE()) and  month(CreatedDate) = month(GETDATE())
order by CreatedDate desc;

select top(100)* from tblApprovedPrintLabel 
--where QRLabel ='OCB3702,6111322101-N053-2504,36,7,P,9/9,162610,1/1|1,201476.2023,1,1,3'
--where IdLabel in ('402842.2023')
--where IdLabel in ('344520.2023','349320.2023')
Where OC = 'PBF000000126' AND BoxNo ='1/1'
--where DAY(CreatedDate) = DAY(GETDATE()) and  month(CreatedDate) = month(GETDATE())
--where ScanDataId = 'E9A6D475-6294-413F-923C-37A0EF92692A'
order by CreatedDate desc;

--select * from tblMetalScanResult order by createddate  desc
--select * from tblScanDataReject order by createddate  desc

--delete tblScanData where Id	in ('3D5B69EB-0629-470C-A9E5-3B78D5AF42D4','5B1698BD-63D4-432C-8A20-E093085CB089')
--delete tblApprovedPrintLabel where Id in ('05EFB3D1-4C8A-4C65-998F-C57D1C47AC1A')

--update tblScanData set ActualDeviationPairs = 27 where id ='978cfe2f-76a0-48d4-97d0-a3310f65a650'
--update tblScanData set Status=1 where id ='0d98b31e-8fac-4599-95ee-61e2c0b07faa' --01b9b561-133e-4e36-923d-6930b6baa6eb
--update tblScanData set Actived=0 where id in ('246A8191-673C-411E-BD14-A046E91F06C9')

--code approved: D7BEF08B-C830-4C67-9F2F-39D52AE178EE


--select * from tblWinlineProductsInfo  where ProductNumber ='6111010702-ADSN-2354' order by CreatedDate desc
--select * from tblCoreDataCodeItemSize order by CreatedDate desc
--select * from tblScanData order by CreatedDate desc