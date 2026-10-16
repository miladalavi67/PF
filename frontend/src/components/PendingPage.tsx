import { IkcoAlert, IkcoTypography } from "@ikco/frontend-kit";

interface PendingPageProps {
  title: string;
}

/** جایگزین موقت صفحه‌ای که هنوز پیاده‌سازی نشده است */
export default function PendingPage({ title }: PendingPageProps) {
  return (
    <>
      <IkcoTypography variant="h5">{title}</IkcoTypography>
      <IkcoAlert status="info" severity="info">این صفحه در حال پیاده‌سازی است.</IkcoAlert>
    </>
  );
}
