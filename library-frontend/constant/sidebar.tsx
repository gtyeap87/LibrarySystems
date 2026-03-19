import AssignmentIcon from "@mui/icons-material/Assignment";
import DashboardIcon from "@mui/icons-material/Dashboard";
import BookIcon from "@mui/icons-material/MenuBook";
import PeopleIcon from "@mui/icons-material/People";

import { SidebarItemType } from "@/interfaces/SidebarItem";

const menu: SidebarItemType[] = [
  { name: "Home", icon: <DashboardIcon />, path: "/" },
  { name: "Books", icon: <BookIcon />, path: "/books" },
  { name: "Members", icon: <PeopleIcon />, path: "/members" },
  { name: "Loaned out books", icon: <AssignmentIcon />, path: "/loanbooks" },
];

export default menu;
