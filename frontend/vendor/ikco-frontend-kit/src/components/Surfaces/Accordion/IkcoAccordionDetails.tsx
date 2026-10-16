import { AccordionDetails } from "@mui/material";
import type { AccordionDetailsProps } from "@mui/material";

type IkcoAccordionDetailsProps= Omit<AccordionDetailsProps, "sx" | "style">;

export default function IkcoAccordionDetails(props: IkcoAccordionDetailsProps) {
  return <AccordionDetails {...props} />;
}
