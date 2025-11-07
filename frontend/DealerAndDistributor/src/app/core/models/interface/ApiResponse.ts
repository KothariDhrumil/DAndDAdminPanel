export interface ApiResponseBase {
  isSuccess: boolean;
  isFailure: boolean;
  error: ApiError;

}


export interface ApiResponse<T> extends ApiResponseBase {
  data: T;
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