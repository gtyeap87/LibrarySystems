"use client";

import MenuIcon from "@mui/icons-material/Menu";
import { Avatar, IconButton } from "@mui/material";

import AppBreadcrumbs from "./AppBreadcrumbs";

export default function Header({ toggle }: { toggle: () => void }) {
  return (
    <div className="h-16 flex items-center justify-between px-6">
      <IconButton onClick={toggle}>
        <MenuIcon />
      </IconButton>
      {/* Breadcrumbs */}
      <div className="px-6 py-3 bg-gray-100">
        <AppBreadcrumbs />
      </div>
      <Avatar>N</Avatar>
    </div>
  );
}
