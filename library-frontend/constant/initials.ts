// initials.ts
export const Initials = {
  Mr: "Mr",
  Mrs: "Mrs",
  Ms: "Ms",
  Dr: "Dr",
  Prof: "Prof",
} as const;

// optional: type for TypeScript
export type InitialsType = (typeof Initials)[keyof typeof Initials];
