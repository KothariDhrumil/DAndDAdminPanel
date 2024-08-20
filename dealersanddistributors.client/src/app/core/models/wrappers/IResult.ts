export interface IResult<T>
{
  succeeded: boolean;
  messages: string[];
  data: T | T[];
  status : IStatus;
}

export interface IStatus{
    requestId: string,
    statusCode: number,
    statusDesc: string,
    httpStatusCode: number,
    httpStatusDesc: string
}
