import { useState } from "react";
import { Link } from "react-router-dom";
import { BookDto } from "@/types/book";
import {
  Card,
  CardContent,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import {
  MoreVertical,
  Edit,
  Trash2,
  BookOpen,
  Calendar,
  User,
  Lock,
} from "lucide-react";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { HoldBookDialog } from "./HoldBookDialog";
import { CheckoutBookDialog } from "./CheckoutBookDialog";

interface BookCardProps {
  book: BookDto;
  onDelete: (id: string) => void;
}

export const BookCard = ({ book, onDelete }: BookCardProps) => {
  const [showHoldDialog, setShowHoldDialog] = useState(false);
  const [showCheckoutDialog, setShowCheckoutDialog] = useState(false);

  const getStatusColor = (status: string) => {
    switch (status.toLowerCase()) {
      case "available":
        return "bg-green-100 text-green-800";
      case "onhold":
        return "bg-yellow-100 text-yellow-800";
      case "checkedout":
        return "bg-red-100 text-red-800";
      default:
        return "bg-gray-100 text-gray-800";
    }
  };

  const formatDate = (dateString?: string) => {
    if (!dateString) return null;
    return new Date(dateString).toLocaleDateString();
  };

  return (
    <>
      <Card className="h-full flex flex-col hover:shadow-lg transition-shadow">
        <CardHeader>
          <div className="flex items-start justify-between">
            <div className="flex-1">
              <CardTitle className="text-lg line-clamp-2">
                {book.title}
              </CardTitle>
              <p className="text-sm text-gray-600 mt-1">by {book.author}</p>
              <p className="text-xs text-gray-500 mt-1">ISBN: {book.isbn}</p>
            </div>
            <DropdownMenu>
              <DropdownMenuTrigger asChild>
                <Button variant="ghost" size="sm" className="h-8 w-8 p-0">
                  <MoreVertical className="h-4 w-4" />
                </Button>
              </DropdownMenuTrigger>
              <DropdownMenuContent align="end">
                <DropdownMenuItem asChild>
                  <Link to={`/books/edit/${book.bookId}`}>
                    <Edit className="mr-2 h-4 w-4" />
                    Edit
                  </Link>
                </DropdownMenuItem>
                <DropdownMenuItem
                  onClick={() => onDelete(book.bookId)}
                  className="text-red-600"
                >
                  <Trash2 className="mr-2 h-4 w-4" />
                  Delete
                </DropdownMenuItem>
              </DropdownMenuContent>
            </DropdownMenu>
          </div>
        </CardHeader>

        <CardContent className="flex-1">
          <div className="space-y-3">
            <div className="flex items-center justify-between">
              <Badge className={getStatusColor(book.status)}>
                {book.status}
              </Badge>
              {book.isRestrictedAccess && (
                <Badge
                  variant="outline"
                  className="text-orange-600 border-orange-600"
                >
                  <Lock className="mr-1 h-3 w-3" />
                  Restricted
                </Badge>
              )}
            </div>

            {book.holdExpiryDate && (
              <div className="flex items-center text-sm text-gray-600">
                <Calendar className="mr-2 h-4 w-4" />
                Hold expires: {formatDate(book.holdExpiryDate)}
              </div>
            )}

            {book.dueDate && (
              <div className="flex items-center text-sm text-gray-600">
                <Calendar className="mr-2 h-4 w-4" />
                Due: {formatDate(book.dueDate)}
              </div>
            )}
          </div>
        </CardContent>

        <CardFooter className="pt-4">
          <div className="flex gap-2 w-full">
            {book.status === "Available" && (
              <>
                <Button
                  variant="outline"
                  size="sm"
                  onClick={() => setShowHoldDialog(true)}
                  className="flex-1"
                >
                  <User className="mr-2 h-4 w-4" />
                  Hold
                </Button>
                <Button
                  size="sm"
                  onClick={() => setShowCheckoutDialog(true)}
                  className="flex-1"
                >
                  <BookOpen className="mr-2 h-4 w-4" />
                  Checkout
                </Button>
              </>
            )}
            {book.status === "CheckedOut" && (
              <Button variant="outline" size="sm" className="w-full">
                Return Book
              </Button>
            )}
          </div>
        </CardFooter>
      </Card>

      <HoldBookDialog
        book={book}
        open={showHoldDialog}
        onOpenChange={setShowHoldDialog}
      />

      <CheckoutBookDialog
        book={book}
        open={showCheckoutDialog}
        onOpenChange={setShowCheckoutDialog}
      />
    </>
  );
};
