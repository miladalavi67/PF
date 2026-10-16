import { Accordion } from "@mui/material";
import type { AccordionProps } from "@mui/material";

type IkcoAccordionProps = Omit<AccordionProps, "sx" | "style">;

export default function IkcoAccordion(props: IkcoAccordionProps) {
  return <Accordion {...props} />;
}
