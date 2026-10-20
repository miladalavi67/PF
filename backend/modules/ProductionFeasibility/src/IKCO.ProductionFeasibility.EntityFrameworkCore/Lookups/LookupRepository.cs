using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.EntityFrameworkCore;
using IKCO.ProductionFeasibility.Exceptions;
using IKCO.ProductionFeasibility.Lookups.Repositories;
using IKCO.ProductionFeasibility.StoredProcedures;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace IKCO.ProductionFeasibility.Lookups;

/// <summary>
/// فهرست‌های مرجع روی جدول/نمای پایه با پرس‌وجوی پارامتری و فهرست سفید (کلید از کلاینت هرگز به SQL اضافه نمی‌شود).
/// ستون‌های خروجی همه <c>Id</c>، <c>Code</c> و <c>Title</c> هستند.
/// </summary>
public class LookupRepository : PfDapperRepository<PfDbContext>, ILookupRepository, ITransientDependency
{
    private sealed record Source(string Title, string? ParentKey, string? Sql, Func<IEnumerable<LookupItem>>? Static = null);

    private static readonly Dictionary<string, Source> Sources = new(StringComparer.OrdinalIgnoreCase)
    {
        ["virtual-plants"] = new("سایت‌های مجازی", null,
            "SELECT CAST(Id AS int) AS Id, Code, DescF AS Title FROM dbo.B_VirtualPlant WITH (NOLOCK) WHERE Status = 1 ORDER BY Id"),
        ["body-saloons"] = new("خطوط بدنه‌سازی", "virtual-plants",
            "SELECT BiwSiteId AS Id, CAST(BiwSiteId AS varchar(20)) AS Code, BIWSiteDescF AS Title FROM dbo.VEXT_BIWSiteList WITH (NOLOCK) WHERE (@ParentId IS NULL OR VirtualPlantId = @ParentId) ORDER BY BIWSiteDescF"),
        ["trim-shops"] = new("سالن‌های مونتاژ", "virtual-plants",
            "SELECT CAST(BS.Id AS int) AS Id, BS.Code AS Code, BS.DescF AS Title FROM dbo.B_Saloon BS WITH (NOLOCK) INNER JOIN dbo.B_Process BP WITH (NOLOCK) ON BS.ProcessId = BP.Id WHERE BS.Status = 1 AND BP.Code IN ('3', '8') AND (@ParentId IS NULL OR BS.VirtualPlantId = @ParentId) ORDER BY BS.Code"),
        ["car-groups"] = new("خانواده محصول", null,
            "SELECT CAST(GroupID AS int) AS Id, GroupCode AS Code, GroupDescF AS Title FROM dbo.VEXT_CarGroup WITH (NOLOCK) WHERE Status = 1 ORDER BY GroupDescF"),
        ["car-models"] = new("مدل", "car-groups",
            "SELECT CAST(Id AS int) AS Id, Code, DescF AS Title FROM dbo.VEXT_CarModel WITH (NOLOCK) WHERE Status = 1 AND (@ParentId IS NULL OR CarGroupId = @ParentId) ORDER BY DescF"),
        ["car-classes"] = new("کلاس", "car-models",
            "SELECT CAST(Id AS int) AS Id, Code, DescF AS Title FROM dbo.VEXT_CarClass WITH (NOLOCK) WHERE Status = 1 AND (@ParentId IS NULL OR CarModelId = @ParentId) ORDER BY Code"),
        ["sub-classes"] = new("زیرکلاس", "car-classes",
            "SELECT CAST(Id AS int) AS Id, Code, DescF AS Title FROM dbo.B_SubClass WITH (NOLOCK) WHERE Status = 1 AND (@ParentId IS NULL OR CarClassId = @ParentId) ORDER BY Code"),
        ["production-possibilities"] = new("شرایط تولید", null,
            "SELECT CAST(Id AS int) AS Id, CAST(Id AS varchar(20)) AS Code, DescF AS Title FROM dbo.B_ProductionPossibility WITH (NOLOCK) ORDER BY Id"),
        ["critical-part-types"] = new("نوع بحران (مکانیکی / سنکرون)", null,
            "SELECT CAST(Id AS int) AS Id, CAST(Id AS varchar(20)) AS Code, DescF AS Title FROM dbo.B_CriticalPartType WITH (NOLOCK) ORDER BY Id"),
        ["body-critical-part-types"] = new("نوع بحران بدنه‌سازی", null,
            "SELECT CAST(Id AS int) AS Id, CAST(Id AS varchar(20)) AS Code, DescF AS Title FROM PF.B_CriticalPartType WITH (NOLOCK) ORDER BY Id"),
        ["depts"] = new("واحدهای مسئول کسری", null,
            "SELECT CAST(Id AS int) AS Id, Code, DescF AS Title FROM dbo.B_Dept WITH (NOLOCK) WHERE Status = 1 ORDER BY DescF"),
        ["trims"] = new("تریم", null,
            "SELECT CAST(Id AS int) AS Id, Code, DescF AS Title FROM dbo.B_Trim WITH (NOLOCK) WHERE Status = 1 ORDER BY Code"),
        ["toodoozis"] = new("تودوزی", null,
            "SELECT CAST(Id AS int) AS Id, Code, DescF AS Title FROM dbo.B_Toodoozi WITH (NOLOCK) WHERE Status = 1 ORDER BY Code"),
        ["options"] = new("آپشن", null,
            "SELECT CAST(Id AS int) AS Id, Code, DescF AS Title FROM dbo.B_Option WITH (NOLOCK) WHERE Status = 1 ORDER BY Code"),
        ["colors"] = new("رنگ", null,
            "SELECT CAST(Id AS int) AS Id, Code, DescF AS Title FROM dbo.B_Color WITH (NOLOCK) WHERE Status = 1 ORDER BY Code"),
        ["bom-part-types-level1"] = new("نوع قطعه BOM (سطح ۱)", null,
            "SELECT DISTINCT CAST(idlvl1 AS int) AS Id, CAST(idlvl1 AS varchar(20)) AS Code, parttype_desclvl1 AS Title FROM PF.VIMP_Bom_PartType WITH (NOLOCK) WHERE idlvl1 IS NOT NULL ORDER BY Title"),
        ["bom-part-types-level2"] = new("نوع قطعه BOM (سطح ۲)", null,
            "SELECT DISTINCT CAST(idlvl2 AS int) AS Id, CAST(idlvl2 AS varchar(20)) AS Code, parttype_desc AS Title FROM PF.VIMP_Bom_PartType WITH (NOLOCK) WHERE idlvl2 IS NOT NULL ORDER BY Title"),
        ["bom-part-types-level3"] = new("نوع قطعه BOM (سطح ۳)", null,
            "SELECT DISTINCT CAST(idlvl3 AS int) AS Id, CAST(idlvl3 AS varchar(20)) AS Code, parttype_desclvl3 AS Title FROM PF.VIMP_Bom_PartType WITH (NOLOCK) WHERE idlvl3 IS NOT NULL ORDER BY Title"),
        // فهرست‌های ثابت (مقادیر پارامترهای رویه‌های گزارش؛ مطابق توضیحات خود رویه‌ها)
        ["crisis-types"] = new("شرایط تولید بحران (فیلتر گزارش)", null, null, () => new[]
        {
            new LookupItem(100, "100", "همه موارد"), new LookupItem(0, "0", "عدم ورود"),
            new LookupItem(1, "1", "تولید با کسری"), new LookupItem(2, "2", "تولید با قطعه جایگزین"),
        }),
        ["prod-states"] = new("وضعیت تولید (گزارش به تفکیک سالن)", null, null, () => new[]
        {
            new LookupItem(100, "100", "همه موارد"), new LookupItem(1, "1", "تولید با کسری"),
            new LookupItem(2, "2", "تولید با قطعه جایگزین"),
        }),
        ["exception-filter-types"] = new("نوع فیلتر شماره فنی‌های دارای استثنا", null, null, () => new[]
        {
            new LookupItem(1, "1", "مدل"), new LookupItem(2, "2", "کلاس"), new LookupItem(3, "3", "زیرکلاس"), new LookupItem(4, "4", "سالن مونتاژ"),
        }),
        ["critical-kinds"] = new("نوع بحران (فیلتر جستجو)", null, null, () => new[]
        {
            new LookupItem(-1, "-1", "همه"), new LookupItem(0, "0", "مکانیکی"), new LookupItem(1, "1", "سنکرون"),
        }),
        ["contrast-kinds"] = new("نوع مغایرت زیرکلاس", null, null, () => new[]
        {
            new LookupItem(1, "1", "موجود در SAP - ناموجود در SFC"), new LookupItem(2, "2", "موجود در SFC - ناموجود در SAP"),
        }),
        ["active-states"] = new("فعال / غیرفعال", null, null, () => new[]
        {
            new LookupItem(1, "1", "فعال"), new LookupItem(2, "2", "غیرفعال"),
        }),
        ["critical-statuses"] = new("وضعیت بحران", null, null, () => new[]
        {
            new LookupItem(-1, "-1", "همه"), new LookupItem(1, "1", "فعال"), new LookupItem(2, "2", "غیرفعال"),
        }),
    };

