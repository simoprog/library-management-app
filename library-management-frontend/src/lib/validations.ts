import { PatronType } from "@/types/patron";
import { z } from "zod";

export const createBookSchema = z.object({
  title: z.string().min(1, "Title is required").max(200, "Title too long"),
  author: z
    .string()
    .min(1, "Author is required")
    .max(100, "Author name too long"),
  isbn: z.string().regex(/^\d{10}(\d{3})?$/, "Invalid ISBN format"),
  isRestrictedAccess: z.boolean().default(false),
});

export const updateBookSchema = createBookSchema;

export const createPatronSchema = z.object({
  name: z.string().min(1, "Name is required").max(100, "Name too long"),
  email: z.email().max(255, "Email too long"),
  type: z.enum(Object.values(PatronType)),
});

export const updatePatronSchema = createPatronSchema;

export type CreateBookFormData = z.infer<typeof createBookSchema>;
export type UpdateBookFormData = z.infer<typeof updateBookSchema>;
export type CreatePatronFormData = z.infer<typeof createPatronSchema>;
export type UpdatePatronFormData = z.infer<typeof updatePatronSchema>;
