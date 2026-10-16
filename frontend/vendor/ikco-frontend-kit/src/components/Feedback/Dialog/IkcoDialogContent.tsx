import { DialogContent, type DialogContentProps } from "@mui/material"



type IkcoDialogContentsProps = Omit<DialogContentProps, 'sx' | 'style'>

export default function IkcoDialogContent({ ...props }: IkcoDialogContentsProps) {
  return (
    <DialogContent {...props} />
  )
}
