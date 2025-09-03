import { AlertCircle } from "lucide-react";
import { Alert, AlertDescription } from "@/components/ui/alert";

interface ErrorMessageProps {
  message: string;
  action?: React.ReactNode;
}

export const ErrorMessage = ({ message, action }: ErrorMessageProps) => {
  return (
    <Alert variant="destructive" className="max-w-md mx-auto">
      <AlertCircle className="h-4 w-4" />
      <AlertDescription className="ml-2">{message}</AlertDescription>
      {action && <div className="mt-2">{action}</div>}
    </Alert>
  );
};
