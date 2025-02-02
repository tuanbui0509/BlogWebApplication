import React from "react";

interface ButtonProps {
  label: string;
  onClick?: () => void;
  type?: "button" | "submit" | "reset";
  variant?: "primary" | "secondary" | "danger";
  disabled?: boolean;
}

const Button: React.FC<ButtonProps> = ({
  label,
  onClick,
  type = "button",
  variant = "primary",
  disabled = false,
}) => {
  const baseStyles =
    "px-4 py-2 rounded-lg text-white font-medium transition duration-300 ease-in-out";

  const variants = {
    primary: "bg-blue-600 hover:bg-blue-700 focus:ring-blue-500",
    secondary: "bg-gray-600 hover:bg-gray-700 focus:ring-gray-500",
    danger: "bg-red-600 hover:bg-red-700 focus:ring-red-500",
  };

  return (
    <button
      type={type}
      onClick={onClick}
      disabled={disabled}
      className={`${baseStyles} ${variants[variant]} ${
        disabled ? "opacity-50 cursor-not-allowed" : ""
      }`}
    >
      {label}
    </button>
  );
};

export default Button;