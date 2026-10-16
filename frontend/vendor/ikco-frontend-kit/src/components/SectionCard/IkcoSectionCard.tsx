import type { ReactNode } from "react";


import styles from "./IkcoSectionCard.module.css";
import { IkcoBox, IkcoStack } from "../Layout";
import { IkcoDivider, IkcoTypography } from "../Datadisplay";

export interface IkcoSectionCardProps {
    /**
     * عنوانی که در سمت راست Header نمایش داده می‌شود.
     */
    title?: ReactNode;

    /**
     * اکشن‌های Header که در سمت چپ نمایش داده می‌شوند.
     * مثال: Search, Filter, Reset, Add, Export
     */
    actions?: ReactNode;

    /**
     * محتوای اصلی Card.
     */
    children: ReactNode;

    /**
     * بخش Footer؛ برای دکمه‌های ثبت، لغو، Pagination و ...
     */
    footer?: ReactNode;

    /**
     * حذف Divider زیر Header در صورت نیاز.
     */
    hideHeaderDivider?: boolean;

    /**
     * حذف Divider بالای Footer در صورت نیاز.
     */
    hideFooterDivider?: boolean;

    /**
     * کلاس اضافه برای سفارشی‌سازی در صفحه مصرف‌کننده.
     */
    className?: string;

    /**
     * کلاس اضافه برای بخش Content.
     */
    contentClassName?: string;
}

export const IkcoSectionCard = ({
    title,
    actions,
    children,
    footer,
    hideHeaderDivider = false,
    hideFooterDivider = false,
    className = "",
    contentClassName = "",
}: IkcoSectionCardProps) => {
    const hasHeader = Boolean(title || actions);
    const hasFooter = Boolean(footer);

    return (
        <IkcoBox className={`${styles.card} ${className}`} dir="rtl">
            {hasHeader && (
                <IkcoBox className={styles.header}>
                    <IkcoStack
                        direction="row"
                        alignItems="center"
                        justifyContent="space-between"
                        className={styles.headerContent}
                    >
                        <IkcoBox className={styles.titleContainer}>
                            {typeof title === "string" ? (
                                <IkcoTypography variant="subtitle1" className={styles.title}>
                                    {title}
                                </IkcoTypography>
                            ) : (
                                title
                            )}
                        </IkcoBox>

                        {actions && (
                            <IkcoStack
                                direction="row"
                                alignItems="center"
                                className={styles.actions}
                            >
                                {actions}
                            </IkcoStack>
                        )}
                    </IkcoStack>

                    {!hideHeaderDivider && <IkcoDivider />}
                </IkcoBox>
            )}

            <IkcoBox className={`${styles.content} ${contentClassName}`}>
                {children}
            </IkcoBox>

            {hasFooter && (
                <IkcoBox className={styles.footer}>
                    {!hideFooterDivider && <IkcoDivider />}

                    <IkcoBox className={styles.footerContent}>{footer}</IkcoBox>
                </IkcoBox>
            )}
        </IkcoBox>
    );
};
