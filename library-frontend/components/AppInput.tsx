"use client";
import { InputBase, styled } from "@mui/material";

import { AppInputProps } from "@/interfaces/AppInput";

const AppInput = (props: AppInputProps) => {
  const StyledInputBase = styled(InputBase)(() => ({
    border: "1px solid rgba(0,0,0,0.1)",
    borderRadius: "0.375rem",
    padding: "3px 6px",
    fontSize: "var(--font-size-base)",
    fontFamily: "var(--font-calibri)",
    lineHeight: "1.5rem", // 24px
    "&:focus-within": {
      border: "2px solid #960000",
    },
  })) as typeof InputBase;

  return (
    <StyledInputBase
      fullWidth
      type={props.type}
      required={props.required}
      id={props.id}
      autoComplete="off"
    />
  );
};

export default AppInput;
