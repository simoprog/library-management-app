import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { BookDto } from "@/types/book";
import { usePlaceBookOnHold } from "@/hooks/useBooks";
import { usePatrons } from "@/hooks/usePatrons";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Button } from "@/components/ui/button";
import { Card, CardContent } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { BookOpen, User, Calendar, Lock } from "lucide-react";

const holdBookSchema = z.object({
  patronId: z.string().min(1, "Please select a patron"),
});

type HoldBookFormData = z.infer<typeof holdBookSchema>;

interface HoldBookDialogProps {
  book: BookDto;
  open: boolean;
  onOpenChange: (open: boolean) => void;
}

export const HoldBookDialog = ({
  book,
  open,
  onOpenChange,
}: HoldBookDialogProps) => {
  const { data: patrons } = usePatrons();
  const placeHoldMutation = usePlaceBookOnHold();

  const form = useForm<HoldBookFormData>({
    resolver: zodResolver(holdBookSchema),
    defaultValues: {
      patronId: "",
    },
  });

  const onSubmit = async (data: HoldBookFormData) => {
    try {
      await placeHoldMutation.mutateAsync({
        bookId: book.bookId,
        data: { patronId: data.patronId },
      });
      onOpenChange(false);
      form.reset();
    } catch (error) {
      // Error is handled by the mutation hook
    }
  };

  const handleClose = () => {
    onOpenChange(false);
    form.reset();
  };

  // Filter patrons who can hold this book
  const eligiblePatrons = patrons?.filter((patron) => {
    if (!patron.isActive) return false;
    if (patron.outstandingFeesAmount > 0) return false;
    if (book.isRestrictedAccess && patron.type !== "Researcher") return false;
    return true;
  });

  return (
    <Dialog open={open} onOpenChange={handleClose}>
      <DialogContent className="sm:max-w-md">
        <DialogHeader>
          <DialogTitle className="flex items-center gap-2">
            <User className="h-5 w-5 text-blue-600" />
            Place Book on Hold
          </DialogTitle>
          <DialogDescription>
            Select a patron to place this book on hold for them.
          </DialogDescription>
        </DialogHeader>

        {/* Book Summary */}
        <Card className="border-dashed">
          <CardContent className="p-4">
            <div className="flex items-start gap-3">
              <BookOpen className="h-5 w-5 text-gray-500 mt-1" />
              <div className="flex-1">
                <h4 className="font-medium text-sm">{book.title}</h4>
                <p className="text-xs text-gray-600">by {book.author}</p>
                <div className="flex items-center gap-2 mt-2">
                  <Badge variant="outline" className="text-xs">
                    {book.status}
                  </Badge>
                  {book.isRestrictedAccess && (
                    <Badge
                      variant="outline"
                      className="text-xs text-orange-600"
                    >
                      <Lock className="mr-1 h-3 w-3" />
                      Restricted
                    </Badge>
                  )}
                </div>
              </div>
            </div>
          </CardContent>
        </Card>

        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
            <FormField
              control={form.control}
              name="patronId"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Select Patron</FormLabel>
                  <Select
                    onValueChange={field.onChange}
                    defaultValue={field.value}
                  >
                    <FormControl>
                      <SelectTrigger>
                        <SelectValue placeholder="Choose a patron..." />
                      </SelectTrigger>
                    </FormControl>
                    <SelectContent>
                      {eligiblePatrons?.map((patron) => (
                        <SelectItem
                          key={patron.patronId}
                          value={patron.patronId}
                        >
                          <div className="flex items-center justify-between w-full">
                            <span>{patron.name}</span>
                            <Badge variant="secondary" className="ml-2 text-xs">
                              {patron.type}
                            </Badge>
                          </div>
                        </SelectItem>
                      ))}
                      {(!eligiblePatrons || eligiblePatrons.length === 0) && (
                        <div className="px-2 py-1 text-sm text-gray-500">
                          No eligible patrons available
                        </div>
                      )}
                    </SelectContent>
                  </Select>
                  <FormMessage />
                  {book.isRestrictedAccess && (
                    <p className="text-xs text-orange-600 flex items-center gap-1">
                      <Lock className="h-3 w-3" />
                      Only researchers can hold restricted books
                    </p>
                  )}
                </FormItem>
              )}
            />

            <div className="bg-blue-50 p-3 rounded-lg">
              <div className="flex items-center gap-2 text-sm text-blue-800">
                <Calendar className="h-4 w-4" />
                <span className="font-medium">Hold Duration: 7 days</span>
              </div>
              <p className="text-xs text-blue-700 mt-1">
                The hold will expire automatically if not checked out within 7
                days.
              </p>
            </div>

            <DialogFooter>
              <Button
                type="button"
                variant="outline"
                onClick={handleClose}
                disabled={placeHoldMutation.isPending}
              >
                Cancel
              </Button>
              <Button
                type="submit"
                disabled={
                  placeHoldMutation.isPending || !eligiblePatrons?.length
                }
                className="min-w-24"
              >
                {placeHoldMutation.isPending ? "Placing..." : "Place Hold"}
              </Button>
            </DialogFooter>
          </form>
        </Form>
      </DialogContent>
    </Dialog>
  );
};
