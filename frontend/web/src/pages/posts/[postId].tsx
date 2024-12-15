import { useRouter } from 'next/router';
import * as React from 'react';


export default function PostsDetailPage () {
    const router = useRouter();
  return (
    <div>
      Post details page
      <p>Query: {JSON.stringify(router.query)}</p>
    </div>
  );
}
