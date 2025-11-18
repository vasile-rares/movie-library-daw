import api from './api';

export const titleService = {
  async getAll() {
    const response = await api.get('/titles');
    return response.data;
  },

  async getById(id) {
    const response = await api.get(`/titles/${id}`);
    return response.data;
  },

  async getByType(type) {
    // type can be 'Movie' or 'Series'
    const response = await api.get(`/titles/type/${type}`);
    return response.data;
  },

  async getByGenre(genreId) {
    const response = await api.get(`/titles/genre/${genreId}`);
    return response.data;
  },

  async create(titleData) {
    const response = await api.post('/titles', titleData);
    return response.data;
  },

  async update(id, titleData) {
    const response = await api.put(`/titles/${id}`, titleData);
    return response.data;
  },

  async delete(id) {
    const response = await api.delete(`/titles/${id}`);
    return response.data;
  },
};
