import LocalLibraryIcon from "@mui/icons-material/LocalLibrary";

import RememberMeCheckbox from "@/app/components/RememberMeCheckbox";
import AppButton from "@/components/AppButton";
import AppInput from "@/components/AppInput";
import AppLabel from "@/components/AppLabel";

const LoginUserPage = () => {
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
        <div className="mt-4 sm:mx-auto sm:w-full sm:max-w-sm">
          <form action="#" method="POST" className="space-y-3">
            <div>
              <AppLabel htmlFor="email" text="Email address" required />
              <AppInput id="email" type="email" required={true} />
            </div>
            <div>
              <div className="flex items-center justify-between">
                <AppLabel htmlFor="password" text="Password" required={true} />
                <div className="text-sm">
                  <a
                    href="#"
                    className="font-semibold text-indigo-400 hover:text-indigo-300"
                  >
                    Forgot password?
                  </a>
                </div>
              </div>
              <AppInput id="password" type="password" required={true} />
            </div>
            <div className="flex items-center justify-center h-full my-8">
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

export default LoginUserPage;
