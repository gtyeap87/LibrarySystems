"use client";
import { Button, styled } from "@mui/material";

import { AppButtonProps } from "@/interfaces/AppButton";

const AppButton = (props: AppButtonProps) => {
  const StyledButton = styled(Button)(({}) => {
    return {
      backgroundColor: "rgb(150,0,0)",
      color: "#fff",
      borderRadius: "0.375rem", // rounded-md
      padding: "6px 12px", // px-3 py-1.5
      fontSize: "0.875rem", // text-sm
      fontWeight: 600,
      lineHeight: "1.5rem", // leading-6
      textTransform: "none", // remove uppercase
      minHeight: "unset",

      "&:hover": {
        backgroundColor: "rgb(248,113,113)",
      },
    };
  }) as typeof Button;

  return (
    <StyledButton type="submit" fullWidth disableElevation>
      {props.text}
    </StyledButton>
  );
};

export default AppButton;
