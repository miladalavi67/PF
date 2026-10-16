import { create } from "zustand";
import type { IConfig, IUserMenu, IUserUnit } from "./types";
import {  fakeUserUnits } from "./fakeData";

interface UserInfo {
  fullName: string;
  profileImage?: string;
  department?: string;
}

interface initialState {
  userUnit: {
    status: "idle" | "loading" | "success" | "error";
    errorData: string | null;
    userUnitData: IUserUnit[] | [];
  };
  userMenu: {
    status: "idle" | "loading" | "success" | "error";
    errorData: string | null;
    realData: IUserMenu[] | [];
    menuData: IUserMenu[] | [];
  };
  layoutConfig: IConfig | null;
  myMenus: IUserMenu[] | [];

  userInfo: UserInfo | null;

  headerLogo: string | null;
  collapsedLogo: string | null;
  appLogo: string | null;

  setHeaderLogo: (logo: string | null) => void;
  setCollapsedLogo: (logo: string | null) => void;
  setAppLogo: (logo: string | null) => void;

  getUserUnit: () => Promise<void>;
  getUserMenu: () => Promise<void>;

  setLayoutConfig: (config: IConfig | null) => void;

  setMyMenus: (menus: IUserMenu[] | []) => void;
  setUserInfo: (info: UserInfo | null) => void;
}

export const useTemplateStore = create<initialState>((set, get) => ({
  userUnit: {
    status: "idle",
    errorData: null,
    userUnitData: [],
  },
  userMenu: {
    status: "idle",
    errorData: null,
    realData: [],
    menuData: [],
  },

  myMenus: [],
  userInfo: null,

  headerLogo: null,
  collapsedLogo: null,
  appLogo: null,

  setHeaderLogo: (logo) => set({ headerLogo: logo }),
  setCollapsedLogo: (logo) => set({ collapsedLogo: logo }),
  setAppLogo: (logo) => set({ appLogo: logo }),

  setMyMenus: (menus: IUserMenu[]) => set(() => ({ myMenus: menus })),

  layoutConfig: null,
  setLayoutConfig: (config: IConfig | null) =>
    set(() => ({ layoutConfig: config })),

  setUserInfo: (info: UserInfo | null) => set(() => ({ userInfo: info })),

  getUserUnit: async () => {
    set({ userUnit: { status: "loading", errorData: null, userUnitData: [] } });
    try {

      let userUnits: IUserUnit[] = [];
      userUnits = fakeUserUnits;

      set({
        userUnit: {
          status: "success",
          errorData: null,
          userUnitData: userUnits,
        },
      });
    } catch {
      set({ userUnit: { status: "error", errorData: null, userUnitData: [] } });
    }
  },

  getUserMenu: async () => {
    set({
      userMenu: {
        status: "loading",
        errorData: null,
        menuData: [],
        realData: [],
      },
    });
    try {
      let data: IUserMenu[] = [];

      data = get()?.myMenus?.length > 0 ? get()?.myMenus : [];

      set({
        userMenu: {
          status: "success",
          errorData: null,
          menuData: data,
          realData: data,
        },
      });
    } catch {
      set({
        userMenu: {
          status: "error",
          errorData: null,
          menuData: [],
          realData: [],
        },
      });
    }
  },
}));

/*
function buildFullTree(data: IUserMenu[]): IUserMenu[] {
  // ابتدا ریشه‌ها رو پیدا کن
  const roots = data.filter(
    (item) =>
      item.parentOperationId === item.operationId ||
      item.parentOperationId === null
  );
  // برای هر ریشه فرزندان رو بساز
  return roots.map((root) => ({
    ...root,
    children: buildTree(data, root.operationId),
  }));
}*/
/*
function buildTree(data: IUserMenu[], parentId: number | null): IUserMenu[] {
  return data
    .filter(
      (item) =>
        item.parentOperationId === parentId && item.operationId !== parentId
    )
    .map((item) => ({
      ...item,
      children: buildTree(data, item.operationId),
    }));
}
*/