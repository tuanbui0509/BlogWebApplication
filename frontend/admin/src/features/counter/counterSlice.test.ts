import { describe, expect, test } from 'vitest';
import counterReducer, { increment, decrement } from './counterSlice';

describe('Counter Reducer', () => {
  test('should return the initial state', () => {
    expect(counterReducer(undefined, { type: "" })).toEqual({ value: 0 });
  });

  test('should increment the value', () => {
    expect(counterReducer({ value: 0 }, increment())).toEqual({ value: 1 });
  });

  test('should decrement the value', () => {
    expect(counterReducer({ value: 1 }, decrement())).toEqual({ value: 0 });
  });
});
