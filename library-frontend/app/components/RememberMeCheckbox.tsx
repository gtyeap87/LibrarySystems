import { Checkbox, FormControlLabel } from "@mui/material";
import React from "react";

import { RememberMeCheckboxProps } from "@/interfaces/RememberMeCheckbox";

const RememberMeCheckbox = (props: RememberMeCheckboxProps) => {
  return (
    <>
      <FormControlLabel
        id={props.id}
        control={
          <Checkbox
            checked={props.checked}
            onChange={props.onChange}
            sx={{
              color: "rgba(0,0,0,0.2)",
              "&.Mui-checked": {
                color: "#960000", // Matching your red theme
              },
            }}
          />
        }
        label={
          <span
            className="block text-sm/6 font-normal text-gray-700"
            style={{
              fontFamily:
                "Calibri, Candara, Segoe, Segoe UI, Optima, Arial, sans-serif",
            }}
          >
            Remember me
          </span>
        }
      />
    </>
  );
};

export default RememberMeCheckbox;
