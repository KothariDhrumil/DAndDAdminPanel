import { PaginatedFilter } from './../../../core/models/PaginatedFilter';

export class UserParams implements PaginatedFilter {
  searchString: string = '';
  pageNumber: number =1;
  pageSize: number = 10;
  orderBy: string = 'created desc';
}
