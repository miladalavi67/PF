import moment from "moment";
import { persianToEnglishNumber } from "./convertionUtility";

function toMiladiDate(persianDateString: string | null): Date | null {
    if (!persianDateString) return null;
    return moment(persianDateString, 'jYYYY/jMM/jDD').toDate();
}
function toPersianDate(date: Date | null) {
    if (!date)
        return null;
    const res = date.toLocaleDateString("fa-IR", { day: '2-digit', month: '2-digit', year: 'numeric' })
    return persianToEnglishNumber(res);
}


export {
    toMiladiDate,
    toPersianDate
}