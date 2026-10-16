import { DialogContent, type DialogContentTextProps } from "@mui/material"



type IkcoDialogContentTextProps = Omit<DialogContentTextProps, 'sx' | 'style'>

export default function IkcoDialogContentText({ ...props }: IkcoDialogContentTextProps) {
  return (
    <DialogContent {...props} />
  )
}
