import { TextareaAutosize, type TextareaAutosizeProps } from "@mui/material"



// type IkcoTextareaAutosizeProps = Omit<TextareaAutosizeProps, 'sx' | 'style'>
export default function IkcoTextareaAutosize({ ...props }: TextareaAutosizeProps) {
    return (
        <TextareaAutosize {...props} />
    )
}
