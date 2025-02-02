// jest.config.js
const nextJest = require('next/jest');

const createJestConfig = nextJest({
  dir: './', // Path to your Next.js app
});

const customJestConfig = {
  setupFilesAfterEnv: ['<rootDir>/jest.setup.js'], // Setup file for React Testing Library
  testEnvironment: 'jest-environment-jsdom', // Use jsdom for browser-like environment
  moduleNameMapper: {
    '^@/(.*)$': '<rootDir>/$1', // Map aliases (if you use them in your project)
  },
  collectCoverage: true,
  coverageReporters: ['text', 'lcov'],
};

module.exports = createJestConfig(customJestConfig);