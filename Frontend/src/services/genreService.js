import api from './api';

export const genreService = {
  async getAll() {
    const response = await api.get('/genres');
    return response.data;
  },

  async getById(id) {
    const response = await api.get(`/genres/${id}`);
    return response.data;
  },

  async create(genreData) {
    const response = await api.post('/genres', genreData);
    return response.data;
  },

  async update(id, genreData) {
    const response = await api.put(`/genres/${id}`, genreData);
    return response.data;
  },

  async delete(id) {
    const response = await api.delete(`/genres/${id}`);
    return response.data;
  },
};
