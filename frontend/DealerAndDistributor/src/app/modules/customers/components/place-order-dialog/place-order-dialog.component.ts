import { ChangeDetectionStrategy, Component, Inject, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogContent, MatDialogActions, MatDialogTitle } from '@angular/material/dialog';
import { CustomersService } from '../../service/customers.service';

@Component({
  selector: 'app-place-order-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatDialogContent, MatDialogActions, MatDialogTitle],
  templateUrl: './place-order-dialog.component.html',
  styleUrls: ['./place-order-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PlaceOrderDialogComponent {
  private fb = inject(FormBuilder);
  private service = inject(CustomersService);
  private dialogRef = inject(MatDialogRef<PlaceOrderDialogComponent>);

  form = this.fb.group({
    total: this.fb.control<number | null>(null, { validators: [Validators.required, Validators.min(0)], nonNullable: false })
  });

  constructor(@Inject(MAT_DIALOG_DATA) public data: { globalCustomerId: string }) {}

  submit() {
    if (this.form.invalid) return;
    const total = Number(this.form.controls.total.value);
    this.service.createCustomerOrder(this.data.globalCustomerId, total).subscribe({
      next: () => this.dialogRef.close(true),
      error: () => this.dialogRef.close(false)
    });
  }

  cancel() { this.dialogRef.close(false); }
}
