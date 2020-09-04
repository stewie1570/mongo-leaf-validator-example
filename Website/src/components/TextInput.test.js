import React, { useState } from "react";
import { TextInput } from "./TextInput";
import { render, screen, fireEvent } from "@testing-library/react";

function TestApp() {
  const [text, setText] = useState("");

  return (
    <>
      <TextInput data-testid="text-input" value={text} onChange={setText} />
      The text: {text}
    </>
  );
}

test("text input works", () => {
  render(<TestApp />);

  fireEvent.change(screen.getByTestId("text-input"), {
    target: { value: "expected test text" },
  });

  screen.getByText("The text: expected test text");
});
