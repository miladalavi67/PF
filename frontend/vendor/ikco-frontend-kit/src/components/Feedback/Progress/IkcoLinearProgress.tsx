import { LinearProgress, type LinearProgressProps } from "@mui/material"



// type IkcoLinearProgressProps = Omit<LinearProgressProps, 'sx' | 'style'>
export default function IkcoLinearProgress({ ...props }: LinearProgressProps) {
    return (
        <LinearProgress {...props} />
    )
}
