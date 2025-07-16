import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogContent,
  MatDialogClose,
} from '@angular/material/dialog';
import { Component, Inject, OnInit } from '@angular/core';
import { TrainingListService } from '../../training-list.service';
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
import { TrainingList } from '../../training-list.model';
import { MAT_DATE_LOCALE, MatOptionModule } from '@angular/material/core';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { formatDate } from '@angular/common';
import { MatDatepickerModule } from '@angular/material/datepicker';

export interface DialogData {
  id: number;
  action: string;
  trainingList: TrainingList;
}

@Component({
  selector: 'app-trainingLists-form',
  templateUrl: './form-dialog.component.html',
  styleUrls: ['./form-dialog.component.scss'],
  providers: [{ provide: MAT_DATE_LOCALE, useValue: 'en-GB' }],
  imports: [
    MatButtonModule,
    MatIconModule,
    MatDialogContent,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatDatepickerModule,
    MatSelectModule,
    MatOptionModule,
    MatInputModule,
    MatDialogClose,
  ],
})
export class TrainingListsFormComponent implements OnInit {
  action: string;
  dialogTitle: string;
  trainingListForm: UntypedFormGroup;
  trainingList: TrainingList;

  constructor(
    public dialogRef: MatDialogRef<TrainingListsFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public trainingListService: TrainingListService,
    private fb: UntypedFormBuilder
  ) {
    // Set action and trainingList data
    this.action = data.action;
    this.dialogTitle =
      this.action === 'edit' ? data.trainingList.trainingType : 'New Training';
    this.trainingList =
      this.action === 'edit'
        ? data.trainingList
        : new TrainingList({} as TrainingList);

    // Create form
    this.trainingListForm = this.createTrainingListForm();
  }

  ngOnInit() {
    // If action is not 'edit', fetch employees and store in the form
    if (this.action !== 'edit') {
      this.trainingListService.getEmployees().subscribe((employees) => {
        // Reset the employee field to empty to ensure no employee is selected by default
        this.trainingListForm.patchValue({ employee: [] });

        // Optionally, store the fetched employees in the trainingList
        this.trainingList.employee = employees;
        // Update the form control with the list of employees
        this.trainingListForm.patchValue({ employee: [] }); // No employee selected by default
      });
    }
  }

  // Create form group for trainingList fields with validation
  createTrainingListForm(): UntypedFormGroup {
    return this.fb.group({
      id: [this.trainingList.id],
      trainingType: [this.trainingList.trainingType, [Validators.required]],
      trainer: [this.trainingList.trainer, [Validators.required]],
      employee: [this.trainingList.employee || [], [Validators.required]],
      timeDuration: [this.trainingList.timeDuration],
      description: [this.trainingList.description],
      cost: [this.trainingList.cost],
      status: [this.trainingList.status || 'Pending', [Validators.required]],
      trainingDate: [
        formatDate(this.trainingList.trainingDate, 'yyyy-MM-dd', 'en'),
        [Validators.required],
      ],
      certification: [
        this.trainingList.certification,
        [Validators.maxLength(100)],
      ],
      department: [
        this.trainingList.department,
        [Validators.required, Validators.maxLength(100)],
      ],
      durationHours: [this.trainingList.durationHours],
      targetAudience: [
        this.trainingList.targetAudience,
        [Validators.required, Validators.maxLength(200)],
      ],
      prerequisites: [
        this.trainingList.prerequisites,
        [Validators.maxLength(500)],
      ],
      trainerContact: [this.trainingList.trainerContact],
      completionDate: [
        formatDate(this.trainingList.completionDate, 'yyyy-MM-dd', 'en'),
        [this.completionDateValidator],
      ],
    });
  }

  // Custom validators
  futureDateValidator(control: AbstractControl): ValidationErrors | null {
    if (!control.value) return null;

    const inputDate = new Date(control.value);
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    return inputDate >= today ? null : { pastDate: true };
  }

  completionDateValidator(control: AbstractControl): ValidationErrors | null {
    if (!control.value) return null;

    const trainingDate = control.parent?.get('trainingDate')?.value;
    if (!trainingDate) return null;

    const training = new Date(trainingDate);
    const completion = new Date(control.value);

    return completion >= training ? null : { invalidCompletionDate: true };
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
    if (this.trainingListForm.valid) {
      const formData = this.trainingListForm.getRawValue();
      if (this.action === 'edit') {
        this.trainingListService.updateTrainingList(formData).subscribe({
          next: (response) => {
            this.dialogRef.close(response);
          },
          error: (error) => {
            console.error('Update Error:', error);
          },
        });
      } else {
        this.trainingListService.addTrainingList(formData).subscribe({
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

  // Confirm and add the trainingList
  public confirmAdd(): void {
    this.submit();
  }
}
