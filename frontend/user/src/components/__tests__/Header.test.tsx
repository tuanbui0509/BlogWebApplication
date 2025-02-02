// components/__tests__/Header.test.js
import { render, screen } from '@testing-library/react';
import Header from '../Header';

describe('Header', () => {
  it('renders the title correctly', () => {
    render(<Header title ="GGS Example" />);
    expect(screen.getByText('GGS Example')).toBeInTheDocument();
  });
});