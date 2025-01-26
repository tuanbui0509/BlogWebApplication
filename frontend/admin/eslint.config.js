import globals from 'globals';
import react from 'eslint-plugin-react';
import typescript from '@typescript-eslint/eslint-plugin';
import reactHooks from 'eslint-plugin-react-hooks';
import tsParser from '@typescript-eslint/parser';

export default [
  // Basic ESLint configuration for JavaScript and TypeScript files
  {
    files: ['**/*.{js,jsx,ts,tsx}'],
    languageOptions: {
      ecmaVersion: 2020, // Modern ECMAScript features
      sourceType: 'module', // Use ES module imports
      parser: tsParser, // Use the TypeScript parser
      parserOptions: {
        ecmaFeatures: {
          jsx: true, // Enable JSX parsing
        },
      },
      globals: {
        ...globals.browser, // Browser globals
        ...globals.node,    // Node.js globals
      },
    },
    plugins: {
      react,
      '@typescript-eslint': typescript,
      'react-hooks': reactHooks,
    },
    rules: {
      'no-unused-vars': 'warn',
      'no-console': 'warn',
      '@typescript-eslint/no-unused-vars': ['error', { argsIgnorePattern: '^_' }],
      'react/react-in-jsx-scope': 'off', // Not needed with React 17+ JSX Transform
      'react/prop-types': 'off', // Disable prop-types since we use TypeScript
      'react-hooks/rules-of-hooks': 'error',
      'react-hooks/exhaustive-deps': 'warn',
    },
    settings: {
      'import/resolver': {
        alias: {
          map: [['@', './src']], // Alias `@` to the `src` folder
          extensions: ['.ts', '.tsx', '.js', '.jsx'],
        },
      },
    },
  },
];
