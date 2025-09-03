import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useNavigate, useParams } from "react-router-dom";
import { usePatron, useUpdatePatron } from "@/hooks/usePatrons";
import { PatronType } from "@/types/patron";
import { updatePatronSchema, UpdatePatronFormData } from "@/lib/validations";
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
import { Users, Mail, UserCheck, UserX } from "lucide-react";
import { LoadingSpinner } from "@/components/common/LoadingSpinner";
import { ErrorMessage } from "@/components/common/ErrorMessage";
import { Badge } from "@/components/ui/badge";
import { useEffect } from "react";

export const EditPatronPage = () => {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const { data: patron, isLoading, error } = usePatron(id!);
  const updatePatronMutation = useUpdatePatron();

  const form = useForm<UpdatePatronFormData>({
    resolver: zodResolver(updatePatronSchema),
    defaultValues: {
      name: patron?.name || "",
      email: patron?.email || "",
      type: patron?.type as PatronType,
    },
  });

  // Update form when patron data is loaded
  useEffect(() => {
    if (patron) {
      form.reset({
        name: patron.name,
        email: patron.email,
        type: patron.type as PatronType,
      });
    }
  }, [patron, form]);

  const onSubmit = async (data: UpdatePatronFormData) => {
    if (!id) return;

    try {
      await updatePatronMutation.mutateAsync({ id, data });
      navigate("/patrons");
    } catch {
      // Error handling is done by the mutation hook
    }
  };

  if (isLoading) return <LoadingSpinner />;
  if (error) return <ErrorMessage message="Failed to load patron details" />;
  if (!patron) return <ErrorMessage message="Patron not found" />;

  return (
    <div className="space-y-6">
      <div className="flex flex-col items-center justify-center space-y-4">
        <div>
          <h1 className="text-3xl text-center font-bold text-gray-900">
            Edit Patron
          </h1>
          <p className="text-gray-600 mt-2">Update patron information</p>
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        {/* Current Patron Info Card */}
        <Card className="lg:col-span-1">
          <CardHeader>
            <CardTitle className="flex items-center space-x-2">
              <Users className="h-5 w-5" />
              <span>Current Info</span>
            </CardTitle>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="flex items-center justify-between">
              <span className="text-sm font-medium">Status:</span>
              <div className="flex items-center space-x-2">
                {patron.isActive ? (
                  <>
                    <UserCheck className="h-4 w-4 text-green-600" />
                    <Badge
                      variant="default"
                      className="text-xs bg-green-100 text-green-800"
                    >
                      Active
                    </Badge>
                  </>
                ) : (
                  <>
                    <UserX className="h-4 w-4 text-red-600" />
                    <Badge variant="destructive" className="text-xs">
                      Inactive
                    </Badge>
                  </>
                )}
              </div>
            </div>

            <div className="space-y-2">
              <div className="flex justify-between text-sm">
                <span className="text-gray-600">Patron ID:</span>
                <span className="font-medium">{patron.patronId}</span>
              </div>
              <div className="flex justify-between text-sm">
                <span className="text-gray-600">Type:</span>
                <Badge
                  variant={
                    patron.type === "Researcher" ? "default" : "secondary"
                  }
                  className="text-xs"
                >
                  {patron.type}
                </Badge>
              </div>
              <div className="flex justify-between text-sm">
                <span className="text-gray-600">Outstanding Fees:</span>
                <span
                  className={`font-medium ${
                    patron.outstandingFeesAmount > 0
                      ? "text-red-600"
                      : "text-green-600"
                  }`}
                >
                  {patron.outstandingFeesCurrency}{" "}
                  {patron.outstandingFeesAmount.toFixed(2)}
                </span>
              </div>
              <div className="flex justify-between text-sm">
                <span className="text-gray-600">Joined:</span>
                <span className="font-medium">
                  {new Date(patron.createdAt).toLocaleDateString()}
                </span>
              </div>
            </div>
          </CardContent>
        </Card>

        {/* Edit Form */}
        <Card className="lg:col-span-2">
          <CardHeader>
            <CardTitle className="flex items-center space-x-2">
              <Users className="h-5 w-5" />
              <span>Edit Patron Information</span>
            </CardTitle>
            <CardDescription>
              Update the patron details in the library system.
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
                        value={field.value}
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
                    disabled={updatePatronMutation.isPending}
                  >
                    {updatePatronMutation.isPending
                      ? "Updating..."
                      : "Update Patron"}
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
