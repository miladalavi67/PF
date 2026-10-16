import { Dialog, type DialogProps } from "@mui/material";


type IkcoDialogProps = Omit<DialogProps,  'sx' | 'style'>

export default function IkcoDialog({ ...props }: IkcoDialogProps) {
    return (
        <Dialog {...props} />
    )
}
