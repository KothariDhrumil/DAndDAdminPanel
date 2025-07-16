import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogContent,
  MatDialogClose,
} from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { EstimatesService } from '../../estimates.service';
import {
  UntypedFormControl,
  Validators,
  UntypedFormGroup,
  UntypedFormBuilder,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { Estimates } from '../../estimates.model';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

export interface DialogData {
  id: number;
  action: string;
  estimates: Estimates;
}

@Component({
    selector: 'app-estimate-form',
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
        MatSelectModule,
        MatOptionModule,
        MatDialogClose,
    ]
})
export class EstimatesFormComponent {
  action: string;
  dialogTitle: string;
  estimatesForm: UntypedFormGroup;
  estimates: Estimates;

  constructor(
    public dialogRef: MatDialogRef<EstimatesFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public estimatesService: EstimatesService,
    private fb: UntypedFormBuilder
  ) {
    // Set the defaults based on the action type
    this.action = data.action;
    this.dialogTitle =
      this.action === 'edit'
        ? `Edit Estimate for ${data.estimates.clientName}`
        : 'New Estimate';
    this.estimates =
      this.action === 'edit' ? data.estimates : new Estimates({} as Estimates);
    this.estimatesForm = this.createEstimatesForm();
  }

  // Create form group for estimates details
  createEstimatesForm(): UntypedFormGroup {
    return this.fb.group({
      id: [this.estimates.id],
      estimateId: [this.estimates.estimateId, [Validators.required]],
      clientName: [this.estimates.clientName, [Validators.required]],
      mobile: [this.estimates.mobile, [Validators.required]],
      email: [
        this.estimates.email,
        [Validators.required, Validators.email, Validators.minLength(5)],
      ],
      estDate: [this.estimates.estDate, [Validators.required]],
      expDate: [this.estimates.expDate, [Validators.required]],
      country: [this.estimates.country],
      amount: [this.estimates.amount, [Validators.required, Validators.min(0)]],
      status: [this.estimates.status, [Validators.required]],
      details: [this.estimates.details],
    });
  }

  // Dynamic error message retrieval
  getErrorMessage(controlName: string): string {
    const control = this.estimatesForm.get(controlName);
    if (control?.hasError('required')) {
      return 'This field is required';
    }
    if (control?.hasError('email')) {
      return 'Not a valid email';
    }
    if (control?.hasError('min')) {
      return 'Amount must be greater than or equal to 0';
    }
    return ''; // Return empty if no errors
  }

  // Submit form data
  submit() {
    if (this.estimatesForm.valid) {
      const estimateData = this.estimatesForm.getRawValue();
      if (this.action === 'edit') {
        this.estimatesService.updateEstimate(estimateData).subscribe({
          next: (response) => {
            this.dialogRef.close(response);
          },
          error: (error) => {
            console.error('Update Error:', error);
            // Optionally show an error message to the user
          },
        });
      } else {
        this.estimatesService.addEstimate(estimateData).subscribe({
          next: (response) => {
            this.dialogRef.close(response);
          },
          error: (error) => {
            console.error('Add Error:', error);
            // Optionally show an error message to the user
          },
        });
      }
    }
  }

  // Close dialog without action
  onNoClick(): void {
    this.dialogRef.close();
  }
}
