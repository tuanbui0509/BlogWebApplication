import React from "react";

interface AboutProps {
  name: string;
}

export default function About({ name }: AboutProps) {
  return <div>AboutPage {name}</div>;
}
