import { render, screen, fireEvent } from "@testing-library/react";
import Button from "../Button";
import { describe, it, expect, vi } from "vitest";

describe("Button Component", () => {
  it("renders correctly with default props", () => {
    render(<Button label="Click me" />);
    expect(screen.getByText("Click me")).toBeInTheDocument();
  });

  it("calls onClick when clicked", () => {
    const mockClick = vi.fn();
    render(<Button label="Click me" onClick={mockClick} />);
    fireEvent.click(screen.getByText("Click me"));
    expect(mockClick).toHaveBeenCalledTimes(1);
  });

  it("applies the correct variant styles", () => {
    render(<Button label="Primary Button" variant="primary" />);
    expect(screen.getByText("Primary Button")).toHaveClass("bg-blue-600");

    render(<Button label="Secondary Button" variant="secondary" />);
    expect(screen.getByText("Secondary Button")).toHaveClass("bg-gray-600");

    render(<Button label="Danger Button" variant="danger" />);
    expect(screen.getByText("Danger Button")).toHaveClass("bg-red-600");
  });

  it("disables the button when disabled prop is true", () => {
    render(<Button label="Disabled Button" disabled />);
    const button = screen.getByText("Disabled Button");
    expect(button).toBeDisabled();
    expect(button).toHaveClass("opacity-50 cursor-not-allowed");
  });
});