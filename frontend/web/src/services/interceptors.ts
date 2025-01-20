import axiosInstance from "./axiosInstance";

export const setupInterceptors = () => {
  // Request Interceptor
  axiosInstance.interceptors.request.use(
    (config) => {
      // Add Authorization header if token is available
      const token = localStorage.getItem("token");
      if (token) {
        config.headers.Authorization = `Bearer ${token}`;
      }
      return config;
    },
    (error) => {
      // Handle request errors
      console.error("Request Error:", error);
      return Promise.reject(error);
    }
  );

  // Response Interceptor
  axiosInstance.interceptors.response.use(
    (response) => {
      // Handle successful responses
      return response;
    },
    (error) => {
      // Handle errors globally
      if (error.response) {
        // API responded with a status code outside 2xx
        console.error(
          "Response Error:",
          error.response.data.message || error.message
        );

        // Handle specific error status codes
        switch (error.response.status) {
          case 401:
            console.warn("Unauthorized. Redirecting to login...");
            window.location.href = "/login"; // Redirect to login page
            break;
          case 403:
            console.warn("Forbidden. Access denied.");
            break;
          case 404:
            console.warn("Resource not found.");
            break;
          case 500:
            console.error("Internal server error. Try again later.");
            break;
          default:
            console.warn("An unexpected error occurred.");
        }
      } else if (error.request) {
        // No response received from server
        console.error("No response received:", error.request);
      } else {
        // Something happened in setting up the request
        console.error("Error setting up request:", error.message);
      }

      // Reject the promise with the error
      return Promise.reject(error);
    }
  );
};
