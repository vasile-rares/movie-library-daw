import api from "./api";

export const authService = {
  async login(email, password) {
    const response = await api.post("/auth/login", { email, password });
    if (response.data.data) {
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
    }
  },

  getCurrentUser() {
    const userStr = localStorage.getItem("user");
    return userStr ? JSON.parse(userStr) : null;
  },

  isAuthenticated() {
    return !!localStorage.getItem("user");
  },
};
