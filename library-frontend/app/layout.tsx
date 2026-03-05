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
    <html lang="en">
      <head>
        <meta name="viewport" content="initial-scale=1, width=device-width" />
        <link rel="preconnect" href="https://fonts.googleapis.com" />
        <link
          rel="preconnect"
          href="https://fonts.gstatic.com"
          crossOrigin=""
        />
      </head>
      <body className="h-screen bg-gray-100">
        <div
          className={`grid h-screen ${
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
          <main className="overflow-auto p-6">
            <div className="container mx-auto bg-white rounded-lg shadow-2xl p-6 min-h-full">
              <ThemeProvider theme={theme}>{children}</ThemeProvider>
            </div>
          </main>
        </div>
      </body>
    </html>
  );
}
