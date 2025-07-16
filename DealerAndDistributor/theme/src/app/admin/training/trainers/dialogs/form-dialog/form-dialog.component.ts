import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogContent,
  MatDialogClose,
} from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { TrainersService } from '../../trainers.service';
import {
  UntypedFormControl,
  Validators,
  UntypedFormGroup,
  UntypedFormBuilder,
  FormsModule,
  ReactiveFormsModule,
  AbstractControl,
  ValidationErrors,
} from '@angular/forms';
import { Trainers } from '../../trainers.model';
import { MAT_DATE_LOCALE, MatOptionModule } from '@angular/material/core';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { formatDate } from '@angular/common';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatChipInputEvent, MatChipsModule } from '@angular/material/chips';

export interface DialogData {
  id: number;
  action: string;
  trainers: Trainers;
}

@Component({
    selector: 'app-trainerss-form',
    templateUrl: './form-dialog.component.html',
    styleUrls: ['./form-dialog.component.scss'],
    providers: [{ provide: MAT_DATE_LOCALE, useValue: 'en-GB' }],
    imports: [
        MatButtonModule,
        MatIconModule,
        MatDialogContent,
        FormsModule,
        MatDatepickerModule,
        ReactiveFormsModule,
        MatChipsModule,
        MatFormFieldModule,
        MatSelectModule,
        MatOptionModule,
        MatInputModule,
        MatDialogClose
    ]
})
export class TrainerssFormComponent {
  action: string;
  dialogTitle: string;
  trainersForm: UntypedFormGroup;
  trainers: Trainers;

  constructor(
    public dialogRef: MatDialogRef<TrainerssFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public trainersService: TrainersService,
    private fb: UntypedFormBuilder
  ) {
    // Set action and trainers data
    this.action = data.action;
    this.dialogTitle =
      this.action === 'edit' ? data.trainers.name : 'New Trainers';
    this.trainers =
      this.action === 'edit' ? data.trainers : new Trainers({} as Trainers);

    // Create form
    this.trainersForm = this.createTrainersForm();
  }

  // Create form group for trainers fields with validation
  createTrainersForm(): UntypedFormGroup {
    return this.fb.group({
      trainer_id: [this.trainers.trainer_id],
      img: [this.trainers.img, [Validators.maxLength(500)]],
      name: [
        this.trainers.name,
        [Validators.required, Validators.maxLength(100)],
      ],
      email: [
        this.trainers.email,
        [Validators.required, Validators.email, Validators.maxLength(100)],
      ],
      phone_number: [
        this.trainers.phone_number,
        [
          Validators.required,
          Validators.pattern(
            /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/
          ),
          Validators.maxLength(15),
        ],
      ],
      hire_date: [
        formatDate(this.trainers.hire_date, 'yyyy-MM-dd', 'en'),
        [Validators.required, this.pastDateValidator],
      ],
      specialization: [
        this.trainers.specialization,
        [Validators.required, Validators.maxLength(100)],
      ],
      technical_skills: [
        this.trainers.technical_skills || [],
        [Validators.required],
      ],
      certifications: [
        this.trainers.certifications || [],
        [Validators.required],
      ],
      training_experience: [
        this.trainers.training_experience,
        [Validators.required, Validators.min(0), Validators.max(50)],
      ],
      industry_experience: [
        this.trainers.industry_experience,
        [Validators.required, Validators.min(0), Validators.max(50)],
      ],
      training_area: [
        this.trainers.training_area,
        [Validators.required, Validators.maxLength(100)],
      ],
      status: [
        this.trainers.status || 'Active',
        [Validators.required, Validators.maxLength(50)],
      ],
      location: [
        this.trainers.location,
        [Validators.required, Validators.maxLength(100)],
      ],
      languages_spoken: [
        this.trainers.languages_spoken || [],
        [Validators.required],
      ],
      training_format: [
        this.trainers.training_format,
        [Validators.required, Validators.maxLength(50)],
      ],
    });
  }

  // Custom validators
  pastDateValidator(control: AbstractControl): ValidationErrors | null {
    if (!control.value) return null;

    const inputDate = new Date(control.value);
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    return inputDate <= today ? null : { futureDate: true };
  }

  // Handle form validation errors for user feedback
  getErrorMessage(control: UntypedFormControl): string {
    if (control.hasError('required')) {
      return 'This field is required';
    }
    if (control.hasError('email')) {
      return 'Invalid email format';
    }
    return '';
  }

  // Submit form data
  submit(): void {
    if (this.trainersForm.valid) {
      const formData = this.trainersForm.getRawValue();
      if (this.action === 'edit') {
        this.trainersService.updateTrainer(formData).subscribe({
          next: (response) => {
            this.dialogRef.close(response);
          },
          error: (error) => {
            console.error('Update Error:', error);
          },
        });
      } else {
        this.trainersService.addTrainer(formData).subscribe({
          next: (response) => {
            this.dialogRef.close(response);
          },
          error: (error) => {
            console.error('Add Error:', error);
          },
        });
      }
    }
  }

  // Close the dialog without submitting
  onNoClick(): void {
    this.dialogRef.close();
  }

  // Confirm and add the trainers
  public confirmAdd(): void {
    this.submit();
  }
}
