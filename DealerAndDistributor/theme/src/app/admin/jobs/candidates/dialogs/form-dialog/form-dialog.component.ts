import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogContent,
  MatDialogClose,
} from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { CandidatesService } from '../../candidates.service';
import {
  UntypedFormControl,
  Validators,
  UntypedFormGroup,
  UntypedFormBuilder,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { Candidates } from '../../candidates.model';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

export interface DialogData {
  id: number;
  action: string;
  candidates: Candidates;
}

@Component({
    selector: 'app-candidates-form',
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
export class CandidatesFormComponent {
  action: string;
  dialogTitle: string;
  candidatesForm: UntypedFormGroup;
  candidates: Candidates;

  constructor(
    public dialogRef: MatDialogRef<CandidatesFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public candidatesService: CandidatesService,
    private fb: UntypedFormBuilder
  ) {
    // Set the defaults
    this.action = data.action;
    this.candidates =
      this.action === 'edit'
        ? data.candidates
        : new Candidates({} as Candidates);
    this.dialogTitle =
      this.action === 'edit'
        ? `Edit Candidate: ${this.candidates.name}`
        : 'New Candidate';
    this.candidatesForm = this.createCandidatesForm();
  }

  // Create form with validation
  private createCandidatesForm(): UntypedFormGroup {
    return this.fb.group({
      id: [this.candidates.id],
      img: [this.candidates.img],
      name: [this.candidates.name, Validators.required],
      title: [this.candidates.title, Validators.required],
      mobile: [
        this.candidates.mobile,
        [Validators.required, Validators.pattern(/^\+?[0-9]{10,15}$/)],
      ],
      download: [this.candidates.download],
      role: [this.candidates.role],
      email: [this.candidates.email, [Validators.required, Validators.email]],
      jobType: [this.candidates.jobType],
    });
  }

  // Get error messages for validation
  getErrorMessage(controlName: string): string {
    const control = this.candidatesForm.get(controlName);
    if (control?.hasError('required')) {
      return 'This field is required';
    }
    if (control?.hasError('email')) {
      return 'Invalid email format';
    }
    if (control?.hasError('pattern')) {
      return 'Invalid mobile number format';
    }
    return '';
  }

  // Form submission logic
  submit(): void {
    if (this.candidatesForm.valid) {
      const candidateData = this.candidatesForm.getRawValue();
      if (this.action === 'edit') {
        this.candidatesService.updateCandidate(candidateData).subscribe({
          next: (response) => {
            this.dialogRef.close(response);
          },
          error: (error) => {
            console.error('Update Error:', error);
            // Optionally show an error message
          },
        });
      } else {
        this.candidatesService.addCandidate(candidateData).subscribe({
          next: (response) => {
            this.dialogRef.close(response);
          },
          error: (error) => {
            console.error('Add Error:', error);
            // Optionally show an error message
          },
        });
      }
    }
  }

  // Close the dialog
  onNoClick(): void {
    this.dialogRef.close();
  }

  // Confirm addition of new candidate
  public confirmAdd(): void {
    if (this.candidatesForm.valid) {
      this.submit();
    }
  }
}
