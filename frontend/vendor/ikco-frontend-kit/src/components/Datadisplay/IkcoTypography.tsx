import { Typography } from "@mui/material";
import type { TypographyProps } from "@mui/material";

type IkcoTypographyProps = Omit<TypographyProps, "sx" | "style">

export default function IkcoTypography(
  props: IkcoTypographyProps
) {
  return <Typography {...props} />;
}