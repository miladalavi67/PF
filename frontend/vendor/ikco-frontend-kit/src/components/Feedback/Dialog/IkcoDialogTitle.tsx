import { DialogTitle, type DialogTitleProps } from "@mui/material"


type IkcoDialogTitleProps = Omit<DialogTitleProps, 'style' | 'sx'>

export default function IkcoDialogTitle({ ...props }: IkcoDialogTitleProps) {
  return (
    <DialogTitle {...props} />
  )
}
