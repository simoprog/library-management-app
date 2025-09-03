import { z } from "zod";
import { PatronType } from "@/types/patron";

// Patron validation schemas
export const createPatronSchema = z.object({
  name: z.string().min(1, "Name is required").max(255, "Name is too long"),
  email: z.string().email("Please enter a valid email address"),
  type: z.enum(PatronType),
});

export const updatePatronSchema = z.object({
  name: z.string().min(1, "Name is required").max(255, "Name is too long"),
  email: z.string().email("Please enter a valid email address"),
  type: z.enum(PatronType),
});

// Book validation schemas
export const createBookSchema = z.object({
  title: z.string().min(1, "Title is required").max(255, "Title is too long"),
  author: z
    .string()
    .min(1, "Author is required")
    .max(255, "Author name is too long"),
  isbn: z
    .string()
    .min(10, "ISBN must be at least 10 characters")
    .max(17, "ISBN is too long"),
  isRestrictedAccess: z.boolean(),
});

export const updateBookSchema = z.object({
  title: z.string().min(1, "Title is required").max(255, "Title is too long"),
  author: z
    .string()
    .min(1, "Author is required")
    .max(255, "Author name is too long"),
  isbn: z
    .string()
    .min(10, "ISBN must be at least 10 characters")
    .max(17, "ISBN is too long"),
  isRestrictedAccess: z.boolean(),
});

// Type exports
export type CreatePatronFormData = z.infer<typeof createPatronSchema>;
export type UpdatePatronFormData = z.infer<typeof updatePatronSchema>;
export type CreateBookFormData = z.infer<typeof createBookSchema>;
export type UpdateBookFormData = z.infer<typeof updateBookSchema>;
