// import { GetStaticProps, GetStaticPaths } from 'next';
// import { fetchPost, fetchAllSlugs } from '../../services/posts';

// export const getStaticPaths: GetStaticPaths = async () => {
//   const slugs = await fetchAllSlugs();
//   return {
//     paths: slugs.map((slug:string) => ({ params: { slug } })),
//     fallback: false,
//   };
// };

// export const getStaticProps: GetStaticProps = async ({ params }) => {
//   const post = await fetchPost(params?.slug as string);
//   return {
//     props: { post },
//   };
// };

// // eslint-disable-next-line @typescript-eslint/no-explicit-any
// export default function Post() {
//   return <article>{/* Render post content */}</article>;
// }

import React from 'react'

const Postsa = () => {
  return (
    <div>Postsa</div>
  )
}

export default Postsa