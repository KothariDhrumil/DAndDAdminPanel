import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogContent,
  MatDialogClose,
} from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import {
  UntypedFormControl,
  Validators,
  UntypedFormGroup,
  UntypedFormBuilder,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { MyLeaves } from '../../my-leaves.model';
import { MyLeavesService } from '../../my-leaves.service';
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
  myLeaves: MyLeaves;
}

@Component({
    selector: 'app-my-leaves-form',
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
export class MyLeavesFormComponent {
  action: string;
  dialogTitle: string;
  myLeavesForm: UntypedFormGroup;
  myLeaves: MyLeaves;

  constructor(
    public dialogRef: MatDialogRef<MyLeavesFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public myLeavesService: MyLeavesService,
    private fb: UntypedFormBuilder
  ) {
    // Set the defaults
    this.action = data.action;
    this.dialogTitle =
      this.action === 'edit' ? 'Edit Leave Request' : 'New Leave Request';
    this.myLeaves =
      this.action === 'edit' ? data.myLeaves : new MyLeaves({} as MyLeaves);

    // Create the form
    this.myLeavesForm = this.createMyLeavesForm();
  }

  // Create the form with validation
  private createMyLeavesForm(): UntypedFormGroup {
    return this.fb.group({
      id: [this.myLeaves.id],
      halfDay: [this.myLeaves.halfDay, [Validators.required]],
      applyDate: [this.myLeaves.applyDate, [Validators.required]],
      fromDate: [this.myLeaves.fromDate, [Validators.required]],
      toDate: [this.myLeaves.toDate, [Validators.required]],
      type: [this.myLeaves.type, [Validators.required]],
      status: [this.myLeaves.status, [Validators.required]],
      reason: [this.myLeaves.reason, [Validators.required]],
    });
  }

  // Display error messages for validation
  getErrorMessage(controlName: string): string {
    const control = this.myLeavesForm.get(controlName);
    return control?.hasError('required') ? 'Required field' : '';
  }

  // Submit the form
  submit(): void {
    if (this.myLeavesForm.valid) {
      const leaveData = this.myLeavesForm.getRawValue();
      if (this.action === 'edit') {
        this.myLeavesService.updateMyLeaves(leaveData).subscribe({
          next: (response) => this.dialogRef.close(response),
          error: (error) => console.error('Update Error:', error),
        });
      } else {
        this.myLeavesService.addMyLeaves(leaveData).subscribe({
          next: (response) => this.dialogRef.close(response),
          error: (error) => console.error('Add Error:', error),
        });
      }
    }
  }

  // Close the dialog without making changes
  onNoClick(): void {
    this.dialogRef.close();
  }

  // Confirm adding or editing of leave request
  public confirmAdd(): void {
    this.submit();
  }
}
