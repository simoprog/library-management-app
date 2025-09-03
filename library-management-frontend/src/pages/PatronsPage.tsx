import { useState } from "react";
import { usePatrons } from "@/hooks/usePatrons";
import { Input } from "@/components/ui/input";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import {
  Search,
  Filter,
  Users,
  Mail,
  DollarSign,
  UserCheck,
  UserX,
} from "lucide-react";
import { ErrorMessage } from "@/components/common/ErrorMessage";
import { LoadingSpinner } from "@/components/common/LoadingSpinner";
import { PatronDto } from "@/types/patron";
import { Link } from "react-router-dom";

export const PatronsPage = () => {
  const { data: patrons, isLoading, error } = usePatrons();
  const [searchTerm, setSearchTerm] = useState("");
  const [statusFilter, setStatusFilter] = useState<string>("all");
  const [typeFilter, setTypeFilter] = useState<string>("all");

  if (isLoading) return <LoadingSpinner />;
  if (error) return <ErrorMessage message="Failed to load patrons" />;
  if (!patrons) return null;

  const filteredPatrons = patrons.filter((patron: PatronDto) => {
    const matchesSearch =
      patron.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
      patron.email.toLowerCase().includes(searchTerm.toLowerCase()) ||
      patron.patronId.includes(searchTerm);

    const matchesStatus =
      statusFilter === "all" ||
      (statusFilter === "active" && patron.isActive) ||
      (statusFilter === "inactive" && !patron.isActive);

    const matchesType =
      typeFilter === "all" || patron.type.toLowerCase() === typeFilter;

    return matchesSearch && matchesStatus && matchesType;
  });

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <h1 className="text-3xl font-bold text-gray-900">Patrons</h1>
        <Link to="/patrons/new">
          <Button>Add New Patron</Button>
        </Link>
      </div>

      <div className="flex items-center space-x-4">
        <div className="relative flex-1 max-w-md">
          <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400 h-4 w-4" />
          <Input
            placeholder="Search patrons..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            className="pl-10"
          />
        </div>
        <Select value={statusFilter} onValueChange={setStatusFilter}>
          <SelectTrigger className="w-40">
            <Filter className="mr-2 h-4 w-4" />
            <SelectValue />
          </SelectTrigger>
          <SelectContent>
            <SelectItem value="all">All Status</SelectItem>
            <SelectItem value="active">Active</SelectItem>
            <SelectItem value="inactive">Inactive</SelectItem>
          </SelectContent>
        </Select>
        <Select value={typeFilter} onValueChange={setTypeFilter}>
          <SelectTrigger className="w-40">
            <Users className="mr-2 h-4 w-4" />
            <SelectValue />
          </SelectTrigger>
          <SelectContent>
            <SelectItem value="all">All Types</SelectItem>
            <SelectItem value="regular">Regular</SelectItem>
            <SelectItem value="researcher">Researcher</SelectItem>
          </SelectContent>
        </Select>
      </div>

      {filteredPatrons.length === 0 ? (
        <div className="text-center py-12">
          <Users className="mx-auto h-12 w-12 text-gray-400" />
          <h3 className="mt-2 text-sm font-medium text-gray-900">
            No patrons found
          </h3>
          <p className="mt-1 text-sm text-gray-500">
            {searchTerm || statusFilter !== "all" || typeFilter !== "all"
              ? "Try adjusting your search or filter criteria."
              : "Get started by adding a new patron."}
          </p>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {filteredPatrons.map((patron: PatronDto) => (
            <Card key={patron.id} className="hover:shadow-md transition-shadow">
              <CardHeader className="pb-3">
                <div className="flex items-start justify-between">
                  <div className="space-y-1">
                    <CardTitle className="text-lg">{patron.name}</CardTitle>
                    <CardDescription className="text-sm">
                      ID: {patron.patronId}
                    </CardDescription>
                  </div>
                  <div className="flex items-center space-x-2">
                    <Badge
                      variant={
                        patron.type === "Researcher" ? "default" : "secondary"
                      }
                      className="text-xs"
                    >
                      {patron.type}
                    </Badge>
                    {patron.isActive ? (
                      <UserCheck className="h-4 w-4 text-green-600" />
                    ) : (
                      <UserX className="h-4 w-4 text-red-600" />
                    )}
                  </div>
                </div>
              </CardHeader>
              <CardContent className="space-y-4">
                <div className="flex items-center space-x-2 text-sm text-gray-600">
                  <Mail className="h-4 w-4" />
                  <span className="truncate">{patron.email}</span>
                </div>

                <div className="flex items-center justify-between">
                  <div className="flex items-center space-x-2 text-sm">
                    <DollarSign className="h-4 w-4 text-gray-500" />
                    <span>Outstanding Fees:</span>
                  </div>
                  <span
                    className={`text-sm font-medium ${
                      patron.outstandingFeesAmount > 0
                        ? "text-red-600"
                        : "text-green-600"
                    }`}
                  >
                    {patron.outstandingFeesCurrency}{" "}
                    {patron.outstandingFeesAmount.toFixed(2)}
                  </span>
                </div>

                <div className="flex items-center justify-between text-xs text-gray-500">
                  <span>Joined:</span>
                  <span>{new Date(patron.createdAt).toLocaleDateString()}</span>
                </div>

                <div className="pt-2 border-t">
                  <div className="flex space-x-2">
                    <Button variant="outline" size="sm" className="flex-1">
                      View Details
                    </Button>
                    <Button variant="outline" size="sm" className="flex-1">
                      Edit
                    </Button>
                  </div>
                </div>
              </CardContent>
            </Card>
          ))}
        </div>
      )}
    </div>
  );
};
