import { Chip, emphasize, styled } from "@mui/material";

interface BreadcrumbChipProps {
  label: string;
  onClick: () => void;
  clickable?: boolean;
  icon?: React.ReactElement;
}
const BreadcrumbChip = (props: BreadcrumbChipProps) => {
  const StyledBreadcrumb = styled(Chip)(({ theme }) => {
    return {
      backgroundColor: theme.palette.grey[100],
      padding: theme.spacing(0.0, 0.0),
      height: theme.spacing(3),
      color: (theme.vars || theme).palette.text.primary,
      fontWeight: theme.typography.fontWeightRegular,
      "&:hover, &:focus": {
        backgroundColor: emphasize(theme.palette.grey[100], 0.06),
        ...theme.applyStyles("dark", {
          backgroundColor: emphasize(theme.palette.grey[800], 0.06),
        }),
      },
      "&:active": {
        boxShadow: theme.shadows[1],
        backgroundColor: emphasize(theme.palette.grey[100], 0.12),
        ...theme.applyStyles("dark", {
          backgroundColor: emphasize(theme.palette.grey[800], 0.12),
        }),
      },
      ...theme.applyStyles("dark", {
        backgroundColor: theme.palette.grey[800],
      }),
    };
  }) as typeof Chip;

  return (
    <StyledBreadcrumb
      label={props.label}
      clickable={!!props.onClick}
      onClick={props.onClick}
      icon={props.icon}
    />
  );
};

export default BreadcrumbChip;
