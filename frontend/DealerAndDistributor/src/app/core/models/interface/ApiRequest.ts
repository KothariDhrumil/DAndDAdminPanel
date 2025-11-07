export interface ApiRequest {
  filter?: string;
  search?: string;
  sort?: {
    active: string;
    direction: 'asc' | 'desc' | '';
  };
  page?: {
    pageIndex: number;
    pageSize: number;
  };
  // Add any additional fields as needed
}
