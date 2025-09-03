import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { BookDto } from "@/types/book";
import { useCheckoutBook } from "@/hooks/useBooks";
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
import {
  BookOpen,
  Calendar,
  AlertCircle,
  CheckCircle2,
  Lock,
} from "lucide-react";

const checkoutBookSchema = z.object({
  patronId: z.string().min(1, "Please select a patron"),
});

type CheckoutBookFormData = z.infer<typeof checkoutBookSchema>;

interface CheckoutBookDialogProps {
  book: BookDto;
  open: boolean;
  onOpenChange: (open: boolean) => void;
}

export const CheckoutBookDialog = ({
  book,
  open,
  onOpenChange,
}: CheckoutBookDialogProps) => {
  const { data: patrons } = usePatrons();
  const checkoutMutation = useCheckoutBook();

  const form = useForm<CheckoutBookFormData>({
    resolver: zodResolver(checkoutBookSchema),
    defaultValues: {
      patronId: book.currentHolderId || "",
    },
  });

  const onSubmit = async (data: CheckoutBookFormData) => {
    try {
      await checkoutMutation.mutateAsync({
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

  // Filter patrons who can checkout this book
  const eligiblePatrons = patrons?.filter((patron) => {
    if (!patron.isActive) return false;
    if (patron.outstandingFeesAmount > 0) return false;
    if (book.isRestrictedAccess && patron.type !== "Researcher") return false;
    return true;
  });

  // Calculate due date (14 days from now)
  const dueDate = new Date();
  dueDate.setDate(dueDate.getDate() + 14);

  const isOnHold = book.status === "OnHold";
  const holdPatron =
    isOnHold && book.currentHolderId
      ? patrons?.find((p) => p.patronId === book.currentHolderId)
      : null;

  return (
    <Dialog open={open} onOpenChange={handleClose}>
      <DialogContent className="sm:max-w-md">
        <DialogHeader>
          <DialogTitle className="flex items-center gap-2">
            <BookOpen className="h-5 w-5 text-green-600" />
            Checkout Book
          </DialogTitle>
          <DialogDescription>
            Check out this book to a patron for borrowing.
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

        {/* Hold Status Warning */}
        {isOnHold && holdPatron && (
          <div className="bg-amber-50 border border-amber-200 p-3 rounded-lg">
            <div className="flex items-center gap-2 text-sm text-amber-800">
              <AlertCircle className="h-4 w-4" />
              <span className="font-medium">Book is on hold</span>
            </div>
            <p className="text-xs text-amber-700 mt-1">
              This book is currently on hold for{" "}
              <strong>{holdPatron.name}</strong>. Only they can check it out, or
              you can override the hold.
            </p>
          </div>
        )}

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
                            <div className="flex items-center gap-1">
                              {patron.patronId === book.currentHolderId && (
                                <CheckCircle2 className="h-3 w-3 text-green-600" />
                              )}
                              <Badge
                                variant={
                                  patron.patronId === book.currentHolderId
                                    ? "default"
                                    : "secondary"
                                }
                                className="text-xs"
                              >
                                {patron.type}
                              </Badge>
                            </div>
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
                      Only researchers can checkout restricted books
                    </p>
                  )}
                </FormItem>
              )}
            />

            <div className="bg-green-50 p-3 rounded-lg">
              <div className="flex items-center gap-2 text-sm text-green-800">
                <Calendar className="h-4 w-4" />
                <span className="font-medium">
                  Due Date: {dueDate.toLocaleDateString()}
                </span>
              </div>
              <p className="text-xs text-green-700 mt-1">
                Standard checkout period is 14 days. Late returns may incur
                fees.
              </p>
            </div>

            <DialogFooter>
              <Button
                type="button"
                variant="outline"
                onClick={handleClose}
                disabled={checkoutMutation.isPending}
              >
                Cancel
              </Button>
              <Button
                type="submit"
                disabled={
                  checkoutMutation.isPending || !eligiblePatrons?.length
                }
                className="min-w-24"
              >
                {checkoutMutation.isPending ? "Checking Out..." : "Checkout"}
              </Button>
            </DialogFooter>
          </form>
        </Form>
      </DialogContent>
    </Dialog>
  );
};
