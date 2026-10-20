import { IkcoButton, IkcoCard, IkcoCardContent, IkcoDataGrid, IkcoDatePicker, IkcoGrid, IkcoRadioGroup, IkcoStack, IkcoTypography } from "@ikco/frontend-kit";
import { PermissionGuard } from "@ikco/mfe-core";
import { Refresh } from "@mui/icons-material";
import { useMemo, useState } from "react";
import toast from "react-hot-toast";
import ExcelExportButton from "../../components/ExcelExportButton";
import PartNumberField from "../../components/PartNumberField";
import { getMechReportsAsnReport } from "../../server/services/mechReportsService";
import type { ITabularResult } from "../../types/tabular";
import { ApiPermissions } from "../../types/permissions";
import { columnHeaders } from "../../utils/columnHeaders";
import { firstOfMonthJalali, todayJalali } from "../../utils/dateUtility";
import { resultColumns, tabularRows } from "../../utils/tabularUtility";
import ActionBar from "../../components/ActionBar";
import { gridColumns } from "../../utils/gridColumns";

/**
 * گزارش ASN های در راه (frm_Rpt_Sap_Asn، رویه S_ASNReport): «نمایش قطعات بحرانی» (Type=1) یا «نمایش تمامی قطعات در راه» (Type=2)،
 * بر اساس بازه تاریخ یا بازه تاریخ و شماره فنی. Backend: GET /api/production-feasibility/mechanical/reports/asn، دسترسی mechanical.reports.sap.
 */
export default function MechAsnPanel() {
  const [type, setType] = useState("1");
  const [byPart, setByPart] = useState("date");
  const [fromDate, setFromDate] = useState<string | null>(firstOfMonthJalali());
  const [toDate, setToDate] = useState<string | null>(todayJalali());
  const [partNo, setPartNo] = useState("");
  const [result, setResult] = useState<ITabularResult | null>(null);
  const [loading, setLoading] = useState(false);

  const columns = useMemo(() => resultColumns(result, columnHeaders, gridColumns.mechAsn), [result]);
  const rows = useMemo(() => tabularRows(result), [result]);

  const run = async () => {
    if (!fromDate || !toDate) {
      toast.error("بازه تاریخ را وارد کنید");
      return;
    }
    if (byPart === "part" && !partNo.trim()) {
      toast.error("شماره فنی را وارد کنید");
      return;
    }
    setLoading(true);
    try {
      setResult(await getMechReportsAsnReport({ partNumber: byPart === "part" ? partNo.trim() : null, fromDate, toDate, type: Number(type) }));
    } finally {
      setLoading(false);
    }
  };

  return (
    <IkcoStack spacing={2}>
      <IkcoCard>
        <IkcoCardContent>
          <IkcoStack spacing={2}>
            <IkcoTypography variant="subtitle1">شرایط جستجو</IkcoTypography>
            <IkcoRadioGroup row value={type} onChange={(_, v) => setType(v)} options={[{ value: "1", label: "نمایش قطعات بحرانی" }, { value: "2", label: "نمایش تمامی قطعات در راه" }]} />
            <IkcoRadioGroup row value={byPart} onChange={(_, v) => setByPart(v)} options={[{ value: "date", label: "بر اساس تاریخ" }, { value: "part", label: "بر اساس تاریخ و شماره فنی" }]} />
            <IkcoGrid container spacing={2} alignItems="center">
              <IkcoGrid size={{ xs: 6, md: 2 }}>
                <IkcoDatePicker label="از تاریخ" value={fromDate} onChange={setFromDate} />
              </IkcoGrid>
              <IkcoGrid size={{ xs: 6, md: 2 }}>
                <IkcoDatePicker label="تا تاریخ" value={toDate} onChange={setToDate} />
              </IkcoGrid>
              <PartNumberField value={partNo} onChange={setPartNo} disabled={byPart !== "part"} descLabel="شرح" size={{ xs: 12, md: 3 }} descSize={{ xs: 12, md: 5 }} />
            </IkcoGrid>
          </IkcoStack>
        </IkcoCardContent>
      </IkcoCard>
      <IkcoTypography variant="subtitle1">نمایش</IkcoTypography>
      <IkcoDataGrid columns={columns} rows={rows} loading={loading} getRowId={(r) => r.__rowId} autoHeight />
      <ActionBar>
          <PermissionGuard can={ApiPermissions.mechReports.sap} fallback={<IkcoTypography color="error">شما به اجرای این گزارش دسترسی ندارید.</IkcoTypography>}>
            <IkcoButton variant="contained" startIcon={<Refresh />} loading={loading} onClick={() => void run()}>
              بازخوانی
            </IkcoButton>
          </PermissionGuard>
          <ExcelExportButton fileName="گزارش ASN های در راه" columns={columns} rows={rows as unknown as Record<string, unknown>[]} />
          <IkcoTypography variant="caption">{result ? `تعداد: ${rows.length}` : "شرایط را وارد و «بازخوانی» را انتخاب کنید"}</IkcoTypography>
        </ActionBar>
    </IkcoStack>
  );
}
