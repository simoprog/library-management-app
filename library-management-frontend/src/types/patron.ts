export interface PatronDto {
  id: string;
  patronId: string;
  name: string;
  email: string;
  type: "Regular" | "Researcher";
  outstandingFeesAmount: number;
  outstandingFeesCurrency: string;
  isActive: boolean;
  createdAt: string;
}

export interface CreatePatronDto {
  name: string;
  email: string;
  type: "Regular" | "Researcher";
}

export interface UpdatePatronDto {
  name: string;
  email: string;
  type: "Regular" | "Researcher";
}
