import { createBrowserRouter, RouterProvider, type RouteObject } from "react-router-dom";
import Layout from "../layouts/Layout";
import type { IConfig, IUserMenu } from "./types";
import { useEffect } from "react";
import { useTemplateStore } from "./templateStore";
import NotFound from "../pages/NotFound";
import '../styles/app.css';
import { Toaster } from "react-hot-toast";
import AccessRequired from "../pages/AccessRequired";
interface UserInfo {
    fullName: string;
    profileImage?: string;
    department?: string;
}

interface TemplateProviderProps {
    routes: RouteObject[];
    myMenus?: IUserMenu[];
    config: IConfig;
    userInfo?: UserInfo | null;
    headerLogo?: string;
    collapsedLogo?: string;
    appLogo?: string;
}


function isLocalEnvironment(): boolean {
    if (typeof window === "undefined") {
        return false;
    }

    const hostname = window.location.hostname;

    return (
        hostname === "localhost" ||
        hostname === "127.0.0.1" ||
        hostname === "::1" ||
        hostname === "[::1]"
    );
}
export default function IkcoTemplateProvider({ routes, config, myMenus, userInfo, headerLogo, collapsedLogo, appLogo }: TemplateProviderProps) {

    const {
        setLayoutConfig,
        setMyMenus,
        getUserUnit,
        setUserInfo,
        setHeaderLogo,
        setCollapsedLogo,
        setAppLogo
    } = useTemplateStore();

    useEffect(() => {

        setLayoutConfig(config);

        setMyMenus(myMenus || []);
        getUserUnit();
        setUserInfo(userInfo ?? null);
        // ← تنظیم لوگوها
        setHeaderLogo(headerLogo ?? null);
        setCollapsedLogo(collapsedLogo ?? null);
        setAppLogo(appLogo ?? null);

    }, [config, myMenus, userInfo, setLayoutConfig, setMyMenus, getUserUnit, setUserInfo]);



    const notFound = { element: <NotFound />, path: '*' } as RouteObject

    const updatedRoutes = [...routes, notFound];

    const router = createBrowserRouter([
        {
            path: "/",
            element: <Layout />,
            children: updatedRoutes
        }
    ])
    const isLocal = isLocalEnvironment();
    return (
        <>

            {isLocal ? (
                <>
                    <Toaster
                        position="top-center"
                        reverseOrder={false}
                    />

                    <RouterProvider router={router} />
                </>
            ) : (
                <AccessRequired />
            )}
        </>
    );

}
