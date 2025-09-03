import { api } from "@/lib/api";
import { PatronDto, CreatePatronDto, UpdatePatronDto } from "@/types/patron";
import { BookDto } from "@/types/book";

export const patronService = {
  // Get all patrons
  getAll: async (): Promise<PatronDto[]> => {
    const response = await api.get("/patrons");
    return response.data;
  },

  // Get patron by ID
  getById: async (id: string): Promise<PatronDto> => {
    const response = await api.get(`/patrons/${id}`);
    return response.data;
  },

  // Create patron
  create: async (data: CreatePatronDto): Promise<PatronDto> => {
    const response = await api.post("/patrons", data);
    return response.data;
  },

  // Update patron
  update: async (id: string, data: UpdatePatronDto): Promise<PatronDto> => {
    const response = await api.put(`/patrons/${id}`, data);
    return response.data;
  },

  // Delete patron
  delete: async (id: string): Promise<void> => {
    await api.delete(`/patrons/${id}`);
  },

  // Get patron's holds
  getHolds: async (patronId: string): Promise<BookDto[]> => {
    const response = await api.get(`/patrons/${patronId}/holds`);
    return response.data;
  },
};
