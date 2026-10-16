import { CircularProgress, type CircularProgressProps } from "@mui/material"



type IkcoCircularProgressProps = Omit<CircularProgressProps, 'sx' | 'style'>
export default function IkcoCircularProgress({ ...props }: IkcoCircularProgressProps) {
    return (
        <CircularProgress {...props} />
    )
}
