import { IResult } from "./IResult";

export class Result<T> implements IResult<T> {
  succeeded: boolean = false;
  messages: string[] = [];
  data !: T;
  source: string = ''; // Add an initializer for the 'source' property
  exception: string = '';
  errorCode: number = 0;
  status: Status = new Status(); // Initialize the 'status' property

  constructor() {
    this.succeeded = false;
  }
}

export class Status {
  requestId: string = '';
  statusCode!: number;
  statusDesc: string = '';
  httpStatusCode!: number;
  httpStatusDesc: string = '';
}
