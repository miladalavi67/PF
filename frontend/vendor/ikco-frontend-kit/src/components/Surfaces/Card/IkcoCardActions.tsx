import { CardActions, type CardActionsProps } from "@mui/material"



// type IkcoCardActionsProps = Omit<CardActionsProps, 'sx' | 'style'>
export default function IkcoCardActions({ ...props }: CardActionsProps) {
    return (
        <CardActions {...props} />
    )
}
