import api from './api';

export const movieService = {
  async getAll() {
    const response = await api.get('/movies');
    return response.data;
  },

  async getById(id) {
    const response = await api.get(`/movies/${id}`);
    return response.data;
  },

  async getByGenre(genreId) {
    const response = await api.get(`/movies/genre/${genreId}`);
    return response.data;
  },

  async create(movieData) {
    const response = await api.post('/movies', movieData);
    return response.data;
  },

  async update(id, movieData) {
    const response = await api.put(`/movies/${id}`, movieData);
    return response.data;
  },

  async delete(id) {
    const response = await api.delete(`/movies/${id}`);
    return response.data;
  },
};
