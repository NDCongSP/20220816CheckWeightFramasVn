declare @mesoyear int 
	set @mesoyear = (select MESOYEAR from [FTT2021].[dbo].[vCompanyCurrentYear])

SELECT 
    _v021.c002 ProductNumber,
    concat(_v021.c011,'-*-',RIGHT(_v021.c002,4)) CodeItemSize,
    _v021.c003 ProductName,
    --_t069.c011,--Text
    _v021.c005 ProductCategory,--=1 nó là thằng HeelCounter
    _t069.c011  Brand,
    CASE
        WHEN (select _t070.c001 from FTT2021..t070 _t070 
                INNER join [CWLSYSTEM]..T069CMP _t069 on _t069.C001 = _t070.c002 and _t069.C011 ='Decoration' and _t069.C006=_t070.c001
                where _t070.c000 = _v021.C002 and _t070.mesoyear = 1476	
            ) is not null THEN 1
        ELSE 0
    END Decoration,--_t069.c006  Decoration,
    --t70Deco.c002 Properties,
    _v021.c011 MainProductNo,
    _v021.c080 MainProductName,
    _v021.c074 Color,
    _v021.c067 SizeCode,
    (select c002 from [FTT2021].[dbo].t314 where mesoyear = @mesoyear and c003 = _v021.c067) SizeName,
    CAST(_v021.c063 as float) Weight,

    CAST(_v021.c215 as float) LeftWeight,
    CAST(_v021.c216 as float) RightWeight,
    _v021.c217 BoxType,
    _v021.c202 ToolingNo,
    _v021.c201 PackingBoxType,
    _v021.c203 CustomeUsePb,
    _v021.c004 Unit
FROM [FTT2021].[dbo].v021 _v021 
    LEFT JOIN FTT2021..t070 _t070 ON _t070.c000 = _v021.C002 and _t070.mesoyear = 1476 and _t070.c002 = (select top(1) c002 from FTT2021..t070 where c000 = _v021.c002 order by c002 desc)	--Join de lay BrandName	
    INNER join [CWLSYSTEM]..T069CMP _t069 on _t069.C001 = _t070.c002 --and _t069.C011 ='Decoration' and _t069.C006=_t070.c001
WHERE _v021.mesoyear = @mesoyear 
    and (_v021.c002 like '6%' or _v021.c002 like '7%') 
    and _v021.c038  is null 
    and _v021.c014 = 2 -- c014 =2  productItem; c014=1  mainItem
    -- and _v021.c002 = '6812322301-NBP2-3601'
ORDER BY ProductNumber ASC


select * from FTT2021..v021 where mesoyear=1476 and c002='6812012214-2541-E057'
select  * from [FTT2021].[dbo].t070 where mesoyear = 1476 and c000 = '6812012214-2541-E057' order by c002 desc --6812322301-NBP2-E056
select  * from CWLSYSTEM..T069CMP-- WHERE C000=1024

    --6811010702-ADSN-2553  ko sơn
    --6812322301-NBP2-3601 son
    --get decoration
    SELECT 
        _t070Decoration.*
        ,_t069Decoration.C011 BrandName
        ,_t069Decoration.c006 --Object
        ,_v021Decoration.*
    FROM FTT2021..v021 _v021Decoration
        LEFT JOIN t070 _t070Decoration ON _t070Decoration.c000 = _v021Decoration.C002 and _t070Decoration.mesoyear = 1476	--Join de lay BrandName	
        INNER join [CWLSYSTEM]..T069CMP _t069Decoration on _t069Decoration.C001 = _t070Decoration.c002 and _t069Decoration.C011 ='Decoration' --and _t069Decoration.C006=_t070Decoration.c001
    WHERE _v021Decoration.mesoyear=1476 and _v021Decoration.c002='6812322301-NBP2-3601'
--get brandName
SELECT _t070Brand.*,_t069BrandName.C011 BrandName,_t069BrandName.c006,_v021Brand.* from FTT2021..v021 _v021Brand 
    LEFT JOIN t070 _t070Brand ON _t070Brand.c000 = _v021Brand.C002 and _t070Brand.mesoyear = 1476	--Join de lay BrandName	
    INNER join [CWLSYSTEM]..T069CMP _t069BrandName on _t069BrandName.C001 = _t070Brand.c002 and _t069BrandName.C011 <> 'Decoration' --and _t069Decoration.C006=_t070Decoration.c001
where _v021Brand.mesoyear=1476 and _v021Brand.c002='6812322301-NBP2-3601'


SELECT _t309.* FROM FTT2021..t309 _t309 WHERE _t309.mesoyear=1476 

SELECT _t309.c000 SubcategoryNumber
	,_t309.c001 [Name]
	,_t309.c002 [Level]
	,_t309.c003 Remarks
	,_t309.c004 Graphic
	,_t309.c005 CalculationScheme
	,_v021.c078 V021_SubcategoryNumber
	,_v021.c002 ProductItemCode
	,_v021.c003 ProductItemName
	,_v021.* 
FROM FTT2021..v021 _v021
	LEFT JOIN FTT2021..t309 _t309 on _t309.mesoyear=1476 and _t309.c000=_v021.c078
WHERE _v021.mesoyear=1476 
	and (_v021.c002 like '6%' or _v021.c002 like '7%') 
	and _v021.c014 = 2
	and _v021.c002='6812012214-2541-E057'