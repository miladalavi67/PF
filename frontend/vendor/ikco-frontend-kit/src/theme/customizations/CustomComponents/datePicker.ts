import type { PickerComponents } from "@mui/x-date-pickers/themeAugmentation";
import type { Theme } from "@mui/material/styles";

export const datePickersCustomizations: PickerComponents<Theme> &
  PickerComponents<Theme> = {
//   MuiPickerPopper: {
//       styleOverrides: {
//           paper: ({ theme }) => ({
//               borderRadius: theme.shape.borderRadius,
//               border: `1px solid ${(theme.cssVariables || theme).palette.divider}`,
//               backgroundImage: 'none',
//               background: 'hsl(0, 0%, 100%)',
//               boxShadow: 'hsla(220, 30%, 5%, 0.07) 0px 4px 16px 0px, hsla(220, 25%, 10%, 0.07) 0px 8px 16px -5px',
//           }),
//       },
//   },
//     MuiDatePicker: {
//       defaultProps: {
//         slotProps: {
//           textField: {
//             size: "small",
//           },
//           actionBar: {
//             sx: {
//               button: {
//                 fontSize: "0.75rem",
//                 padding: "4px 8px",
//               },
//             },
//           },
//         },
//       },
//     },
};
