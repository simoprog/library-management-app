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

export enum PatronType {
  Regular = "Regular",
  Researcher = "Researcher",
}

export interface CreatePatronDto {
  name: string;
  email: string;
  type: PatronType;
}

export interface UpdatePatronDto {
  name: string;
  email: string;
  type: PatronType;
}
