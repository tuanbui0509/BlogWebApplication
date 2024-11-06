"use client";

import { ThemeContextType } from "@/types";
import { createContext, useEffect, useState, ReactNode, FC } from "react";

export const ThemeContext = createContext<ThemeContextType | undefined>(
  undefined
);

const getFromLocalStorage = (): string => {
  if (typeof window !== "undefined") {
    const value = localStorage.getItem("theme");
    return value || "light";
  }
  return "light";
};

interface ThemeContextProviderProps {
  children: ReactNode;
}

export const ThemeContextProvider: FC<ThemeContextProviderProps> = ({
  children,
}) => {
  const [theme, setTheme] = useState<string>(() => getFromLocalStorage());

  const toggle = () => {
    setTheme((prevTheme) => (prevTheme === "light" ? "dark" : "light"));
  };

  useEffect(() => {
    localStorage.setItem("theme", theme);
  }, [theme]);

  return (
    <ThemeContext.Provider value={{ theme, toggle }}>
      {children}
    </ThemeContext.Provider>
  );
};
