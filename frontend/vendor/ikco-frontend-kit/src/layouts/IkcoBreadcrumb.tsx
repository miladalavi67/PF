import Breadcrumbs from '@mui/material/Breadcrumbs';
import Typography from '@mui/material/Typography';
import Link from '@mui/material/Link';
import Stack from '@mui/material/Stack';
import { useLocation, Link as RouterLink } from 'react-router-dom'; // اگر React Router داری
import type { IUserMenu } from '../utilities/types';
import { useEffect, useState } from 'react';
import { useTemplateStore } from '../utilities/templateStore';

export default function IkcoBreadcrumb() {

    const { userMenu } = useTemplateStore();

    const { layoutConfig } = useTemplateStore();
    const location = useLocation();
    const [paths, setPaths] = useState<IUserMenu[]>([]);


    useEffect(() => {

        const pathSegments = location.pathname.split('/').filter(Boolean);
        const targetEnName = pathSegments[pathSegments.length - 1] || '';

        const paths = findPathToRoot(userMenu.realData, targetEnName);
        setPaths(paths);

    }, [location, userMenu.realData])


    // ساخت آرایه JSX برای breadcrumbs
    const breadcrumbs = [
        <Link underline="hover" key="home" color="inherit" component={RouterLink} to={layoutConfig?.mainPage ?? ''}>
            صفحه اصلی
        </Link>,
        ...paths.map((item, index) => {
            const isLast = index === paths.length - 1;
            return isLast ? (
                <Typography key={item.id} color="text.primary" >
                    {item.title}
                </Typography>
            ) : (
                <Link
                    underline="hover"
                    key={item.id}
                    color="inherit"
                    component={RouterLink}
                    to={item.path ?? ''}
                >
                    {item.title}
                </Link>
            );
        }),
    ];

    return (
        <Stack>
            <Breadcrumbs separator="›" aria-label="breadcrumb">
                {breadcrumbs}
            </Breadcrumbs>
        </Stack>
    );
}

function findPathToRoot(data: IUserMenu[], targetEnName: string): IUserMenu[] {
    const map = new Map<string, IUserMenu>();
    data.forEach(item => map.set(item.id, item));

    const target = data.find(item => item.title === targetEnName);
    if (!target) return [];

    const path: IUserMenu[] = [];
    let current: IUserMenu | undefined = target;

    while (current) {
        path.unshift(current);
        if (current.parentId === null || current.parentId === current.id) break;
        current = map.get(current.parentId ?? 0);
    }

    return path;
}
