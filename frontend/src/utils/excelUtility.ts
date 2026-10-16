import { persianToEnglishNumber } from "./convertionUtility";

/**
 * خواندن/نوشتن فایل xlsx (جایگزین خروجی اکسل Telerik و ورودی اکسل فرم‌های گروهی سامانه قدیم).
 * exceljs به صورت پویا بارگذاری می‌شود تا در bundle اولیه صفحات نباشد.
 */

export interface IExcelColumn {
  field: string;
  headerName: string;
}

/** ذخیره ردیف‌ها در فایل xlsx راست‌به‌چپ و دانلود آن */
async function exportToExcel(fileName: string, columns: IExcelColumn[], rows: Record<string, unknown>[]) {
  const ExcelJS = (await import("exceljs")).default;
  const workbook = new ExcelJS.Workbook();
  const sheet = workbook.addWorksheet("Sheet1", { views: [{ rightToLeft: true }] });
  sheet.columns = columns.map((c) => ({ header: c.headerName, key: c.field, width: Math.max(12, c.headerName.length + 4) }));
  rows.forEach((row) => sheet.addRow(Object.fromEntries(columns.map((c) => [c.field, row[c.field] ?? null]))));
  sheet.getRow(1).font = { bold: true };
  const buffer = await workbook.xlsx.writeBuffer();
  download(new Blob([buffer], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" }), `${fileName}.xlsx`);
}

/**
 * ردیف‌های اولین برگه (یا Sheet1) بدون سطر عنوان — قرارداد ورودی اکسل سامانه قدیم.
 * سلول‌های خالی null، ارقام فارسی به لاتین تبدیل می‌شوند.
 */
async function readExcelRows(file: File): Promise<(string | null)[][]> {
  const ExcelJS = (await import("exceljs")).default;
  const workbook = new ExcelJS.Workbook();
  await workbook.xlsx.load(await file.arrayBuffer());
  const sheet = workbook.getWorksheet("Sheet1") ?? workbook.worksheets[0];
  if (!sheet) return [];
  const rows: (string | null)[][] = [];
  sheet.eachRow({ includeEmpty: false }, (row) => {
    const values: (string | null)[] = [];
    for (let i = 1; i <= row.cellCount; i++) {
      const text = row.getCell(i).text?.trim();
      values.push(text ? persianToEnglishNumber(text) : null);
    }
    if (values.some((v) => v !== null)) rows.push(values);
  });
  return rows;
}

/** فایل نمونه ورودی (فقط سطر عنوان برای راهنمای کاربر) */
async function downloadSampleExcel(fileName: string, headers: string[]) {
  await exportToExcel(fileName, headers.map((h) => ({ field: h, headerName: h })), []);
}

function download(blob: Blob, fileName: string) {
  const url = URL.createObjectURL(blob);
  const a = document.createElement("a");
  a.href = url;
  a.download = fileName;
  a.click();
  URL.revokeObjectURL(url);
}

export { exportToExcel, readExcelRows, downloadSampleExcel };
