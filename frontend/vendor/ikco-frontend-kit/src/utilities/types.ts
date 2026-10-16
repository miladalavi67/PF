
export interface ICurrentUser {
    fullName: string
    token: string
    nationalId: string | null,
    userName: string | null
    expireAt?: number,
    profile: string
}

export interface IUserUnit {
    systemUnitId: number;
    parentSystemUnitId: number;
    name: string;
    comment: string | null;
    showChild: number;
    agent: number;
    groups: IGroup[];
}


export interface IGroup {
    groupId: number;
    name: string;
    comment: string | null;
}



export interface IUserMenu {
    /** شناسه یکتا برای این آیتم منو (مثال: 'users.list') */
    id: string;

    /** شناسه آیتم والد - اگر null باشد، در سطح ریشه قرار می‌گیرد */
    parentId: string | null;

    /** عنوان نمایشی در منو (مثال: 'لیست کاربران') */
    title: string;

    /** مسیر URL برای هدایت به این صفحه (اگر null باشد، فقط به عنوان گروه عمل می‌کند) */
    path?: string | null;

    /** کلید مجوز برای کنترل دسترسی (اگر null باشد، برای همه قابل مشاهده است) */
    permissionKey?: string | null;

    /** نام کامپوننتی که در این مسیر رندر می‌شود */
    componentName?: string | null;

    /** مسیر فیزیکی فایل کامپوننت */
    componentPath?: string | null;

    /** نام آیکون برای نمایش کنار عنوان */
    icon?: string;

    /** عدد برای تعیین ترتیب نمایش در منو (مثال: 1) */
    order?: number;
    children?: IUserMenu[];


}

export interface IConfig {
    softwareId: number,
    mainPage: string,
    prefixApplicationTitlePage: string,
    applicationName: string,
    apiBaseUrl: string,
}

