import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogContent,
} from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import {
  UntypedFormControl,
  Validators,
  UntypedFormGroup,
  UntypedFormBuilder,
} from '@angular/forms';
import { formatDate, DatePipe } from '@angular/common';
import { AttendancesService } from '../attendance.service';
import { Attendances } from '../attendance.model';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

export interface DialogData {
  id: number;
  action: string;
  attendances: Attendances;
}

@Component({
    selector: 'app-attendance-form',
    templateUrl: './form.component.html',
    styleUrls: ['./form.component.scss'],
    imports: [
        MatButtonModule,
        MatIconModule,
        MatDialogContent,
        MatCardModule,
        MatTooltipModule,
        DatePipe,
    ]
})
export class AttendanceFormComponent {
  action: string;
  dialogTitle?: string;
  isDetails = false;
  attendancesForm: UntypedFormGroup;
  attendances: Attendances;

  constructor(
    public dialogRef: MatDialogRef<AttendanceFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public attendancesService: AttendancesService,
    private fb: UntypedFormBuilder
  ) {
    // Set the defaults
    this.action = data.action;
    this.attendances = data.attendances || ({} as Attendances);

    // Initialize form
    this.attendancesForm = this.createAttendanceForm();

    if (this.action === 'details') {
      this.isDetails = true;
      this.dialogTitle = `Attendance Details for ${this.attendances.date}`;
    } else {
      this.dialogTitle =
        this.action === 'edit' ? 'Edit Attendance' : 'New Attendance';
    }
  }

  // Create form with validation rules
  private createAttendanceForm(): UntypedFormGroup {
    return this.fb.group({
      id: [this.attendances.id],
      date: [
        formatDate(this.attendances.date, 'yyyy-MM-dd, HH:mm', 'en'),
        [Validators.required],
      ],
      check_in: [this.attendances.check_in, [Validators.required]],
      break: [this.attendances.break],
      check_out: [this.attendances.check_out],
      hours: [this.attendances.hours],
      status: [this.attendances.status],
      details: [this.attendances.details],
    });
  }

  // Display error messages for validation
  getErrorMessage(controlName: string): string {
    const control = this.attendancesForm.get(controlName);
    if (control?.hasError('required')) {
      return 'This field is required';
    }
    return '';
  }

  // Submit form for adding or editing attendance
  submit(): void {
    if (this.attendancesForm.valid) {
      const attendanceData = this.attendancesForm.getRawValue();
      if (this.action === 'edit') {
        this.attendancesService.updateAttendance(attendanceData).subscribe({
          next: (response) => this.dialogRef.close(response),
          error: (error) => console.error('Update Error:', error),
        });
      } else {
        this.attendancesService.addAttendance(attendanceData).subscribe({
          next: (response) => this.dialogRef.close(response),
          error: (error) => console.error('Add Error:', error),
        });
      }
    }
  }

  // Close the dialog without changes
  onNoClick(): void {
    this.dialogRef.close();
  }

  // Confirm adding or editing of attendance
  public confirmAdd(): void {
    this.submit();
  }
}
