// src/App.tsx
import React from "react";
import { useDispatch, useSelector } from "react-redux";
import {
  increment,
  decrement,
  incrementByAmount,
} from "./features/counter/counterSlice";
import { RootState } from "./app/store";

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
        <button
          className="bg-blue-500 text-white py-2 px-4 rounded"
          onClick={() => dispatch(increment())}
        >
          Increment
        </button>
        <button
          className="bg-red-500 text-white py-2 px-4 rounded"
          onClick={() => dispatch(decrement())}
        >
          Decrement
        </button>
        <button
          className="bg-green-500 text-white py-2 px-4 rounded"
          onClick={() => dispatch(incrementByAmount(100))}
        >
          Set to 100
        </button>
      </div>
    </div>
  );
};

export default App;
