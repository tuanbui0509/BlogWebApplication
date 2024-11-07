"use client";

import { ThemeContext } from "@/context/ThemeContext";
import { ThemeContextType } from "@/types";
import React, { useContext, useEffect, useState } from "react";

const ThemeProvider = ({ children }: any) => {
  const { theme } = useContext(ThemeContext) as ThemeContextType;
  const [mounted, setMounted] = useState(false);
  useEffect(() => {
    setMounted(true);
  }, []);

  if (mounted) {
    return <div className={theme}>{children}</div>;
  }
  return <div>ThemeProvider</div>;
};

export default ThemeProvider;
