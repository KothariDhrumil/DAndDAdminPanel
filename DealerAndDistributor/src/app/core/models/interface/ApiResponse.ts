
export interface ApiResponse<T> {
  data: T;
  isSuccess: boolean;
  isFailure: boolean;
  error: ApiError;
}


export interface ApiError {
  code: string;
  description: string;
  type: number;
}