import api from './api';

export const ratingService = {
  async getAll() {
    const response = await api.get('/ratings');
    return response.data;
  },

  async getById(id) {
    const response = await api.get(`/ratings/${id}`);
    return response.data;
  },

  async getByUser(userId) {
    const response = await api.get(`/ratings/user/${userId}`);
    return response.data;
  },

  async getByMovie(movieId) {
    const response = await api.get(`/ratings/movie/${movieId}`);
    return response.data;
  },

  async getBySeries(seriesId) {
    const response = await api.get(`/ratings/series/${seriesId}`);
    return response.data;
  },

  async create(ratingData) {
    const response = await api.post('/ratings', ratingData);
    return response.data;
  },

  async update(id, ratingData) {
    const response = await api.put(`/ratings/${id}`, ratingData);
    return response.data;
  },

  async delete(id) {
    const response = await api.delete(`/ratings/${id}`);
    return response.data;
  },
};
