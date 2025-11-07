import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';

export interface RenameTenantDialogData {
  tenantName: string;
}

@Component({
  selector: 'app-rename-tenant-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatDialogModule, MatButtonModule],
  templateUrl: './rename-tenant-dialog.component.html',
  styleUrls: ['./rename-tenant-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RenameTenantDialogComponent {
  form = new FormGroup({
    tenantName: new FormControl<string>('', { nonNullable: true, validators: [Validators.required, Validators.maxLength(200)] })
  });

  constructor(
    private dialogRef: MatDialogRef<RenameTenantDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: RenameTenantDialogData
  ) {
    this.form.patchValue({ tenantName: data?.tenantName ?? '' });
  }

  save() {
    if (this.form.invalid) return;
    this.dialogRef.close({ tenantName: this.form.value.tenantName });
  }

  close() { this.dialogRef.close(); }
}
