import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogContent,
  MatDialogClose,
} from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { ClientPaymentService } from '../../client-payment.service';
import {
  UntypedFormGroup,
  UntypedFormBuilder,
  Validators,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { ClientPayment } from '../../client-payment.model';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

export interface DialogData {
  id: number;
  action: string;
  clientPayment: ClientPayment;
}

@Component({
    selector: 'app-client-payment-form',
    templateUrl: './form-dialog.component.html',
    styleUrls: ['./form-dialog.component.scss'],
    imports: [
        MatButtonModule,
        MatIconModule,
        MatDialogContent,
        FormsModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatDatepickerModule,
        MatDialogClose,
    ]
})
export class ClientPaymentFormComponent {
  action: string;
  dialogTitle: string;
  paymentForm: UntypedFormGroup;
  clientPayment: ClientPayment;

  constructor(
    public dialogRef: MatDialogRef<ClientPaymentFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public clientPaymentService: ClientPaymentService,
    private fb: UntypedFormBuilder
  ) {
    // Set the defaults
    this.action = data.action;
    this.clientPayment =
      this.action === 'edit'
        ? data.clientPayment
        : new ClientPayment({} as ClientPayment);
    this.dialogTitle =
      this.action === 'edit'
        ? `Edit Payment for ${this.clientPayment.client_name}`
        : 'New Client Payment';
    this.paymentForm = this.createPaymentForm();
  }

  // Create payment form
  private createPaymentForm(): UntypedFormGroup {
    return this.fb.group({
      payment_id: [this.clientPayment.payment_id],
      client_id: [this.clientPayment.client_id],
      client_name: [this.clientPayment.client_name, Validators.required],
      invoice_id: [this.clientPayment.invoice_id, Validators.required],
      payment_date: [this.clientPayment.payment_date, Validators.required],
      payment_amount: [
        this.clientPayment.payment_amount,
        [Validators.required, Validators.pattern(/^\d+(\.\d{1,2})?$/)],
      ],
      payment_method: [this.clientPayment.payment_method, Validators.required],
      transaction_id: [this.clientPayment.transaction_id],
      payment_status: [this.clientPayment.payment_status, Validators.required],
      currency: [this.clientPayment.currency, Validators.required],
      description: [this.clientPayment.description],
      created_at: [this.clientPayment.created_at],
      updated_at: [this.clientPayment.updated_at],
    });
  }

  // Generalized error message method
  getErrorMessage(controlName: string): string {
    const control = this.paymentForm.get(controlName);
    if (control?.hasError('required')) {
      return 'This field is required';
    }
    if (control?.hasError('pattern')) {
      return 'Invalid amount format';
    }
    return '';
  }

  // Submit logic
  submit(): void {
    if (this.paymentForm.valid) {
      const paymentData = this.paymentForm.getRawValue();
      if (this.action === 'edit') {
        this.clientPaymentService.updatePayment(paymentData).subscribe({
          next: (response) => {
            this.dialogRef.close(response);
          },
          error: (error) => {
            console.error('Update Payment Error:', error);
            // Optionally show an error message to the user
          },
        });
      } else {
        this.clientPaymentService.addPayment(paymentData).subscribe({
          next: (response) => {
            this.dialogRef.close(response);
          },
          error: (error) => {
            console.error('Add Payment Error:', error);
            // Optionally show an error message to the user
          },
        });
      }
    }
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
