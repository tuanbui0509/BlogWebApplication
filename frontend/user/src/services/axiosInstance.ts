import axios from "axios";

// Create an Axios instance
const axiosInstance = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL,
  timeout: 5000, // Set a timeout for requests
  headers: {
    "Content-Type": "application/json",
  },
});

export default axiosInstance;
