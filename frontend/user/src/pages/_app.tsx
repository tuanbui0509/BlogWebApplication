import { useEffect } from "react";
import { setupInterceptors } from "../services/interceptors";
import { Provider } from "react-redux";
import { store } from "../store";
import "../styles/globals.scss";

// eslint-disable-next-line @typescript-eslint/no-explicit-any
export default function App({ Component, pageProps }:any) {
  useEffect(() => {
    setupInterceptors(); // Initialize interceptors
  }, []);

  return (
    <Provider store={store}>
      <Component {...pageProps} />
    </Provider>
  );
}
