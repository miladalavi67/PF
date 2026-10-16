import { AccordionSummary } from "@mui/material";
import type { AccordionSummaryProps } from "@mui/material";

type IkcoAccordionSummaryProps = Omit<AccordionSummaryProps, "sx" | "style">;

export default function IkcoAccordionSummary(props: IkcoAccordionSummaryProps) {
  return <AccordionSummary {...props} />;
}
