import Link from "next/link";
import React from "react";

import RegisterForm from "@/app/libraries/library/components/RegisterForm/RegisterForm";

const RegisterUserPage = () => {
  return (
    <main>
      <RegisterForm />
      <Link href="/">Home</Link>
    </main>
  );
};

export default RegisterUserPage;
