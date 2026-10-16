/**
 * بررسی سازگاری mfe-manifest.ts با App.tsx و Backend:
 *  1) هر مسیر منو باید در routeObject فایل App.tsx باشد و برعکس (قانون «routes are declared twice»)
 *  2) فایل componentPath باید وجود داشته باشد و کامپوننت هم‌نام را default export کند
 *  3) هر permissionKey باید یکی از کلیدهای UI ثبت‌شده در Backend باشد (src/types/permissions.ts)
 * اجرا: npm run check:manifest
 */
import fs from "node:fs";
import path from "node:path";

const read = (p) => fs.readFileSync(p, "utf8");
const manifestSource = read("src/mfe-manifest.ts");
const appSource = read("src/App.tsx");
const permissionsSource = read("src/types/permissions.ts");

const uiKeys = new Set([...permissionsSource.matchAll(/^\s+"([^"]+)",/gm)].map((m) => m[1]));
const leaves = [...manifestSource.matchAll(/leaf\("([^"]+)", "[^"]+", "([^"]+)", "([^"]+)", (REPORTS|"[^"]*"), "([^"]+)", "([^"]+)", \d+\)/g)].map((m) => ({
  id: m[1],
  title: m[2],
  path: m[3],
  permissionKey: m[4] === "REPORTS" ? "reports.menu.view" : m[4].slice(1, -1),
  componentName: m[5],
  componentPath: `./src/pages/${m[6]}/${m[5]}`,
}));
const internal = [...manifestSource.matchAll(/path: "([^"]+)",\s*componentName: "([^"]+)",\s*componentPath: "([^"]+)"/g)].map((m) => ({
  id: m[2],
  title: m[2],
  path: m[1],
  permissionKey: "",
  componentName: m[2],
  componentPath: m[3],
}));

const appPaths = new Set([...appSource.matchAll(/\{ path: "([^"]+)", element:/g)].map((m) => m[1]));
const errors = [];

for (const leaf of [...leaves, ...internal]) {
  if (!appPaths.has(leaf.path)) errors.push(`مسیر «${leaf.path}» (${leaf.title}) در App.tsx تعریف نشده است`);
  appPaths.delete(leaf.path);

  const file = path.join(leaf.componentPath.replace("./", ""), "").replace(/\\/g, "/") + ".tsx";
  if (!fs.existsSync(file)) errors.push(`فایل کامپوننت «${file}» (${leaf.title}) وجود ندارد`);
  else if (!new RegExp(`export default function ${leaf.componentName}\\b`).test(read(file)))
    errors.push(`کامپوننت «${leaf.componentName}» در ${file} با همین نام default export نشده است`);

  if (leaf.permissionKey && !uiKeys.has(leaf.permissionKey))
    errors.push(`کلید دسترسی «${leaf.permissionKey}» (${leaf.title}) در درخت دسترسی Backend وجود ندارد`);
}

for (const extra of appPaths) errors.push(`مسیر «${extra}» در App.tsx هست ولی در mfe-manifest.ts نیست`);

if (errors.length > 0) {
  console.error(`✗ ${errors.length} مورد مغایرت:`);
  for (const e of errors) console.error("  - " + e);
  process.exit(1);
}
console.log(`✓ ${leaves.length} منو + ${internal.length} مسیر داخلی: مسیرها، کامپوننت‌ها و کلیدهای دسترسی سازگارند`);
