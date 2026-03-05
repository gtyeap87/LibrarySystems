// theme.ts
'use client';
import { createTheme } from '@mui/material/styles';

const theme = createTheme({
  components: {
    MuiButton: {
      defaultProps: {
        // Sets all buttons to 'small', 'medium', or 'large' by default
        size: 'small', 
        variant: "contained"
      },
      styleOverrides: {
        root: {
          // You can also define exact pixel dimensions here if 'large' isn't enough
          // padding: '12px 24px',
        },
      },
    },
  },
  // typography: {
  //   // fontFamily: 'Calibri, Arial, Helvetica, sans-serif', // global font
  //   fontSize: 14, // default font size in px
  // },
});

export default theme;