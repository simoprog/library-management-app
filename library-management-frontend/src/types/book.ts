export interface BookDto {
  id: string;
  bookId: string;
  title: string;
  author: string;
  isbn: string;
  status: "Available" | "OnHold" | "CheckedOut";
  isRestrictedAccess: boolean;
  currentHolderId?: string;
  currentBorrowerId?: string;
  holdExpiryDate?: string;
  dueDate?: string;
  createdAt: string;
}

export interface CreateBookDto {
  title: string;
  author: string;
  isbn: string;
  isRestrictedAccess: boolean;
}

export interface UpdateBookDto {
  title: string;
  author: string;
  isbn: string;
  isRestrictedAccess: boolean;
}

export interface PlaceHoldRequest {
  patronId: string;
}

export interface CheckoutRequest {
  patronId: string;
}
