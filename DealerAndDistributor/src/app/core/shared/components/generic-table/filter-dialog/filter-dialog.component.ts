import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { ReactiveFormsModule, FormGroup } from '@angular/forms';
import { ColumnDefinition } from '../generic-table.model';

@Component({
  selector: 'app-filter-dialog',
  standalone: true,
  imports: [CommonModule, MatDialogModule, ReactiveFormsModule],
  templateUrl: './filter-dialog.component.html',
  styleUrls: ['./filter-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FilterDialogComponent {
  form!: FormGroup;
  constructor(
    private dialogRef: MatDialogRef<FilterDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public columns: ColumnDefinition[]
  ) {
    // TODO: Build form dynamically based on columns with filterConfig
  }
  apply() {
    // TODO: Emit filter values
    this.dialogRef.close(this.form.value);
  }
  close() {
    this.dialogRef.close();
  }
}
