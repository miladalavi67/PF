import { Alert, type AlertProps } from "@mui/material"


type IkcoAlertVariant = "filled" | "outlined";
type IkcoAlertStatus = "default" | "success" | "warning" | "error" | "info";

interface IkcoAlertProps extends Omit<AlertProps, 'sx' | 'style'> {
    variant?: IkcoAlertVariant;
    status?: IkcoAlertStatus;
}

const statusColorMap: Record<IkcoAlertStatus, AlertProps["color"]> = {
    success: "success",
    warning: "warning",
    error: "error",
    info: "info",
    default: 'success'
};


export default function IkcoAlert({ status = "default", variant = "filled", ...props }: IkcoAlertProps) {
    return <Alert variant={variant} color={statusColorMap[status]}  {...props} />
}

