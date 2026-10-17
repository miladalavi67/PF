import { IkcoButton, IkcoGrid, IkcoStack, IkcoTextField } from "@ikco/frontend-kit";
import { Search } from "@mui/icons-material";

export interface IInfoCell {
  label: string;
  value: string | null | undefined;
}

interface BodyInfoStripProps {
  bodyNumber: string;
  onBodyNumberChange: (value: string) => void;
  /** جستجوی بدنه (Enter یا دکمه) */
  onSearch: () => void;
  /** سلول‌های اطلاعات بدنه به ترتیب فرم: محصول، رنگ، زیرکلاس، شماره موتور، VIN، مرحله جاری، سالن */
  cells: IInfoCell[];
  disabled?: boolean;
  loading?: boolean;
}

/** ردیف «شماره بدنه + مشخصات بدنه» مشترک فرم‌های ثبت بحران و تعداد بدنه‌ها (محصول، رنگ، زیرکلاس، موتور، VIN، مرحله، سالن) */
export default function BodyInfoStrip({ bodyNumber, onBodyNumberChange, onSearch, cells, disabled, loading }: BodyInfoStripProps) {
  return (
    <IkcoGrid container spacing={1} alignItems="center">
      <IkcoGrid size={{ xs: 12, sm: 6, md: 2 }}>
        <IkcoStack direction="row" spacing={1}>
          <IkcoTextField
            label="بدنه"
            value={bodyNumber}
            disabled={disabled}
            onChange={(e) => onBodyNumberChange(e.target.value)}
            onKeyDown={(e) => {
              if (e.key === "Enter") onSearch();
            }}
            slotProps={{ htmlInput: { maxLength: 8 } }}
          />
          <IkcoButton variant="outlined" loading={loading} disabled={disabled} onClick={onSearch} aria-label="جستجوی بدنه">
            <Search />
          </IkcoButton>
        </IkcoStack>
      </IkcoGrid>
      {cells.map((c) => (
        <IkcoGrid key={c.label} size={{ xs: 6, sm: 4, md: "grow" }}>
          <IkcoTextField label={c.label} value={c.value ?? ""} disabled />
        </IkcoGrid>
      ))}
    </IkcoGrid>
  );
}

/** سلول‌های اطلاعات بدنه (IBodyInfoRow) به ترتیب فرم */
export function bodyInfoCells(info: { carAttribute?: string | null; color?: string | null; subClassCode?: string | null; engineNO?: string | null; vin?: string | null; stepDescF?: string | null; saloonDescF?: string | null; saloon?: string | null } | null): IInfoCell[] {
  return [
    { label: "محصول", value: info?.carAttribute },
    { label: "رنگ", value: info?.color },
    { label: "زیرکلاس", value: info?.subClassCode },
    { label: "شماره موتور", value: info?.engineNO },
    { label: "VIN", value: info?.vin },
    { label: "مرحله جاری", value: info?.stepDescF },
    { label: "سالن", value: info?.saloonDescF ?? info?.saloon },
  ];
}
