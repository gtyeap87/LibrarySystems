"use client";

import AssignmentIcon from "@mui/icons-material/Assignment";
import Copyright from "@mui/icons-material/Copyright";
import DashboardIcon from "@mui/icons-material/Dashboard";
import LocalLibraryIcon from "@mui/icons-material/LocalLibrary";
import BookIcon from "@mui/icons-material/MenuBook";
import PeopleIcon from "@mui/icons-material/People";

import { SidebarItemType } from "../../interfaces/SidebarItem";
import SidebarItem from "./SidebarItem";

export default function Sidebar({ collapsed }: { collapsed: boolean }) {
  const menu: SidebarItemType[] = [
    { name: "Home", icon: <DashboardIcon />, path: "/" },
    { name: "Books", icon: <BookIcon />, path: "/books" },
    { name: "Members", icon: <PeopleIcon />, path: "/members" },
    { name: "Loaned out books", icon: <AssignmentIcon />, path: "/loanbooks" },
  ];

  return (
    <div className="flex flex-col h-full gradient-red text-white">
      {/* Logo */}
      <div className="h-20 flex items-center px-5 py-3 font-semibold">
        {!collapsed && (
          <div className="flex items-center gap-3">
            <LocalLibraryIcon className="text-2xl" />

            <div className="leading-tight">
              <div className="text-sm font-semibold">
                Library Management System
              </div>
              <div className="text-xs text-gray-300">Kuala Lumpur</div>
            </div>
          </div>
        )}
      </div>

      {/* Menu */}
      <div className="flex-1 p-2">
        {menu.map((item) => (
          <SidebarItem key={item.name} item={item} collapsed={collapsed} />
        ))}
      </div>

      {/* Footer */}
      <div className="p-4 text-xs text-white">
        {!collapsed && (
          <>
            <span>
              <Copyright /> 2026, GT Tecx. All rights reserved.
            </span>
          </>
        )}
      </div>
    </div>
  );
}
