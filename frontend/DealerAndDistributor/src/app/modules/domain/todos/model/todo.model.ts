export enum Priority {
  Normal = 0,
  Low = 1,
  Medium = 2,
  High = 3,
  Top = 4,
}

export interface TodoItemDto {
  id: string; // GUID
  tenantId?: number; // multi-tenant context, may be provided by server
  description: string;
  dueDate?: string | null; // ISO string or null
  labels: string[];
  priority: Priority;
  isCompleted?: boolean;
  completedAtUtc?: string | null;
  createdAtUtc?: string;
  updatedAtUtc?: string;
}

export interface CreateTodoRequest {
  description: string;
  dueDate?: string | null;
  labels: string[];
  priority: Priority;
}

export interface UpdateTodoRequest {
  todoItemId: string; // GUID
  description?: string;
  dueDate?: string | null;
  labels?: string[];
  priority?: Priority;
}
