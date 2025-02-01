// src/App.tsx
import React from "react";
import { useDispatch, useSelector } from "react-redux";
import {
  increment,
  decrement,
  incrementByAmount,
} from "./features/counter/counterSlice";
import { RootState } from "./app/store";
import Button from "./components/Button";

const App: React.FC = () => {
  const dispatch = useDispatch();
  const counter = useSelector((state: RootState) => state.counter.value);

  return (
    <div className="App flex flex-col items-center justify-center min-h-screen bg-gray-200">
      <header className="text-center mb-4">
        <h1 className="text-2xl font-bold text-gray-700">
          Redux Toolkit with React
        </h1>
        <p className="text-lg text-gray-600 mt-2">Counter Value: {counter}</p>
      </header>
      <div className="flex space-x-4">
        <Button label="Increment" onClick={() => dispatch(increment())} />
        <Button label="Decrement" onClick={() => dispatch(decrement())} />
        <Button label="Set to 100" onClick={() => dispatch(incrementByAmount(100))}/>
      </div>
    </div>
  );
};

export default App;
