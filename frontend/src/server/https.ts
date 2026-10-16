import { getAccessToken } from "@ikco/mfe-core";
import axios from "axios";
import toast from "react-hot-toast";

/** بدنه خطای ABP: { error: { code, message, details, validationErrors } } */
interface IAbpError {
  error?: {
    code?: string | null;
    message?: string | null;
    details?: string | null;
    validationErrors?: { message: string; members: string[] }[] | null;
  };
}

axios.interceptors.request.use(async (config) => {
  config.baseURL = import.meta.env.VITE_API_BASE_URL;
  // آرایه‌ها به شکل ids=1&ids=2 (model binding ASP.NET Core)
  config.paramsSerializer = { indexes: null };

  const authToken = await getAccessToken?.();

  if (authToken) {
    config.headers.set("Authorization", `Bearer ${authToken}`);
  }

  return config;
});

const abpMessage = (data: unknown): string | null => {
  const body = data as IAbpError | undefined;
  const error = body?.error;
  if (!error?.message) return null;
  const validation = error.validationErrors?.map((v) => v.message).filter(Boolean) ?? [];
  return validation.length > 0 ? `${error.message}\n${validation.join("\n")}` : error.message;
};

axios.interceptors.response.use(
  (response) => response,
  (error) => {
    const status = error.response ? error.response.status : null;
    const message = abpMessage(error.response?.data);
    if (status === 401) {
      toast.error("عدم دسترسی به منابع");
    } else if (status === 403) {
      toast.error(message ?? "شما به این عملیات دسترسی ندارید");
    } else if (status === 400 || status === 409) {
      // پیام RAISERROR رویه‌ها و خطاهای اعتبارسنجی همان‌طور که سرور برمی‌گرداند
      toast.error(message ?? "اطلاعات ارسالی معتبر نیست", { position: "top-center" });
    } else if (status === 404) {
      toast.error("اطلاعات يافت نشد");
    } else if (status === 503) {
      toast.error(message ?? "ارتباط با پایگاه داده برقرار نیست");
    } else if (status === 504) {
      toast.error(message ?? "زمان اجرای درخواست به پایان رسید");
    } else if (error.code === "ERR_NETWORK") {
      toast.error("کاربر گرامی! اتصال به شبکه برقرار نمی باشد");
    } else {
      toast.error(
        message ?? "کاربر گرامی! خطای نامشخص رخ داده است لطفا با راهبر سیستم تماس حاصل فرمایید",
      );
    }
    // خطا به کاربر نمایش داده شد؛ همچنان reject می‌شود تا صفحه‌ای که catch دارد بتواند واکنش نشان دهد
    error.__toasted = true;
    return Promise.reject(error);
  },
);

// صفحاتی که خطا را نمی‌گیرند (چون پیام قبلاً toast شده) نباید Unhandled Rejection در کنسول تولید کنند
window.addEventListener("unhandledrejection", (event) => {
  if ((event.reason as { __toasted?: boolean } | undefined)?.__toasted) event.preventDefault();
});

export default {
  get: axios.get,
  post: axios.post,
  put: axios.put,
  delete: axios.delete,
};
