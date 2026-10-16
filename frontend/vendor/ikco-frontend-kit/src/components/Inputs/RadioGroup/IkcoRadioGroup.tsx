import {
  Radio,
  RadioGroup,
  FormControlLabel,
  FormControl,
  FormLabel,
  type RadioGroupProps,
} from "@mui/material";

type Option = {
  label: string;
  value: string;
  disabled?: boolean;
};

type IkcoRadioGroupProps = Omit<RadioGroupProps, "sx" | "style"> & {
  label?: string;
  options: Option[];
};

export default function IkcoRadioGroup({
  label,
  options,
  row = false,
  ...rest
}: IkcoRadioGroupProps) {
  return (
    <FormControl>
      {label && <FormLabel>{label}</FormLabel>}

      <RadioGroup row={row} {...rest}>
        {options.map((option) => (
          <FormControlLabel
            key={option.value}
            value={option.value}
            control={<Radio size="small" />}
            label={option.label}
            disabled={option.disabled}
          />
        ))}
      </RadioGroup>
    </FormControl>
  );
}
