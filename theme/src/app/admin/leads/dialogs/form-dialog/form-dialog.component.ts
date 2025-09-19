import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogContent,
  MatDialogClose,
} from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { LeadsService } from '../../leads.service';
import {
  UntypedFormGroup,
  UntypedFormBuilder,
  Validators,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { Leads } from '../../leads.model';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

export interface DialogData {
  id: number;
  action: string;
  leads: Leads;
}

@Component({
    selector: 'app-leads-form',
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
        MatDialogClose,
    ]
})
export class LeadsFormComponent {
  action: string;
  dialogTitle: string;
  leadsForm: UntypedFormGroup;
  leads: Leads;

  constructor(
    public dialogRef: MatDialogRef<LeadsFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public leadsService: LeadsService,
    private fb: UntypedFormBuilder
  ) {
    // Set the defaults
    this.action = data.action;
    this.leads = this.action === 'edit' ? data.leads : new Leads({} as Leads);
    this.dialogTitle =
      this.action === 'edit' ? `Edit Leads: ${this.leads.name}` : 'New Leads';
    this.leadsForm = this.createLeadsForm();
  }

  // Create the form for Leads
  private createLeadsForm(): UntypedFormGroup {
    return this.fb.group({
      id: [this.leads.id],
      img: [this.leads.img],
      name: [this.leads.name, Validators.required],
      email: [this.leads.email, [Validators.required, Validators.email]],
      role: [this.leads.role],
      mobile: [
        this.leads.mobile,
        [Validators.required, Validators.pattern(/^\+?[0-9]{10,15}$/)],
      ],
      department: [this.leads.department],
      project: [this.leads.project],
    });
  }

  // Error message handling for all fields
  getErrorMessage(controlName: string): string {
    const control = this.leadsForm.get(controlName);
    if (control?.hasError('required')) {
      return 'This field is required';
    }
    if (control?.hasError('email')) {
      return 'Not a valid email';
    }
    if (control?.hasError('pattern')) {
      return 'Invalid phone number format';
    }
    return '';
  }

  // Submit logic
  submit(): void {
    if (this.leadsForm.valid) {
      const leadsData = this.leadsForm.getRawValue();
      if (this.action === 'edit') {
        this.leadsService.updateLeads(leadsData).subscribe({
          next: (response) => {
            this.dialogRef.close(response);
          },
          error: (error) => {
            console.error('Update Error:', error);
            // Optionally show an error message to the user
          },
        });
      } else {
        this.leadsService.addLeads(leadsData).subscribe({
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

  // Close the dialog
  onNoClick(): void {
    this.dialogRef.close();
  }
}
