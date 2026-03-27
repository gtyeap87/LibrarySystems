"use client";
import LocalLibraryIcon from "@mui/icons-material/LocalLibrary";

import RememberMeCheckbox from "@/app/components/RememberMeCheckbox";
import AppButton from "@/components/AppButton";
import AppInput from "@/components/AppInput";
import AppLabel from "@/components/AppLabel";
import AppSelect from "@/components/AppSelect";
import { Initials } from "@/constant/initials";
import { Role } from "@/constant/role";

const RegisterUserPage = () => {
  // const [age, setAge] = React.useState("");

  // const handleChange = (event: SelectChangeEvent) => {
  //   setAge(event.target.value as string);
  // };

  return (
    <>
      <div className="flex min-h-full flex-col justify-center px-6 py-12 lg:px-8">
        <div className="flex items-center justify-center h-full my-4">
          <div className="flex items-center gap-3 text-center">
            <LocalLibraryIcon className="text-2xl" />
            <div className="leading-tight">
              <div className="text-sm/6 font-semibold">
                Library Management System
              </div>
              <div className="text-xs text-gray-400">Kuala Lumpur</div>
            </div>
          </div>
        </div>
        <div className="m-4 sm:mx-auto sm:w-full sm:max-w-sm">
          <form action="#" method="POST" className="space-y-3">
            <div>
              <AppLabel htmlFor="initials" text="Initials" />
              <AppSelect
                options={[
                  { value: Initials.Mr, label: Initials.Mr },
                  { value: Initials.Mrs, label: Initials.Mrs },
                  { value: Initials.Ms, label: Initials.Ms },
                  { value: Initials.Dr, label: Initials.Dr },
                  { value: Initials.Prof, label: Initials.Prof },
                ]}
              />
            </div>
            <div>
              <AppLabel htmlFor="firstName" text="First Name" required={true} />
              <AppInput id="firstName" type="text" required={true} />
            </div>
            <div>
              <AppLabel htmlFor="lastName" text="Last Name" required={true} />
              <AppInput id="lastName" type="text" required={true} />
            </div>
            <div>
              <AppLabel htmlFor="email" text="Email address" required={true} />
              <AppInput id="email" type="email" required={true} />
            </div>
            <div>
              <AppLabel htmlFor="password" text="Password" required={true} />
              <AppInput id="password" type="password" required={true} />
            </div>
            <div>
              <AppLabel htmlFor="roles" text="Roles" />
              <AppSelect
                options={[
                  { value: Role.Admin, label: "Admin" },
                  { value: Role.Librarian, label: "Librarian" },
                  { value: Role.Member, label: "Member" },
                ]}
              />
            </div>
            <div className="flex items-center justify-center h-full">
              <RememberMeCheckbox checked={true} id={"remember-me"} />
            </div>
            <div>
              <AppButton text="Sign in" />
            </div>
          </form>
        </div>
      </div>
    </>
  );
};

export default RegisterUserPage;
