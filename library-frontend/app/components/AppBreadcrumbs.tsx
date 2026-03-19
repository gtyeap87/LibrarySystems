"use client";

import HomeIcon from "@mui/icons-material/Home";
import NavigateNextIcon from "@mui/icons-material/NavigateNext";
import { Breadcrumbs } from "@mui/material";
import { usePathname, useRouter } from "next/navigation";
import { isValidElement, ReactElement, ReactNode } from "react";

import BreadcrumbChip from "@/components/BreadcrumbChip";
import menu from "@/constant/sidebar";

export default function AppBreadcrumbs() {
  const pathname = usePathname();
  const router = useRouter();

  const pathnames = pathname.split("/").filter(Boolean);

  const sidebar = () => {
    const foundItem = menu.find((item) => {
      if (item.path === "/" + pathnames.join("/")) {
        return (item.name, item.icon);
      }
    });
    return foundItem;
  };

  const getSafeIcon = (icon: ReactNode): ReactElement | undefined => {
    return isValidElement(icon) ? icon : undefined;
  };

  return (
    <Breadcrumbs
      aria-label="breadcrumb"
      separator={<NavigateNextIcon fontSize="small" />}
    >
      <BreadcrumbChip
        label="Home"
        clickable={true}
        onClick={() => router.push("/")}
        icon={<HomeIcon />}
      />

      {pathnames.map((value, index) => {
        const routeTo = "/" + pathnames.slice(0, index + 1).join("/");

        const sidebarLabel = sidebar()?.name ?? "...";
        const sidebarIcon = getSafeIcon(sidebar()?.icon);

        return (
          <BreadcrumbChip
            key={value}
            label={sidebarLabel}
            clickable={true}
            onClick={() => router.push(routeTo)}
            icon={sidebarIcon}
          />
        );
      })}
    </Breadcrumbs>
  );
}
