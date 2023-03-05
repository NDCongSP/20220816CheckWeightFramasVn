select top(100) * from tblScanData 
--where BarcodeString ='OPRT8451,6117012202-2645-D243,86,2,P,2/3,170000,1/1|1,,,,'
--where IdLabel = '169292.2023'
Where OcNo = 'OPRT8592' AND BoxNo ='6/8'
order by CreatedDate desc;

select top(100)* from tblApprovedPrintLabel 
--where QRLabel ='OPRT8451,6117012202-2645-D243,86,2,P,2/3,170000,1/1|1,,,,'
--where IdLabel = '169292.2023'
Where OC = 'OPRT8594' AND BoxNo ='1/1'
order by CreatedDate desc;

--delete tblScanData where Id	= '54AD583F-1BE3-463E-8D93-A7B21C568B9D'
--delete tblApprovedPrintLabel where Id = '53BF7985-12B3-4881-BC5D-7A40BE905B9F'

--update tblScanData set ApprovedBy ='00000000-0000-0000-0000-000000000000' where id ='11EE87EC-C546-4DB4-9447-3967246B5629'
--update tblScanData set Status=1 where id ='D29A25A7-3C19-4D92-B3A2-4FE672C744A3'
--update tblScanData set Actived=0 where id ='2790C524-741F-429A-ACFB-6F9B57A6AE20'