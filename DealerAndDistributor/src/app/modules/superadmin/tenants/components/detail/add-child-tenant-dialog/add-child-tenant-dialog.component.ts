import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { TenantsService } from '../../../service/tenants.service';

export interface AddChildTenantDialogData {
  parentTenantId: number;
}

@Component({
  selector: 'app-add-child-tenant-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatDialogModule, MatButtonModule],
  template: `
    <h2 mat-dialog-title>Add Child Tenant</h2>
    <div mat-dialog-content>
      <form [formGroup]="form" (ngSubmit)="save()">
        <mat-form-field appearance="outline" class="w-100">
          <mat-label>Tenant Name</mat-label>
          <input matInput formControlName="tenantName" required />
        </mat-form-field>
      </form>
    </div>
    <div mat-dialog-actions align="end">
      <button mat-stroked-button (click)="close()">Cancel</button>
      <button mat-flat-button color="primary" [disabled]="form.invalid" (click)="save()">Add</button>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AddChildTenantDialogComponent {
  form = new FormGroup({
    tenantName: new FormControl<string>('', { nonNullable: true, validators: [Validators.required, Validators.maxLength(200)] })
  });

  constructor(
    private dialogRef: MatDialogRef<AddChildTenantDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: AddChildTenantDialogData,
    private tenantsService: TenantsService
  ) {}

  save() {
    if (this.form.invalid) return;
    const name = this.form.value.tenantName ?? '';
    this.tenantsService.createChildTenant(this.data.parentTenantId, name).subscribe({
      next: (res) => this.dialogRef.close(!!res?.isSuccess),
      error: () => this.dialogRef.close(false)
    });
  }

  close() {
    this.dialogRef.close();
  }
}
