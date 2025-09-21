import { ChangeDetectionStrategy, Component, Inject, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { CreateTodoRequest, Priority, TodoItemDto, UpdateTodoRequest } from '../../model/todo.model';

export interface TodoDialogData {
  mode: 'create' | 'update';
  item?: TodoItemDto;
}

@Component({
  selector: 'app-todo-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatDialogModule, MatButtonModule, MatDatepickerModule, MatChipsModule, MatIconModule, MatSelectModule],
  templateUrl: './todo-dialog.component.html',
  styleUrls: ['./todo-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TodoDialogComponent {
  priorities = Object.values(Priority).filter(v => typeof v === 'number') as number[];

  form = new FormGroup({
    description: new FormControl<string>('', { nonNullable: true, validators: [Validators.required] }),
    dueDate: new FormControl<string | null>(null),
    labels: new FormControl<string[]>([], { nonNullable: true }),
    priority: new FormControl<Priority>(Priority.Medium, { nonNullable: true }),
  });

  labelsInput = new FormControl<string>('');

  constructor(
    private dialogRef: MatDialogRef<TodoDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: TodoDialogData
  ) {
    if (data?.item) {
      const { description, dueDate, labels, priority } = data.item;
      this.form.patchValue({
        description,
        dueDate: dueDate ?? null,
        labels: labels ?? [],
        priority
      });
    }
  }

  addLabel() {
    const value = (this.labelsInput.value || '').trim();
    if (!value) return;
    this.form.controls.labels.patchValue([...(this.form.controls.labels.value || []), value]);
    this.labelsInput.setValue('');
  }

  removeLabel(label: string) {
    this.form.controls.labels.patchValue((this.form.controls.labels.value || []).filter(l => l !== label));
  }

  save() {
    if (this.form.invalid) return;
    const raw = this.form.getRawValue();
    // Normalize dueDate to ISO string or null
    const dueDate = raw.dueDate ? (typeof raw.dueDate === 'string' ? raw.dueDate : new Date(raw.dueDate as any).toISOString()) : null;
    if (this.data?.mode === 'update' && this.data.item) {
      const payload: UpdateTodoRequest = {
        todoItemId: this.data.item.id,
        description: raw.description,
        dueDate,
        labels: raw.labels,
        priority: raw.priority
      };
      this.dialogRef.close(payload);
    } else {
      const payload: CreateTodoRequest = {
        description: raw.description,
        dueDate,
        labels: raw.labels,
        priority: raw.priority
      };
      this.dialogRef.close(payload);
    }
  }
  close() { this.dialogRef.close(); }
}