    public LookupRepository(IDbContextProvider<PfDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public IReadOnlyList<LookupDefinition> GetCatalog()
        => Sources.Select(s => new LookupDefinition(s.Key, s.Value.Title, s.Value.ParentKey)).ToList();

    public async Task<List<LookupItem>> GetAsync(string key, int? parentId)
    {
        if (!Sources.TryGetValue(key, out var source))
            throw new BaseException(PfErrorCodes.LookupNotFound, $"فهرست مرجع «{key}» تعریف نشده است.").WithData("Key", key);

        if (source.Static != null)
            return source.Static().ToList();

        var fromProcedure = await FromLegacyProcedureAsync(key, parentId);
        if (fromProcedure != null)
            return fromProcedure;

        var rows = await QuerySqlAsync<Row>(source.Sql!, new { ParentId = parentId }, PfCommandTimeouts.Lookup);
        return rows.Select(r => new LookupItem(r.Id, r.Code, r.Title ?? string.Empty)).ToList();
    }

    /// <summary>سایت پیش‌فرض رویه‌های SFC وقتی فهرست بدون سایت درخواست شود (PlantId = 1 در سامانه قبلی)</summary>
    private const int DefaultPlantId = 1;

    /// <summary>
    /// فهرست‌هایی که بک‌اند قبلی (SyncParts / CriticalPartsReport) از رویه ذخیره‌شده می‌خواند با همان رویه و Dapper خوانده می‌شوند
    /// (PFReview بند ۴ و ۸)؛ سایر فهرست‌ها همان پرس‌وجوی پارامتری روی نمای پایه هستند که بک‌اند قبلی هم با متن SQL می‌خواند.
    /// </summary>
    private async Task<List<LookupItem>?> FromLegacyProcedureAsync(string key, int? parentId)
    {
        var plantId = (byte)(parentId ?? DefaultPlantId);
        List<LookupSpRow> rows;
        switch (key.ToLowerInvariant())
        {
            case "virtual-plants":
                var plants = await QueryRowsAsync<LookupPlantRow>("PF.GetSiteByDomainId", new SpParameters(), LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
                return plants.Where(p => p.VirtualPlantId != null)
                    .Select(p => new LookupItem(p.VirtualPlantId!.Value, p.VirtualPlantId.Value.ToString(), p.VirtualPlantDescF ?? string.Empty)).ToList();
            case "trim-shops" when parentId != null:
                rows = await QueryRowsAsync<LookupSpRow>("dbo.S_GetTrimShopSaloonsByVirtualPlnatId_Web", new SpParameters().Int("@VirtualPlantId", parentId),
                    LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
                break;
            case "depts":
                rows = await QueryRowsAsync<LookupSpRow>("dbo.S_DeptSelectAll", new SpParameters().TinyInt("@SelectType", 1).TinyInt("@PlantId", plantId),
                    LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
                break;
            case "trims":
                rows = await QueryRowsAsync<LookupSpRow>("dbo.S_TrimSelectAll", new SpParameters().TinyInt("@PlantId", plantId).TinyInt("@SelectType", 1),
                    LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
                break;
            case "toodoozis":
                rows = await QueryRowsAsync<LookupSpRow>("dbo.S_ToodooziSellectAll", new SpParameters().TinyInt("@PlantId", plantId),
                    LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
                break;
            case "options":
                rows = await QueryRowsAsync<LookupSpRow>("dbo.S_OptionSelectAll", new SpParameters().TinyInt("@SelectType", 1).TinyInt("@PlantId", plantId),
                    LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
                break;
            case "colors":
                rows = await QueryRowsAsync<LookupSpRow>("dbo.S_SelectColorsWith5DigitCode", new SpParameters().TinyInt("@Status", 1),
                    LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
                break;
            default:
                return null;
        }

        return rows.Where(r => r.Id != null).Select(r => new LookupItem(r.Id!.Value, r.Code?.Trim(), (r.DescF ?? r.Code ?? string.Empty).Trim())).ToList();
    }

    private sealed class Row
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Title { get; set; }
    }

    /// <summary>ستون‌های مشترک رویه‌های فهرست پایه (S_DeptSelectAll، S_TrimSelectAll، S_OptionSelectAll، S_ToodooziSellectAll، رنگ، سالن)</summary>
    private sealed class LookupSpRow : PfResultRow
    {
        public int? Id { get; set; }
        public string? Code { get; set; }
        public string? DescF { get; set; }
    }

    /// <summary>خروجی <c>PF.GetSiteByDomainId</c> (SiteDto بک‌اند قبلی)</summary>
    private sealed class LookupPlantRow : PfResultRow
    {
        public int? VirtualPlantId { get; set; }
        public string? VirtualPlantDescF { get; set; }
    }
}
