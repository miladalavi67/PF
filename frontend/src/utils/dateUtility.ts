import moment from "moment-jalaali";
import { persianToEnglishNumber } from "./convertionUtility";

function toMiladiDate(persianDateString: string | null): Date | null {
  if (!persianDateString) return null;
  return moment(persianDateString, "jYYYY/jMM/jDD").toDate();
}

function toPersianDate(date: Date | null) {
  if (!date) return null;
  // moment-jalaali (نه toLocaleDateString که نویسه‌های کنترلی جهت متن اضافه می‌کند)؛ ارقام همیشه لاتین
  return persianToEnglishNumber(moment(date).format("jYYYY/jMM/jDD"));
}

/** تاریخ شمسی امروز به قالب رشته‌ای Backend (yyyy/MM/dd، ارقام لاتین) */
function todayJalali(): string {
  return toPersianDate(new Date()) ?? "";
}

/** اول ماه جاری شمسی (پیش‌فرض «از تاریخ» گزارش‌ها) */
function firstOfMonthJalali(): string {
  const today = todayJalali();
  return today ? today.slice(0, 8) + "01" : "";
}

export { toMiladiDate, toPersianDate, todayJalali, firstOfMonthJalali };
