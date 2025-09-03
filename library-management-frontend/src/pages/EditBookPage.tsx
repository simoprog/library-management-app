import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useNavigate, useParams } from "react-router-dom";
import { useBook, useUpdateBook } from "@/hooks/useBooks";
import { updateBookSchema, UpdateBookFormData } from "@/lib/validations";
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Checkbox } from "@/components/ui/checkbox";
import { BookOpen, ArrowLeft } from "lucide-react";
import { Link } from "react-router-dom";
import { LoadingSpinner } from "@/components/common/LoadingSpinner";
import { ErrorMessage } from "@/components/common/ErrorMessage";
import { useEffect } from "react";

export const EditBookPage = () => {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const { data: book, isLoading, error } = useBook(id!);
  const updateBookMutation = useUpdateBook();

  const form = useForm<UpdateBookFormData>({
    resolver: zodResolver(updateBookSchema),
    defaultValues: {
      title: "",
      author: "",
      isbn: "",
      isRestrictedAccess: false,
    },
  });

  // Update form when book data is loaded
  useEffect(() => {
    if (book) {
      form.reset({
        title: book.title,
        author: book.author,
        isbn: book.isbn,
        isRestrictedAccess: book.isRestrictedAccess,
      });
    }
  }, [book, form]);

  const onSubmit = async (data: UpdateBookFormData) => {
    if (!id) return;

    try {
      await updateBookMutation.mutateAsync({ id, data });
      navigate("/books");
    } catch {
      // Error handling is done by the mutation hook
    }
  };

  if (isLoading) return <LoadingSpinner />;
  if (error) return <ErrorMessage message="Failed to load book details" />;
  if (!book) return <ErrorMessage message="Book not found" />;

  return (
    <div className="space-y-6">
      <div className="flex items-center space-x-4">
        <Link to="/books">
          <Button variant="outline" size="sm">
            <ArrowLeft className="h-4 w-4 mr-2" />
            Back to Books
          </Button>
        </Link>
        <div>
          <h1 className="text-3xl font-bold text-gray-900">Edit Book</h1>
          <p className="text-gray-600 mt-2">Update the book information</p>
        </div>
      </div>

      <Card className="max-w-2xl">
        <CardHeader>
          <CardTitle className="flex items-center space-x-2">
            <BookOpen className="h-5 w-5" />
            <span>Book Information</span>
          </CardTitle>
          <CardDescription>
            Update the book details in the library system.
          </CardDescription>
        </CardHeader>
        <CardContent>
          <Form {...form}>
            <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6">
              <FormField
                control={form.control}
                name="title"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Title *</FormLabel>
                    <FormControl>
                      <Input placeholder="Enter book title" {...field} />
                    </FormControl>
                    <FormDescription>
                      The full title of the book
                    </FormDescription>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="author"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Author *</FormLabel>
                    <FormControl>
                      <Input placeholder="Enter author name" {...field} />
                    </FormControl>
                    <FormDescription>The author's full name</FormDescription>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="isbn"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>ISBN *</FormLabel>
                    <FormControl>
                      <Input
                        placeholder="Enter ISBN (10 or 13 digits)"
                        {...field}
                      />
                    </FormControl>
                    <FormDescription>
                      International Standard Book Number (with or without
                      hyphens)
                    </FormDescription>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="isRestrictedAccess"
                render={({ field }) => (
                  <FormItem className="flex flex-row items-start space-x-3 space-y-0 rounded-md border p-4">
                    <FormControl>
                      <Checkbox
                        checked={field.value}
                        onCheckedChange={field.onChange}
                      />
                    </FormControl>
                    <div className="space-y-1 leading-none">
                      <FormLabel>Restricted Access</FormLabel>
                      <FormDescription>
                        Check this if the book requires special permissions or
                        is only available to researchers.
                      </FormDescription>
                    </div>
                  </FormItem>
                )}
              />

              <div className="bg-amber-50 p-4 rounded-lg border border-amber-200">
                <h4 className="text-sm font-medium text-amber-900 mb-2">
                  Book Status Information
                </h4>
                <div className="text-xs text-amber-700 space-y-1">
                  <p>
                    <strong>Current Status:</strong> {book.status}
                  </p>
                  <p>
                    <strong>Book ID:</strong> {book.bookId}
                  </p>
                  <p>
                    <strong>Created:</strong>{" "}
                    {new Date(book.createdAt).toLocaleDateString()}
                  </p>
                  {book.currentHolderId && (
                    <p>
                      <strong>Current Holder:</strong> {book.currentHolderId}
                    </p>
                  )}
                  {book.currentBorrowerId && (
                    <p>
                      <strong>Current Borrower:</strong>{" "}
                      {book.currentBorrowerId}
                    </p>
                  )}
                </div>
              </div>

              <div className="flex justify-end space-x-4 pt-6">
                <Button
                  type="button"
                  variant="outline"
                  onClick={() => navigate("/books")}
                >
                  Cancel
                </Button>
                <Button type="submit" disabled={updateBookMutation.isPending}>
                  {updateBookMutation.isPending ? "Updating..." : "Update Book"}
                </Button>
              </div>
            </form>
          </Form>
        </CardContent>
      </Card>
    </div>
  );
};
