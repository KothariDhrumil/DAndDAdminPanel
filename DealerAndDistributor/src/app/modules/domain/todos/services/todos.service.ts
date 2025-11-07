import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TODOS_API } from '../../../../core/helpers/routes/api-endpoints';
import { ApiResponse } from '../../../../core/models/interface/ApiResponse';
import { CreateTodoRequest, TodoItemDto, UpdateTodoRequest } from '../model/todo.model';

@Injectable({ providedIn: 'root' })
export class TodosService {
  private http = inject(HttpClient);

  list() {
    return this.http.get<ApiResponse<TodoItemDto[]>>(TODOS_API);
  }

  getById(id: string) {
    return this.http.get<ApiResponse<TodoItemDto>>(`${TODOS_API}/${id}`);
  }

  create(body: CreateTodoRequest, tenantId?: number) {
    const url = tenantId ? `${TODOS_API}` : TODOS_API;
    return this.http.post<ApiResponse<TodoItemDto>>(url, body);
  }

  update(body: UpdateTodoRequest) {
    return this.http.put<ApiResponse<TodoItemDto>>(TODOS_API, body);
  }

  delete(id: string) {
    return this.http.delete<ApiResponse<boolean>>(`${TODOS_API}/${id}`);
  }

  complete(id: string) {
    return this.http.put<ApiResponse<boolean>>(`${TODOS_API}/${id}/complete`, {});
  }
}
