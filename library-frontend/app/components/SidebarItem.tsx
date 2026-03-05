"use client";

import { usePathname, useRouter } from "next/navigation";

import { SidebarItemType } from "../../interfaces/SidebarItem";

interface SidebarItemProps {
  item: SidebarItemType;
  collapsed: boolean;
}

export default function SidebarItem({ item, collapsed }: SidebarItemProps) {
  const router = useRouter();
  const pathname = usePathname();

  const isActive = pathname === item.path;

  const handleClick = () => {
    router.push(item.path);
  };

  const baseClasses =
    "flex items-center gap-3 p-3 rounded-lg cursor-pointer transition";
  const activeClasses = isActive
    ? "bg-red-50 text-red-900"
    : "hover:bg-red-100 hover:text-red-900";

  return (
    <div onClick={handleClick} className={`${baseClasses} ${activeClasses}`}>
      {item.icon}
      {!collapsed && <span>{item.name}</span>}
    </div>
  );
}
