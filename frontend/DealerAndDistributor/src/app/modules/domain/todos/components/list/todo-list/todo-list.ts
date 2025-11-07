import { ChangeDetectionStrategy, Component, computed, inject, Input, OnChanges, signal, SimpleChanges } from '@angular/core';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { BreadcrumbComponent } from "@core/shared/components/breadcrumb/breadcrumb.component";
import { GenericTableComponent } from "@core/shared/components/generic-table/generic-table.component";
import { ColumnDefinition, RowAction, TableConfig, TableEventArgs } from '@core/shared/components/generic-table/generic-table.model';
import { TodoItemDto, Priority, CreateTodoRequest, UpdateTodoRequest } from '../../../model/todo.model';
import { TodosService } from '../../../services/todos.service';
import { TodoDialogComponent } from '../../todo-dialog/todo-dialog.component';

@Component({
  selector: 'app-todo-list',
  imports: [BreadcrumbComponent, GenericTableComponent, MatDialogModule],
  templateUrl: './todo-list.html',
  styleUrl: './todo-list.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TodoList implements OnChanges {
  @Input({ required: true }) tenantId!: number;

  private svc = inject(TodosService);
  private dialog = inject(MatDialog);

  private _rows = signal<TodoItemDto[]>([]);
  rows = computed(() => this._rows());

  columns: ColumnDefinition[] = [
    { def: 'id', label: 'Id', type: 'text', sortable: true },
    { def: 'description', label: 'Description', type: 'text', sortable: true },
    { def: 'dueDate', label: 'Due Date', type: 'date', sortable: true },
    { def: 'labels', label: 'Labels', type: 'text' },
    { def: 'priorityText', label: 'Priority', type: 'text', sortable: true },
    { def: 'isCompleted', label: 'Completed', type: 'check' },
    { def: 'actions', label: 'Actions', type: 'actionBtn' }
  ];

  rowActions: RowAction[] = [
    { name: 'complete', icon: 'check', tooltip: 'Complete Todo', color: 'primary' }
  ];

  config: TableConfig = {
    title: 'Todos',
    enableAdd: true,
    enableEdit: true,
    enableDelete: true,
    enableRefresh: true,
    enableColumnHide: true,
    enableExport: false,
    enableSelection: false,
    pageSize: 10,
    pageSizeOptions: [5, 10, 25, 50, 100]
  };

  ngOnChanges(changes: SimpleChanges): void {
    this.load();
  }

  ngOnInit(): void {
    this.load();
  }

  load() {
    this.svc.list().subscribe(res => {
      if (res?.isSuccess && Array.isArray(res.data)) {
        // Map numeric priority to text for display convenience
        const rows = (res.data || []).map(r => ({
          ...r,
          priorityText: this.priorityToText(r.priority)
        } as any));
        this._rows.set(rows as TodoItemDto[]);
      } else {
        this._rows.set([]);
      }
    });
  }

  private priorityToText(p: Priority | number | undefined): string {
    switch (p) {
      case Priority.Normal: return 'Normal';
      case Priority.Low: return 'Low';
      case Priority.Medium: return 'Medium';
      case Priority.High: return 'High';
      case Priority.Top: return 'Top';
      default: return '';
    }
  }

  onTableEvent(e: TableEventArgs) {
    switch (e.type) {
      case 'refresh':
        this.load();
        break;
      case 'add':
        this.openCreate();
        break;
      case 'edit':
        if (e.data) this.openUpdate(e.data as TodoItemDto);
        break;
      case 'delete':
        if (e.data?.id) this.delete(e.data.id);
        break;
      case 'custom':
        if (e.action === 'complete' && e.data?.id) this.complete(e.data.id);
        break;
    }
  }

  openCreate() {
    const ref = this.dialog.open(TodoDialogComponent, { width: '600px', data: { mode: 'create', tenantId: this.tenantId } });
    ref.afterClosed().subscribe((changed: boolean) => { if (changed) this.load(); });
  }

  openUpdate(item: TodoItemDto) {
    const ref = this.dialog.open(TodoDialogComponent, { width: '600px', data: { mode: 'update', item } });
    ref.afterClosed().subscribe((changed: boolean) => { if (changed) this.load(); });
  }

  delete(id: string) {
    this.svc.delete(id).subscribe({ next: r => { if (r?.isSuccess) this.load(); } });
  }

  complete(id: string) {
    this.svc.complete(id).subscribe({ next: r => { if (r?.isSuccess) this.load(); } });
  }

}
