import { Component } from '@angular/core';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { BreadcrumbComponent } from '@shared/components/breadcrumb/breadcrumb.component';
@Component({
    selector: 'app-add-payment',
    templateUrl: './add-payment.component.html',
    styleUrls: ['./add-payment.component.scss'],
    imports: [
        BreadcrumbComponent,
        FormsModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatSelectModule,
        MatOptionModule,
        MatDatepickerModule,
        MatButtonModule,
    ]
})
export class AddPaymentComponent {
  paymentForm: UntypedFormGroup;
  constructor(private fb: UntypedFormBuilder) {
    this.paymentForm = this.fb.group({
      payment_id: [''],
      client_id: [''],
      client_name: ['', Validators.required],
      invoice_id: ['', Validators.required],
      payment_date: ['', Validators.required],
      payment_amount: [
        '',
        [Validators.required, Validators.pattern(/^\d+(\.\d{1,2})?$/)],
      ],
      payment_method: ['', Validators.required],
      transaction_id: [''],
      payment_status: ['', Validators.required],
      currency: ['', Validators.required],
      description: [''],
      created_at: [''],
      updated_at: [''],
    });
  }
  onSubmit() {
    console.log('Form Value', this.paymentForm.value);
  }
}
