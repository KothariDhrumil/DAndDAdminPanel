import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogContent,
  MatDialogClose,
} from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { JobsListService } from '../../jobs-list.service';
import {
  UntypedFormControl,
  Validators,
  UntypedFormGroup,
  UntypedFormBuilder,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { JobsList } from '../../jobs-list.model';
import { formatDate } from '@angular/common';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

export interface DialogData {
  id: number;
  action: string;
  jobsList: JobsList;
}

@Component({
    selector: 'app-jobs-list-form',
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
        MatSelectModule,
        MatOptionModule,
        MatDatepickerModule,
        MatDialogClose,
    ]
})
export class JobsListFormComponent {
  action: string;
  dialogTitle: string;
  jobsListForm: UntypedFormGroup;
  jobsList: JobsList;

  constructor(
    public dialogRef: MatDialogRef<JobsListFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public jobsListService: JobsListService,
    private fb: UntypedFormBuilder
  ) {
    // Set the defaults
    this.action = data.action;
    this.jobsList =
      this.action === 'edit' ? data.jobsList : new JobsList({} as JobsList);
    this.dialogTitle =
      this.action === 'edit' ? `Edit Job: ${this.jobsList.title}` : 'New Job';
    this.jobsListForm = this.createJobsListForm();
  }

  // Form control with validation
  formControl = new UntypedFormControl('', [Validators.required]);

  // Get error message for form validation
  getErrorMessage(controlName: string): string {
    const control = this.jobsListForm.get(controlName);
    if (control?.hasError('required')) {
      return 'This field is required';
    }
    return '';
  }

  // Create the form with default values and validations
  private createJobsListForm(): UntypedFormGroup {
    return this.fb.group({
      id: [this.jobsList.id],
      title: [this.jobsList.title, Validators.required],
      status: [this.jobsList.status, Validators.required],
      date: [
        formatDate(this.jobsList.date, 'yyyy-MM-dd', 'en'),
        Validators.required,
      ],
      role: [this.jobsList.role],
      vacancies: [this.jobsList.vacancies],
      department: [this.jobsList.department],
      jobType: [this.jobsList.jobType],
    });
  }

  // Form submission logic
  submit(): void {
    if (this.jobsListForm.valid) {
      const jobData = this.jobsListForm.getRawValue();
      if (this.action === 'edit') {
        this.jobsListService.updateJobsList(jobData).subscribe({
          next: (response) => {
            this.dialogRef.close(response);
          },
          error: (error) => {
            console.error('Update Error:', error);
            // Optionally show error to user
          },
        });
      } else {
        this.jobsListService.addJobsList(jobData).subscribe({
          next: (response) => {
            this.dialogRef.close(response);
          },
          error: (error) => {
            console.error('Add Error:', error);
            // Optionally show error to user
          },
        });
      }
    }
  }

  // Close the dialog
  onNoClick(): void {
    this.dialogRef.close();
  }

  // Confirm add action
  public confirmAdd(): void {
    if (this.jobsListForm.valid) {
      this.submit();
    }
  }
}
