"use client";
import { Button } from "@mui/material";
import React from "react";

const RegisterFormButtons = () => {
  return (
    <div>
      <Button onClick={() => alert("mocking register user")}>
        Register User
      </Button>
    </div>
  );
};

export default RegisterFormButtons;
