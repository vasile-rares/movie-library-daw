import api from './api';

export const toWatchService = {
  async getMyList() {
    const response = await api.get('/towatch/my-list');
    return response.data;
  },

  async addToList(itemData) {
    const response = await api.post('/towatch', itemData);
    return response.data;
  },

  async removeFromList(id) {
    const response = await api.delete(`/towatch/${id}`);
    return response.data;
  },
};
