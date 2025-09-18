import { HttpParams } from '@angular/common/http';
import { ApiRequest } from '../../models/interface/ApiRequest';

export function toApiRequestParams(request?: ApiRequest): HttpParams {
  let params = new HttpParams();
  if (!request) return params;
  if (request.filter) params = params.set('filter', request.filter);
  if (request.search) params = params.set('search', request.search);
  if (request.sort) {
    params = params.set('sortActive', request.sort.active);
    params = params.set('sortDirection', request.sort.direction);
  }
  if (request.page) {
    params = params.set('pageIndex', request.page.pageIndex.toString());
    params = params.set('pageSize', request.page.pageSize.toString());
  }
  return params;
}
