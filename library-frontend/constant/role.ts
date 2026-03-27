// roles.ts
export const Role = {
  Admin: "Admin",
  Librarian: "Librarian",
  Member: "Member",
} as const;

// optional: type for TypeScript
export type RoleType = (typeof Role)[keyof typeof Role];
