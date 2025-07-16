import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogContent,
  MatDialogClose,
} from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { TrainingTypeService } from '../../training-type.service';
import {
  UntypedFormControl,
  Validators,
  UntypedFormGroup,
  UntypedFormBuilder,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { TrainingType } from '../../training-type.model';
import { MAT_DATE_LOCALE, MatOptionModule } from '@angular/material/core';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

export interface DialogData {
  id: number;
  action: string;
  trainingType: TrainingType;
}

@Component({
    selector: 'app-trainingTypes-form',
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
        MatSelectModule,
        MatOptionModule,
        MatInputModule,
        MatDialogClose,
    ]
})
export class TrainingTypesFormComponent {
  action: string;
  dialogTitle: string;
  trainingTypeForm: UntypedFormGroup;
  trainingType: TrainingType;

  constructor(
    public dialogRef: MatDialogRef<TrainingTypesFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public trainingTypeService: TrainingTypeService,
    private fb: UntypedFormBuilder
  ) {
    // Set action and trainingType data
    this.action = data.action;
    this.dialogTitle =
      this.action === 'edit'
        ? data.trainingType.trainingTypeName
        : 'New Training Type';
    this.trainingType =
      this.action === 'edit'
        ? data.trainingType
        : new TrainingType({} as TrainingType);

    // Create form
    this.trainingTypeForm = this.createTrainingTypeForm();
  }

  // Create form group for trainingType fields with validation
  createTrainingTypeForm(): UntypedFormGroup {
    return this.fb.group({
      trainingTypeId: [this.trainingType.trainingTypeId],
      trainingTypeName: [
        this.trainingType.trainingTypeName,
        [Validators.required],
      ],
      description: [this.trainingType.description],
      category: [this.trainingType.category, [Validators.required]],
      duration: [this.trainingType.duration, [Validators.required]],
      deliveryMethod: [this.trainingType.deliveryMethod, [Validators.required]],
      targetAudience: [this.trainingType.targetAudience],
      status: [this.trainingType.status, [Validators.required]],
      createdDate: [this.trainingType.createdDate],
      updatedDate: [this.trainingType.updatedDate],
      createdBy: [this.trainingType.createdBy],
      updatedBy: [this.trainingType.updatedBy],
      isMandatory: [this.trainingType.isMandatory, [Validators.required]],
      cost: [this.trainingType.cost, [Validators.required, Validators.min(0)]],
      certification: [this.trainingType.certification],
    });
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
    if (this.trainingTypeForm.valid) {
      const formData = this.trainingTypeForm.getRawValue();
      if (this.action === 'edit') {
        this.trainingTypeService.updateTrainingType(formData).subscribe({
          next: (response) => {
            this.dialogRef.close(response);
          },
          error: (error) => {
            console.error('Update Error:', error);
          },
        });
      } else {
        this.trainingTypeService.addTrainingType(formData).subscribe({
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

  // Confirm and add the trainingType
  public confirmAdd(): void {
    this.submit();
  }
}
