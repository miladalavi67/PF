import { Popover, type PopoverProps } from "@mui/material"



// type IkcoPopoverProps = Omit<PopoverProps, 'sx' | 'style'>
export default function IkcoPopover({ ...props }: PopoverProps) {
    return (
        <Popover {...props} />
    )
}
