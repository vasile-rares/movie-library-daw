import api from "./api";

export const authService = {
  async login(email, password) {
    const response = await api.post("/auth/login", { email, password });
    if (response.data.data) {
      // Token is now in HttpOnly cookie
      localStorage.setItem("user", JSON.stringify(response.data.data));
    }
    return response.data;
  },

  async register(nickname, email, password, profilePictureUrl = null) {
    const response = await api.post("/auth/register", {
      nickname,
      email,
      password,
      profilePictureUrl,
    });
    if (response.data.data) {
      // Token is now in HttpOnly cookie
      localStorage.setItem("user", JSON.stringify(response.data.data));
    }
    return response.data;
  },

  async logout() {
    try {
      await api.post("/auth/logout");
    } catch (error) {
      console.error("Logout failed", error);
    } finally {
      localStorage.removeItem("user");
      // We don't need to remove token as it was not in localStorage
    }
  },

  getCurrentUser() {
    const userStr = localStorage.getItem("user");
    return userStr ? JSON.parse(userStr) : null;
  },

  isAuthenticated() {
    // We can't check cookie existence in JS, so we rely on user object presence
    // Ideally we should verify with backend, but for UI state this is enough
    return !!localStorage.getItem("user");
  },
};
