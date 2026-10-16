import { setAccessTokenProvider } from "@ikco/mfe-core";
import { create } from "zustand";
import http from "../server/https";
import { getPfUserCurrentPermissions } from "../server/services/pfUserService";
import { ApiPermissions, uiPermissionKeys } from "../types/permissions";

/**
 * نشست اجرای مستقل (npm run dev) — فقط برای توسعه/آزمون محلی. در دریچه توکن و دسترسی‌ها را پوسته فراهم می‌کند و این
 * store استفاده نمی‌شود. توکن از POST /_auth/token (Oidc:Mode=Embedded سرور) و دسترسی‌ها از
 * GET api/production-feasibility/users/me/permissions (همان سیاست‌های IKCO.Auth کنترلرها) گرفته می‌شود.
 */
interface DevSessionState {
  token: string | null;
  userName: string | null;
  permissions: ReadonlySet<string>;
  /** دسترسی‌ها خوانده شده‌اند؛ پیش از آن نباید منو ساخته شود (پوسته منو را یک‌بار می‌سازد) */
  ready: boolean;
  login: (userName: string) => Promise<void>;
  restore: () => Promise<void>;
  /** اجرای مستقل بدون فرم ورود: همه منوها و صفحات باز؛ توکن در صورت در دسترس بودن Backend بی‌صدا گرفته می‌شود */
  startOpenAccess: () => Promise<void>;
  logout: () => void;
}

/**
 * VITE_AUTH_MODE=login فرم ورود توسعه را فعال می‌کند؛ در غیر این صورت (پیش‌فرض) اجرای مستقل بدون ورود است.
 * در دریچه هیچ‌کدام استفاده نمی‌شود.
 */
export const openAccessMode = import.meta.env.VITE_AUTH_MODE !== "login";

/** همه کلیدهای دسترسی UI و API (برای حالت بدون ورود) */
const allPermissionKeys = (): Set<string> => {
  const keys = new Set<string>(uiPermissionKeys);
  const walk = (node: unknown) => {
    if (typeof node === "string") keys.add(node);
    else if (node && typeof node === "object") Object.values(node).forEach(walk);
  };
  walk(ApiPermissions);
  return keys;
};

const STORAGE_KEY = "productionfeasibility.dev.session";

export const useDevSessionStore = create<DevSessionState>((set, get) => ({
  token: null,
  userName: null,
  permissions: new Set<string>(),
  ready: false,

  login: async (userName: string) => {
    const { data } = await http.post<{ accessToken: string }>("/_auth/token", {
      userName,
      company: import.meta.env.VITE_DEV_COMPANY ?? "ikco",
    });
    set({ token: data.accessToken, userName });
    const granted = await getPfUserCurrentPermissions();
    set({ permissions: new Set(granted), ready: true });
    sessionStorage.setItem(STORAGE_KEY, JSON.stringify({ token: data.accessToken, userName }));
  },

  restore: async () => {
    const saved = sessionStorage.getItem(STORAGE_KEY);
    if (!saved || get().token) return;
    const { token, userName } = JSON.parse(saved) as { token: string; userName: string };
    set({ token, userName });
    try {
      set({ permissions: new Set(await getPfUserCurrentPermissions()), ready: true });
    } catch {
      sessionStorage.removeItem(STORAGE_KEY);
      set({ token: null, userName: null, permissions: new Set(), ready: false });
    }
  },

  startOpenAccess: async () => {
    if (get().ready) return;
    const userName = import.meta.env.VITE_DEV_USER || "devtester";
    set({ userName, permissions: allPermissionKeys(), ready: true });
    // fetch به جای axios تا نبودن Backend پیام خطای سراسری نشان ندهد؛ صفحات در هر حال نمایش داده می‌شوند
    try {
      const response = await fetch(new URL("_auth/token", import.meta.env.VITE_API_BASE_URL), {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ userName, company: import.meta.env.VITE_DEV_COMPANY ?? "ikco" }),
      });
      if (response.ok) set({ token: ((await response.json()) as { accessToken: string }).accessToken });
    } catch {
      // Backend در دسترس نیست؛ درخواست‌های API بدون توکن ارسال می‌شوند
    }
  },

  logout: () => {
    sessionStorage.removeItem(STORAGE_KEY);
    set({ token: null, userName: null, permissions: new Set(), ready: false });
  },
}));

// ارائه‌دهنده توکن پیش از هر درخواست ثبت می‌شود (اولین فراخوانی در restore پیش از افکت App انجام می‌شود)
setAccessTokenProvider(async () => useDevSessionStore.getState().token);

/** ارائه‌دهنده توکن برای setAccessTokenProvider در اجرای مستقل */
export const devAccessToken = async () => useDevSessionStore.getState().token;
