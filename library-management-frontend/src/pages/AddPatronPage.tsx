import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useNavigate } from "react-router-dom";
import { useCreatePatron } from "@/hooks/usePatrons";
import { PatronType } from "@/types/patron";
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
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Users, Mail } from "lucide-react";
import { CreatePatronFormData, createPatronSchema } from "@/lib/validations";

export const AddPatronPage = () => {
  const navigate = useNavigate();
  const createPatronMutation = useCreatePatron();

  const form = useForm<CreatePatronFormData>({
    resolver: zodResolver(createPatronSchema),
    defaultValues: {
      name: "",
      email: "",
      type: undefined,
    },
  });

  const onSubmit = async (data: CreatePatronFormData) => {
    try {
      await createPatronMutation.mutateAsync(data);
      navigate("/patrons");
    } catch (error) {
      // Error handling is done by the mutation hook
    }
  };

  return (
    <div className="space-y-6">
      <div className="flex flex-col items-center justify-center space-y-4">
        <div>
          <h1 className="text-3xl text-center font-bold text-gray-900">
            Add New Patron
          </h1>
          <p className="text-gray-600 mt-2">Register a new library member</p>
        </div>
        <Card className="max-w-4xl">
          <CardHeader>
            <CardTitle className="flex items-center space-x-2">
              <Users className="h-5 w-5" />
              <span>Patron Information</span>
            </CardTitle>
            <CardDescription>
              Fill out all required fields to register a new patron in the
              library system.
            </CardDescription>
          </CardHeader>
          <CardContent>
            <Form {...form}>
              <form
                onSubmit={form.handleSubmit(onSubmit)}
                className="space-y-6"
              >
                <FormField
                  control={form.control}
                  name="name"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Full Name *</FormLabel>
                      <FormControl>
                        <Input
                          placeholder="Enter patron's full name"
                          {...field}
                        />
                      </FormControl>
                      <FormDescription>
                        The patron's complete legal name
                      </FormDescription>
                      <FormMessage />
                    </FormItem>
                  )}
                />

                <FormField
                  control={form.control}
                  name="email"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Email Address *</FormLabel>
                      <FormControl>
                        <div className="relative">
                          <Mail className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400 h-4 w-4" />
                          <Input
                            type="email"
                            placeholder="patron@example.com"
                            className="pl-10"
                            {...field}
                          />
                        </div>
                      </FormControl>
                      <FormDescription>
                        Email address for library communications and
                        notifications
                      </FormDescription>
                      <FormMessage />
                    </FormItem>
                  )}
                />

                <FormField
                  control={form.control}
                  name="type"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Patron Type *</FormLabel>
                      <Select
                        onValueChange={field.onChange}
                        defaultValue={field.value}
                      >
                        <FormControl>
                          <SelectTrigger>
                            <SelectValue placeholder="Select patron type" />
                          </SelectTrigger>
                        </FormControl>
                        <SelectContent>
                          <SelectItem value={PatronType.Regular}>
                            <div className="flex flex-col">
                              <span className="font-medium">Regular</span>
                              <span className="text-xs text-gray-500">
                                Standard library member with basic access
                              </span>
                            </div>
                          </SelectItem>
                          <SelectItem value={PatronType.Researcher}>
                            <div className="flex flex-col">
                              <span className="font-medium">Researcher</span>
                              <span className="text-xs text-gray-500">
                                Advanced access including restricted materials
                              </span>
                            </div>
                          </SelectItem>
                        </SelectContent>
                      </Select>
                      <FormDescription>
                        Patron type determines access level and borrowing
                        privileges
                      </FormDescription>
                      <FormMessage />
                    </FormItem>
                  )}
                />

                <div className="bg-blue-50 p-4 rounded-lg border border-blue-200">
                  <h4 className="text-sm font-medium text-blue-900 mb-2">
                    Patron Type Information
                  </h4>
                  <div className="text-xs text-blue-700 space-y-1">
                    <p>
                      <strong>Regular:</strong> Can borrow standard books and
                      materials
                    </p>
                    <p>
                      <strong>Researcher:</strong> Can access restricted
                      materials and special collections
                    </p>
                  </div>
                </div>

                <div className="flex justify-end space-x-4 pt-6">
                  <Button
                    type="button"
                    variant="outline"
                    onClick={() => navigate("/patrons")}
                  >
                    Cancel
                  </Button>
                  <Button
                    type="submit"
                    disabled={createPatronMutation.isPending}
                  >
                    {createPatronMutation.isPending
                      ? "Adding..."
                      : "Add Patron"}
                  </Button>
                </div>
              </form>
            </Form>
          </CardContent>
        </Card>
      </div>
    </div>
  );
};
