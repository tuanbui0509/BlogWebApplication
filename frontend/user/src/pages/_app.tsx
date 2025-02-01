import '../styles/globals.css'; // Import global styles
import type { AppProps } from 'next/app'; // Import AppProps type
import Layout from '../components/Layout'; // Import a layout component

function MyApp({ Component, pageProps }: AppProps) {
  return (
    <Layout>
      <Component {...pageProps} />
    </Layout>
  );
}

export default MyApp;