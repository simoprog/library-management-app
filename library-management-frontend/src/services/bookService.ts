import { api } from "@/lib/api";
import {
  BookDto,
  CreateBookDto,
  UpdateBookDto,
  PlaceHoldRequest,
  CheckoutRequest,
} from "@/types/book";

export const bookService = {
  // Get all books
  getAll: async (): Promise<BookDto[]> => {
    const response = await api.get("/books");
    return response.data;
  },

  // Get book by ID
  getById: async (id: string): Promise<BookDto> => {
    const response = await api.get(`/books/${id}`);
    return response.data;
  },

  // Create book
  create: async (data: CreateBookDto): Promise<BookDto> => {
    const response = await api.post("/books", data);
    return response.data;
  },

  // Update book
  update: async (id: string, data: UpdateBookDto): Promise<BookDto> => {
    const response = await api.put(`/books/${id}`, data);
    return response.data;
  },

  // Delete book
  delete: async (id: string): Promise<void> => {
    await api.delete(`/books/${id}`);
  },

  // Place book on hold
  placeOnHold: async (
    bookId: string,
    data: PlaceHoldRequest
  ): Promise<void> => {
    await api.post(`/books/${bookId}/hold`, data);
  },

  // Checkout book
  checkout: async (bookId: string, data: CheckoutRequest): Promise<void> => {
    await api.post(`/books/${bookId}/checkout`, data);
  },

  // Return book
  returnBook: async (bookId: string): Promise<void> => {
    await api.post(`/books/${bookId}/return`);
  },
};
