/* eslint-disable @typescript-eslint/no-explicit-any */
import { useEffect, useState } from "react";
import { fetchPost } from "@/services/posts";

export default function Posts() {
  const [posts, setPosts] = useState([]);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const data = await fetchPost();
        setPosts(data);
      } catch (err: any) {
        setError(err.response?.data?.message || "An unexpected error occurred");
      }
    };

    fetchData();
  }, []);

  if (error) {
    return <div>Error: {error}</div>;
  }

  return (
    <ul>
      {posts.map((post) => (
        <li key={post}>{post}</li>
      ))}
    </ul>
  );
}
