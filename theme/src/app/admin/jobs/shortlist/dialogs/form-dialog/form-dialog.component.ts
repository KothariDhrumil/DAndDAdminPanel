import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogContent,
  MatDialogClose,
} from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { ShortlistService } from '../../shortlist.service';
import {
  UntypedFormControl,
  Validators,
  UntypedFormGroup,
  UntypedFormBuilder,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { Shortlist } from '../../shortlist.model';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

export interface DialogData {
  id: number;
  action: string;
  shortlist: Shortlist;
}

@Component({
    selector: 'app-shortlist-form',
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
export class ShortlistFormComponent {
  action: string;
  dialogTitle: string;
  shortlistForm: UntypedFormGroup;
  shortlist: Shortlist;

  constructor(
    public dialogRef: MatDialogRef<ShortlistFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public shortlistService: ShortlistService,
    private fb: UntypedFormBuilder
  ) {
    // Set the defaults
    this.action = data.action;
    this.shortlist =
      this.action === 'edit' ? data.shortlist : new Shortlist({} as Shortlist);
    this.dialogTitle =
      this.action === 'edit'
        ? `Edit Shortlist: ${this.shortlist.name}`
        : 'New Shortlist';
    this.shortlistForm = this.createShortlistForm();
  }

  // Create form with validation
  private createShortlistForm(): UntypedFormGroup {
    return this.fb.group({
      id: [this.shortlist.id],
      img: [this.shortlist.img],
      name: [this.shortlist.name, Validators.required],
      title: [this.shortlist.title, Validators.required],
      mobile: [
        this.shortlist.mobile,
        [Validators.required, Validators.pattern(/^\+?[0-9]{10,15}$/)],
      ],
      download: [this.shortlist.download],
      role: [this.shortlist.role],
      email: [this.shortlist.email, [Validators.required, Validators.email]],
      jobType: [this.shortlist.jobType],
    });
  }

  // Get error messages for validation
  getErrorMessage(controlName: string): string {
    const control = this.shortlistForm.get(controlName);
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
    if (this.shortlistForm.valid) {
      const shortlistData = this.shortlistForm.getRawValue();
      if (this.action === 'edit') {
        this.shortlistService.updateShortlist(shortlistData).subscribe({
          next: (response) => {
            this.dialogRef.close(response);
          },
          error: (error) => {
            console.error('Update Error:', error);
            // Optionally show an error message
          },
        });
      } else {
        this.shortlistService.addShortlist(shortlistData).subscribe({
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
}
