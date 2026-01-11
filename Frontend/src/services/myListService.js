import api from "./api";

export const myListService = {
  async getMyList() {
    const response = await api.get("/mylist/my-list");
    return response.data;
  },

  async addToList(itemData) {
    const response = await api.post("/mylist", itemData);
    return response.data;
  },

  async updateStatus(id, status) {
    const response = await api.put(`/mylist/${id}/status`, { status });
    return response.data;
  },

  async removeFromList(id) {
    const response = await api.delete(`/mylist/${id}`);
    return response.data;
  },
};

export const WatchStatus = {
  PlanToWatch: 0,
  Watching: 1,
  Completed: 2,
  OnHold: 3,
  Dropped: 4,
};
