export interface CustomerOrderCreateRequest {
  globalCustomerId: string;
  total: number;
}

export interface CustomerOrderResponse {
  id: string;
  globalCustomerId: string;
  total: number;
  createdAt?: string;
}
