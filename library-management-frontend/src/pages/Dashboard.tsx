import { useBooks } from "@/hooks/useBooks";
import { usePatrons } from "@/hooks/usePatrons";
import { BookOpen, Users, TrendingUp, AlertCircle } from "lucide-react";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { LoadingSpinner } from "@/components/common/LoadingSpinner";

export const Dashboard = () => {
  const { data: books, isLoading: booksLoading } = useBooks();
  const { data: patrons, isLoading: patronsLoading } = usePatrons();

  if (booksLoading || patronsLoading) {
    return <LoadingSpinner />;
  }

  const totalBooks = books?.length || 0;
  const availableBooks =
    books?.filter((book) => book.status.toLowerCase() === "available").length ||
    0;
  const checkedOutBooks =
    books?.filter((book) => book.status.toLowerCase() === "checkedout")
      .length || 0;
  const onHoldBooks =
    books?.filter((book) => book.status.toLowerCase() === "onhold").length || 0;
  const totalPatrons = patrons?.length || 0;

  const stats = [
    {
      title: "Total Books",
      value: totalBooks,
      description: "Books in the library",
      icon: BookOpen,
      color: "text-blue-600",
      bgColor: "bg-blue-100",
    },
    {
      title: "Available Books",
      value: availableBooks,
      description: "Ready to be borrowed",
      icon: TrendingUp,
      color: "text-green-600",
      bgColor: "bg-green-100",
    },
    {
      title: "Checked Out",
      value: checkedOutBooks,
      description: "Currently borrowed",
      icon: AlertCircle,
      color: "text-orange-600",
      bgColor: "bg-orange-100",
    },
    {
      title: "Total Patrons",
      value: totalPatrons,
      description: "Registered library members",
      icon: Users,
      color: "text-purple-600",
      bgColor: "bg-purple-100",
    },
  ];

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-3xl font-bold text-gray-900">Dashboard</h1>
        <p className="text-gray-600 mt-2">
          Welcome to your Library Management System
        </p>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        {stats.map((stat) => {
          const Icon = stat.icon;
          return (
            <Card key={stat.title}>
              <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                <CardTitle className="text-sm font-medium">
                  {stat.title}
                </CardTitle>
                <div className={`${stat.bgColor} p-2 rounded-md`}>
                  <Icon className={`h-4 w-4 ${stat.color}`} />
                </div>
              </CardHeader>
              <CardContent>
                <div className="text-2xl font-bold">{stat.value}</div>
                <CardDescription className="text-xs text-muted-foreground">
                  {stat.description}
                </CardDescription>
              </CardContent>
            </Card>
          );
        })}
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <Card>
          <CardHeader>
            <CardTitle>Book Status Overview</CardTitle>
            <CardDescription>
              Current status distribution of all books
            </CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="space-y-3">
              <div className="flex items-center justify-between">
                <span className="text-sm text-gray-600">Available</span>
                <span className="text-sm font-medium">{availableBooks}</span>
              </div>
              <div className="w-full bg-gray-200 rounded-full h-2">
                <div
                  className="bg-green-500 h-2 rounded-full"
                  style={{
                    width: `${
                      totalBooks > 0 ? (availableBooks / totalBooks) * 100 : 0
                    }%`,
                  }}
                ></div>
              </div>
            </div>

            <div className="space-y-3">
              <div className="flex items-center justify-between">
                <span className="text-sm text-gray-600">Checked Out</span>
                <span className="text-sm font-medium">{checkedOutBooks}</span>
              </div>
              <div className="w-full bg-gray-200 rounded-full h-2">
                <div
                  className="bg-orange-500 h-2 rounded-full"
                  style={{
                    width: `${
                      totalBooks > 0 ? (checkedOutBooks / totalBooks) * 100 : 0
                    }%`,
                  }}
                ></div>
              </div>
            </div>

            <div className="space-y-3">
              <div className="flex items-center justify-between">
                <span className="text-sm text-gray-600">On Hold</span>
                <span className="text-sm font-medium">{onHoldBooks}</span>
              </div>
              <div className="w-full bg-gray-200 rounded-full h-2">
                <div
                  className="bg-blue-500 h-2 rounded-full"
                  style={{
                    width: `${
                      totalBooks > 0 ? (onHoldBooks / totalBooks) * 100 : 0
                    }%`,
                  }}
                ></div>
              </div>
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle>Quick Actions</CardTitle>
            <CardDescription>Common tasks and shortcuts</CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="grid grid-cols-2 gap-4">
              <a
                href="/books/new"
                className="flex flex-col items-center p-4 bg-blue-50 rounded-lg hover:bg-blue-100 transition-colors"
              >
                <BookOpen className="h-8 w-8 text-blue-600 mb-2" />
                <span className="text-sm font-medium text-blue-900">
                  Add Book
                </span>
              </a>
              <a
                href="/patrons/new"
                className="flex flex-col items-center p-4 bg-purple-50 rounded-lg hover:bg-purple-100 transition-colors"
              >
                <Users className="h-8 w-8 text-purple-600 mb-2" />
                <span className="text-sm font-medium text-purple-900">
                  Add Patron
                </span>
              </a>
            </div>
          </CardContent>
        </Card>
      </div>
    </div>
  );
};
