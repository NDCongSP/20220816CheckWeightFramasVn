/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) *
  FROM [DOGE_WH].[dbo].[tblPrintedLabels]
  WHERE Id in ('F9FBCEC4-2CB7-4F4D-A939-000411EFEF86','F58310EE-BFC7-43AB-A9DB-00043288B0B8')


declare @_ocNo nvarchar(500) = 'LAB000000906'
print right(@_ocNo,9)

--SELECT TOP (1000) *
--  FROM [DOGE_WH].[dbo].[tblPrintedLabels_OC_Index]
--  WHERE Number = right(@_ocNo,9)
--  ORDER By CreatedDate desc


EXEC sp_IdcSsfgPrintedLabels_OC_IndexCheck @OcNo = @_ocNo

--update tblPrintedLabels_OC_Index set ParentOC ='OCB8221', ParentBoxCode ='51/149' where Id='DCE13366-2C3D-4577-A790-49CD24C08664'