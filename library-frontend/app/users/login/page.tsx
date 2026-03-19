import LocalLibraryIcon from "@mui/icons-material/LocalLibrary";
import Checkbox from "@mui/material/Checkbox";
import FormControlLabel from "@mui/material/FormControlLabel";

import AppButton from "@/components/AppButton";
import AppInput from "@/components/AppInput";

const LoginUserPage = () => {
  return (
    <>
      <div className="flex min-h-full flex-col justify-center px-6 py-12 lg:px-8">
        <div className="flex items-center justify-center h-full my-8">
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
        <div className="mt-10 sm:mx-auto sm:w-full sm:max-w-sm">
          <form action="#" method="POST" className="space-y-6">
            <div>
              <label htmlFor="email" className="block text-sm/6 font-medium">
                Email address
              </label>
              <AppInput type="email" required={true} />
            </div>
            <div>
              <div className="flex items-center justify-between">
                <label
                  htmlFor="password"
                  className="block text-sm/6 font-medium text-black"
                >
                  Password
                </label>
                <div className="text-sm">
                  <a
                    href="#"
                    className="font-semibold text-indigo-400 hover:text-indigo-300"
                  >
                    Forgot password?
                  </a>
                </div>
              </div>
              <AppInput type="password" required={true} />
            </div>
            <div className="flex items-center justify-center h-full my-8">
              <FormControlLabel
                control={<Checkbox defaultChecked />}
                label={
                  <span className="block text-sm/6 font-medium text-gray-700">
                    Remember me
                  </span>
                }
              />
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

export default LoginUserPage;
