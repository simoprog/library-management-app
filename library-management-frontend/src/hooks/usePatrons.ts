import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { patronService } from "@/services/patronService";
import { CreatePatronDto, UpdatePatronDto } from "@/types/patron";
import { toast } from "@/hooks/use-toast";

export const usePatrons = () => {
  return useQuery({
    queryKey: ["patrons"],
    queryFn: patronService.getAll,
  });
};

export const usePatron = (id: string) => {
  return useQuery({
    queryKey: ["patron", id],
    queryFn: () => patronService.getById(id),
    enabled: !!id,
  });
};

export const useCreatePatron = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: CreatePatronDto) => patronService.create(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["patrons"] });
      toast({
        title: "Success",
        description: "Patron created successfully",
      });
    },
    onError: (error: any) => {
      toast({
        title: "Error",
        description: error.response?.data?.error || "Failed to create patron",
        variant: "destructive",
      });
    },
  });
};

export const useUpdatePatron = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, data }: { id: string; data: UpdatePatronDto }) =>
      patronService.update(id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["patrons"] });
      toast({
        title: "Success",
        description: "Patron updated successfully",
      });
    },
    onError: (error: any) => {
      toast({
        title: "Error",
        description: error.response?.data?.error || "Failed to update patron",
        variant: "destructive",
      });
    },
  });
};

export const useDeletePatron = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: string) => patronService.delete(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["patrons"] });
      toast({
        title: "Success",
        description: "Patron deleted successfully",
      });
    },
    onError: (error: any) => {
      toast({
        title: "Error",
        description: error.response?.data?.error || "Failed to delete patron",
        variant: "destructive",
      });
    },
  });
};
