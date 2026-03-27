"use client";

import "./globals.css";

import { ThemeProvider } from "@mui/material/styles";
import { useState } from "react";

import Header from "./components/Header";
import Sidebar from "./components/Sidebar";
import theme from "./theme/theme";

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  const [collapsed, setCollapsed] = useState(false);

  return (
    <html lang="en" className="h-full bg-gray-100">
      <head>
        <meta name="viewport" content="initial-scale=1, width=device-width" />
        <link rel="preconnect" href="https://fonts.googleapis.com" />
        <link
          rel="preconnect"
          href="https://fonts.gstatic.com"
          crossOrigin=""
        />
      </head>
      <body className="h-full bg-gray-100">
        <div
          className={`grid h-full ${
            collapsed ? "grid-cols-[80px_1fr]" : "grid-cols-[240px_1fr]"
          } grid-rows-[64px_1fr] bg-gray-100`}
        >
          {/* Sidebar */}
          <div className="row-span-2">
            <Sidebar collapsed={collapsed} />
          </div>
          {/* Header */}
          <div>
            <Header toggle={() => setCollapsed(!collapsed)} />
          </div>
          {/* Page Content */}
          {/* <main className="overflow-auto p-6">
            <div className="max-w-full mx-auto bg-white rounded-lg shadow-2xl p-6 min-h-full">
              <ThemeProvider theme={theme}>{children}</ThemeProvider>
            </div>
          </main> */}

          {/* 2. Main Content (Fills the remaining screen space) */}
          <main className="flex-1 flex flex-col p-4 md:p-6 bg-gray-50 overflow-hidden">
            {/* 3. The Card Container */}
            <div className="flex-1 w-full max-w-7xl mx-auto bg-white rounded-lg shadow-2xl p-6 flex flex-col overflow-hidden">
              <ThemeProvider theme={theme}>
                <div className="flex-1 overflow-auto">{children}</div>
              </ThemeProvider>
            </div>
          </main>
        </div>
      </body>
    </html>
  );
}
