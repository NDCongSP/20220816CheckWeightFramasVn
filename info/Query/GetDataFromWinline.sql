declare @mesoyear int 
	set @mesoyear = (select MESOYEAR from [VNT86].[dbo].[vCompanyCurrentYear])

select 
	v21.c002 ProductNumber,
	v21.c003 ProductName,
	t69.c011  Brand,
	--v21. Decoration,
	v21.c011 MainProductNo,
	v21.c074 Color,
	v21.c067 Size,
	v21.c063 Weight,
	v21.c204 PackingMethod,
	v21.c215 LeftWeight,
	v21.c216 RightWeight,
	v21.c217 BoxType,
	v21.c202 ToolingNo,
	v21.c201 PackingBoxType,
	v21.c203 CustomeUsePb,
	v21.c206 PlacticBox,
	v21.c205 PPbagWeight,
	v21.c211 Bx1Weight,
	v21.c212 Bx2Weight,
	v21.c213 Bx3Weight,
	v21.c214 Bx4Weight,
	v21.c221 Bx1AWeight,
	v21.c207 Bx1_50_40_34,
	v21.c208 Bx2_50_40_17,
	v21.c219 Bx5Weight,
	v21.c209 Bx3_41_32_31,
	v21.c210 Bx4_32_23_15,
	v21.c220 Bx1A,
	v21.c218 Bx5_41_32_31,
	v21.c222 PlaticBoxWeight,
	v21.c223 PEUW,
	v21.c224 BagWeight,
	v21.c225 PartitionWeight,
	v21.c226 QtyPerbag,
	v21.c227 QtyPerPartition,
	v21.c228 QtyPerWrapSheet,
	v21.c229 WrapSheetWeight
from [VNT86].[dbo].v021 v21 
	left join [VNT86].[dbo].t070 t70 on t70.c000=v21.c010 and t70.c001 = 0 and t70.mesoyear = @mesoyear
	left join [CWLSYSTEM].[dbo].T069CMP t69 on t69.c001 = t70.c002
where v21.mesoyear = @mesoyear
order by ProductNumber asc