export interface UserType {
  userTypeId: number;
  name: string;
  description?: string;
  createdAt?: string;
  updatedAt?: string;
}

export interface UpsertUserTypeRequest {
  name: string;
  description?: string;
}

export interface UserTypeApiResponse {
  isSuccess: boolean;
  data?: UserType | UserType[];
  error?: { description: string };
}
