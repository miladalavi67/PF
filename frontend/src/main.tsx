import { createRoot } from "react-dom/client";
import moment from "moment-jalaali";
import App from "./App.tsx";

// IkcoDatePicker هنگام import، moment را با usePersianDigits=true بارگذاری می‌کند و در آن حالت تجزیه دقیق (strict) تاریخ
// شمسی با ارقام لاتین شکست می‌خورد و مقدارهای آماده مثل "1405/06/31" به 0784/... تبدیل می‌شوند. چون قالب رشته‌ای
// Backend همیشه ارقام لاتین است، پس از بارگذاری کیت همان تنظیم را با ارقام لاتین اعمال می‌کنیم.
moment.loadPersian({ dialect: "persian-modern", usePersianDigits: false });

createRoot(document.getElementById("root")!).render(<App />);
