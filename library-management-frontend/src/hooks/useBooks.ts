import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { bookService } from "@/services/bookService";
import {
  CreateBookDto,
  UpdateBookDto,
  PlaceHoldRequest,
  CheckoutRequest,
} from "@/types/book";
import { toast } from "@/hooks/use-toast";

export const useBooks = () => {
  return useQuery({
    queryKey: ["books"],
    queryFn: bookService.getAll,
  });
};

export const useBook = (id: string) => {
  return useQuery({
    queryKey: ["book", id],
    queryFn: () => bookService.getById(id),
    enabled: !!id,
  });
};

export const useCreateBook = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: CreateBookDto) => bookService.create(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["books"] });
      toast({
        title: "Success",
        description: "Book created successfully",
      });
    },
    onError: (error: any) => {
      toast({
        title: "Error",
        description: error.response?.data?.error || "Failed to create book",
        variant: "destructive",
      });
    },
  });
};

export const useUpdateBook = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, data }: { id: string; data: UpdateBookDto }) =>
      bookService.update(id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["books"] });
      toast({
        title: "Success",
        description: "Book updated successfully",
      });
    },
    onError: (error: any) => {
      toast({
        title: "Error",
        description: error.response?.data?.error || "Failed to update book",
        variant: "destructive",
      });
    },
  });
};

export const useDeleteBook = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: string) => bookService.delete(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["books"] });
      toast({
        title: "Success",
        description: "Book deleted successfully",
      });
    },
    onError: (error: any) => {
      toast({
        title: "Error",
        description: error.response?.data?.error || "Failed to delete book",
        variant: "destructive",
      });
    },
  });
};

export const usePlaceBookOnHold = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      bookId,
      data,
    }: {
      bookId: string;
      data: PlaceHoldRequest;
    }) => bookService.placeOnHold(bookId, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["books"] });
      toast({
        title: "Success",
        description: "Book placed on hold successfully",
      });
    },
    onError: (error: any) => {
      toast({
        title: "Error",
        description:
          error.response?.data?.error || "Failed to place book on hold",
        variant: "destructive",
      });
    },
  });
};

export const useCheckoutBook = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ bookId, data }: { bookId: string; data: CheckoutRequest }) =>
      bookService.checkout(bookId, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["books"] });
      toast({
        title: "Success",
        description: "Book checked out successfully",
      });
    },
    onError: (error: any) => {
      toast({
        title: "Error",
        description: error.response?.data?.error || "Failed to checkout book",
        variant: "destructive",
      });
    },
  });
};
