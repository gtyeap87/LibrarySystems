"use client";

import NavigateNextIcon from "@mui/icons-material/NavigateNext";
import { Breadcrumbs, Link, Typography } from "@mui/material";
import { usePathname, useRouter } from "next/navigation";

export default function AppBreadcrumbs() {
  const pathname = usePathname();
  const router = useRouter();

  const pathnames = pathname.split("/").filter(Boolean);

  return (
    <Breadcrumbs
      aria-label="breadcrumb"
      separator={<NavigateNextIcon fontSize="small" />}
      // className="text-sm"
      // style={{ fontFamily: "Calibri, Arial, Helvetica, sans-serif" }}
    >
      <Link
        underline="hover"
        color="inherit"
        onClick={() => router.push("/")}
        sx={{ cursor: "pointer" }}
      >
        Home
      </Link>

      {pathnames.map((value, index) => {
        const routeTo = "/" + pathnames.slice(0, index + 1).join("/");
        const isLast = index === pathnames.length - 1;

        const label = value.charAt(0).toUpperCase() + value.slice(1);

        return isLast ? (
          <Typography key={routeTo} color="text.primary">
            {label}
          </Typography>
        ) : (
          <Link
            key={routeTo}
            underline="hover"
            color="inherit"
            sx={{ cursor: "pointer" }}
            onClick={() => router.push(routeTo)}
          >
            {label}
          </Link>
        );
      })}
    </Breadcrumbs>
  );
}
