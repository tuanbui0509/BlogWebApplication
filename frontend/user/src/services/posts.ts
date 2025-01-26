import axiosInstance from "./axiosInstance";

// export const fetchAllSlugs = async () => {
//   try {
//     const { data } = await axiosInstance.get("/posts/slugs");
//     return data;
//   } catch (error) {
//     console.error("Error fetching slugs:", error);
//     throw error; // Propagate the error if needed
//   }
// };

export const fetchPost = async () => {
  try {
    const { data } = await axiosInstance.get(`/posts?pageNumber=1&pageSize=10`);
    return data;
  } catch (error) {
    console.error("Error fetching post:", error);
    throw error; // Propagate the error if needed
  }
};