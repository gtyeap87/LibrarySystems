import {
  FormControl,
  MenuItem,
  Select,
  SelectChangeEvent,
  styled,
} from "@mui/material";
import React from "react";

import { AppSelectProps } from "@/interfaces/AppSelect";

const AppSelect = (props: AppSelectProps) => {
  const [initials, setInitials] = React.useState("");

  const handleChange = (event: SelectChangeEvent) => {
    setInitials(event.target.value as string);
  };

  // styling

  // Base shared styles for both InputBase and Select

  const StyledMenuItem = styled(MenuItem)(() => ({
    fontSize: "var(--font-size-base)",
    fontFamily: "var(--font-calibri)",
  })) as typeof MenuItem;

  const StyledSelect = styled(Select)(() => ({
    width: "100%",
    borderRadius: "0.375rem",
    fontSize: "var(--font-size-base)",
    fontFamily: "var(--font-calibri)",
    backgroundColor: "#fff",

    // 1. Hide the default notched outline logic entirely to stop the "sticking"
    "& .MuiOutlinedInput-notchedOutline": {
      border: "1px solid rgba(0,0,0,0.1) !important", // Default light border
      transition: "all 0.1s ease-in-out",
    },

    // 2. Hover state: Only if the mouse is OVER it
    "&:hover .MuiOutlinedInput-notchedOutline": {
      borderColor: "#960000 !important",
      borderWidth: "2px !important",
    },

    // 3. Focused state: Use both MUI class AND focus-within for reliability
    "&.Mui-focused .MuiOutlinedInput-notchedOutline, &:focus-within .MuiOutlinedInput-notchedOutline":
      {
        borderColor: "#960000 important",
        borderWidth: "2px important",
      },

    // 4. Remove the blue glow/outline completely
    "&.MuiInputBase-root": {
      outline: "none",
      boxShadow: "none !important",
    },

    // 5. Internal Padding
    "& .MuiSelect-select": {
      padding: "8px 10px",
      "&:focus": {
        backgroundColor: "transparent",
      },
    },
  })) as unknown as typeof Select;

  return (
    <FormControl fullWidth size="small">
      <StyledSelect value={initials} onChange={handleChange}>
        {props.options.map((option) => (
          <StyledMenuItem key={option.value} value={option.value}>
            {option.label}
          </StyledMenuItem>
        ))}
      </StyledSelect>
    </FormControl>
  );
};

export default AppSelect;
