import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogContent,
  MatDialogClose,
} from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { ResumesService } from '../../resumes.service';
import {
  UntypedFormControl,
  Validators,
  UntypedFormGroup,
  UntypedFormBuilder,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { Resumes } from '../../resumes.model';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

export interface DialogData {
  id: number;
  action: string;
  resumes: Resumes;
}

@Component({
    selector: 'app-resumes-form',
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
        MatDialogClose,
    ]
})
export class ResumesFormComponent {
  action: string;
  dialogTitle: string;
  resumesForm: UntypedFormGroup;
  resumes: Resumes;

  constructor(
    public dialogRef: MatDialogRef<ResumesFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public resumesService: ResumesService,
    private fb: UntypedFormBuilder
  ) {
    // Set the defaults
    this.action = data.action;
    this.resumes =
      this.action === 'edit' ? data.resumes : new Resumes({} as Resumes);
    this.dialogTitle =
      this.action === 'edit'
        ? `Edit Resume: ${this.resumes.name}`
        : 'New Resume';
    this.resumesForm = this.createResumesForm();
  }

  // Create the form for Resumes
  private createResumesForm(): UntypedFormGroup {
    return this.fb.group({
      id: [this.resumes.id],
      img: [this.resumes.img],
      name: [this.resumes.name, Validators.required],
      title: [this.resumes.title, Validators.required],
      status: [this.resumes.status, Validators.required],
      download: [this.resumes.download],
      role: [this.resumes.role],
      department: [this.resumes.department],
      jobType: [this.resumes.jobType],
    });
  }

  // Get error messages for validation
  getErrorMessage(controlName: string): string {
    const control = this.resumesForm.get(controlName);
    if (control?.hasError('required')) {
      return 'This field is required';
    }
    return '';
  }

  // Form submission logic
  submit(): void {
    if (this.resumesForm.valid) {
      const resumeData = this.resumesForm.getRawValue();
      if (this.action === 'edit') {
        this.resumesService.updateResume(resumeData).subscribe({
          next: (response) => {
            this.dialogRef.close(response);
          },
          error: (error) => {
            console.error('Update Error:', error);
            // Optionally handle error
          },
        });
      } else {
        this.resumesService.addResume(resumeData).subscribe({
          next: (response) => {
            this.dialogRef.close(response);
          },
          error: (error) => {
            console.error('Add Error:', error);
            // Optionally handle error
          },
        });
      }
    }
  }

  // Close dialog
  onNoClick(): void {
    this.dialogRef.close();
  }

  // Confirm addition of new resume
  public confirmAdd(): void {
    if (this.resumesForm.valid) {
      this.submit();
    }
  }
}
