
export interface ApiResponse<T> {
  data: T;
  isSuccess: boolean;
  isFailure: boolean;
  error: ApiError;
}


export interface PaginatedApiResponse<T> extends ApiResponse<T> {
  pageNumber: number;
  pageSize: number;
  totalRecords: number;
}


export interface ApiError {
  code: string;
  description: string;
  type: number;
}