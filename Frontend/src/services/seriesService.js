import api from './api';

export const seriesService = {
  async getAll() {
    const response = await api.get('/series');
    return response.data;
  },

  async getById(id) {
    const response = await api.get(`/series/${id}`);
    return response.data;
  },

  async getByGenre(genreId) {
    const response = await api.get(`/series/genre/${genreId}`);
    return response.data;
  },

  async create(seriesData) {
    const response = await api.post('/series', seriesData);
    return response.data;
  },

  async update(id, seriesData) {
    const response = await api.put(`/series/${id}`, seriesData);
    return response.data;
  },

  async delete(id) {
    const response = await api.delete(`/series/${id}`);
    return response.data;
  },
};
