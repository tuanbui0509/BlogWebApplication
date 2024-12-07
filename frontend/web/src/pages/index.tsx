import { useEffect, useState } from "react";

const Home = () => {
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  const [user, setUser] = useState<any>(null);

  const login = (provider: string) => {
    window.location.href = `http://localhost:5001/api/auth/login/${provider}`;
  };

  const logout = () => {
    setUser(null);
  };

  useEffect(() => {
    // You can fetch user info or token verification here if needed
  }, []);

  return (
    <div>
      <h1>Social Login Example</h1>
      <button onClick={() => login("Google")}>Login with Google</button>
      <button onClick={() => login("Facebook")}>Login with Facebook</button>
      <button onClick={() => login("GitHub")}>Login with GitHub</button>
      {user && <button onClick={logout}>Logout</button>}
    </div>
  );
};

export default Home;
