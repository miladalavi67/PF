import { Outlet } from "react-router";

export default function ProtectedRoute() {
    // const isAuthenticate = isUserAuthenticate();

    // if (!isAuthenticate)
    //     return <Navigate to="/signout" replace />
    return <Outlet />
}
