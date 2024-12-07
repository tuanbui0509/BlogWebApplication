import { signIn, signOut, useSession } from "next-auth/react";
import styles from "../styles/Home.module.css";

const LoginButtons: React.FC = () => {
  const { data: session } = useSession();

  return (
    <div>
      {!session ? (
        <div>
          <button className={styles.button} onClick={() => signIn("google")}>
            Sign in with Google
          </button>
          <button className={styles.button} onClick={() => signIn("facebook")}>
            Sign in with Facebook
          </button>
          <button className={styles.button} onClick={() => signIn("github")}>
            Sign in with GitHub
          </button>
        </div>
      ) : (
        <div>
          <p>Welcome, {session.user?.name}</p>
          <button className={styles.button} onClick={() => signOut()}>
            Sign out
          </button>
        </div>
      )}
    </div>
  );
};

export default LoginButtons;
