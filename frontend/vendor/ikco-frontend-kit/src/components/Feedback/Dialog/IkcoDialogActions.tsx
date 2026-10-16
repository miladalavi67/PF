import { DialogActions, type DialogActionsProps } from "@mui/material"



type IkcoDialogActionsProps = Omit<DialogActionsProps, 'style' | 'sx'>

export default function IkcoDialogActions({ ...props }: IkcoDialogActionsProps) {
  return (
    <DialogActions {...props} />
  )
}
