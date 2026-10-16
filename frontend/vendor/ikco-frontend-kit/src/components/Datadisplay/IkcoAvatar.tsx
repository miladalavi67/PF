import { Avatar } from "@mui/material";
import type { AvatarProps } from "@mui/material";

type IkcoAvatarProps = Omit<AvatarProps, "sx" | "style">

export default function IkcoAvatar(
  props: IkcoAvatarProps
) {
  return <Avatar {...props} />;
}