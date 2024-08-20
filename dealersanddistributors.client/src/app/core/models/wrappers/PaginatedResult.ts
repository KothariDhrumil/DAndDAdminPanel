import { IResult, IStatus } from "./IResult";
export class PaginatedResult<T> implements IResult<T> {
  succeeded: boolean;
  messages: string[];
  data: T[];
  source: string;
  exception: string;
  errorCode: number;
  currentPage: number;
  pageSize: number;
  totalPages: number;
  totalCount: number;

  constructor() {
    this.succeeded = false;
    this.messages = [];
    this.data = [];
    this.source = '';
    this.exception = '';
    this.errorCode = 0;
    this.currentPage = 0;
    this.pageSize = 0;
    this.totalPages = 0;
    this.totalCount = 0;
  }
  status !: IStatus;
}
