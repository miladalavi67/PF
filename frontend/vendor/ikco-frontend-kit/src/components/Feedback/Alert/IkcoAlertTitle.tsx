import { AlertTitle, type AlertTitleProps } from "@mui/material"


type IkcoAlertTitleProps = Omit<AlertTitleProps, 'sx' | 'style'>

export default function IkcoAlertTitle(props: IkcoAlertTitleProps) {
    return <AlertTitle {...props} />
}
