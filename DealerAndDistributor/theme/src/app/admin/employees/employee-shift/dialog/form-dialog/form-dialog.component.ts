import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogContent,
  MatDialogClose,
} from '@angular/material/dialog';
import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import {
  UntypedFormGroup,
  UntypedFormBuilder,
  Validators,
  FormsModule,
  ReactiveFormsModule,
  AbstractControl,
  ValidationErrors,
} from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { EmployeeShift } from '../../employee-shift.model';
import { EmployeeShiftService } from '../../employee-shift.service';
import { formatDate } from '@angular/common';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { provideNativeDateAdapter } from '@angular/material/core';
import { MatTimepickerModule } from '@angular/material/timepicker';

export interface DialogData {
  id: number;
  action: string;
  employeeShift: EmployeeShift;
}

@Component({
  selector: 'app-employeeShift-form',
  templateUrl: './form-dialog.component.html',
  styleUrls: ['./form-dialog.component.scss'],
  providers: [provideNativeDateAdapter()],
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
    MatDialogClose,
    MatTimepickerModule,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EmployeeShiftFormComponent {
  action: string;
  dialogTitle: string;
  employeeShiftForm: UntypedFormGroup;
  employeeShift: EmployeeShift;

  constructor(
    public dialogRef: MatDialogRef<EmployeeShiftFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public employeeShiftService: EmployeeShiftService,
    private fb: UntypedFormBuilder
  ) {
    // Set the defaults
    this.action = data.action;
    this.employeeShift =
      this.action === 'edit'
        ? data.employeeShift
        : new EmployeeShift({} as EmployeeShift);
    this.dialogTitle =
      this.action === 'edit'
        ? `Edit ${this.employeeShift.employeeName}`
        : 'New Employee';
    this.employeeShiftForm = this.createClientForm();
  }

  // Create the form with validation
  private createClientForm(): UntypedFormGroup {
    return this.fb.group({
      shiftId: [this.employeeShift.shiftId],
      employeeId: [this.employeeShift.employeeId, [Validators.required]],
      img: [this.employeeShift.img],
      employeeName: [this.employeeShift.employeeName, [Validators.required]],
      shiftStartTime: [
        this.parseTimeString(this.employeeShift.shiftStartTime),
        [Validators.required],
      ],
      shiftEndTime: [
        this.parseTimeString(this.employeeShift.shiftEndTime),
        [Validators.required],
      ],
      shiftType: [
        this.employeeShift.shiftType || 'Regular',
        [Validators.required, Validators.maxLength(50)],
      ],
      shiftDate: [
        formatDate(this.employeeShift.shiftDate, 'yyyy-MM-dd', 'en'),
        [Validators.required],
      ],
      breakStartTime: [this.parseTimeString(this.employeeShift.breakStartTime)],
      breakEndTime: [this.parseTimeString(this.employeeShift.breakEndTime)],
      totalShiftHours: [
        this.employeeShift.totalShiftHours,
        [Validators.min(0), Validators.max(24)],
      ],
      shiftStatus: [
        this.employeeShift.shiftStatus || 'Scheduled',
        [Validators.required, Validators.maxLength(50)],
      ],
      shiftDescription: [
        this.employeeShift.shiftDescription,
        [Validators.maxLength(500)],
      ],
      shiftAssignedBy: [
        this.employeeShift.shiftAssignedBy,
        [Validators.maxLength(100)],
      ],
      overtimeHours: [
        this.employeeShift.overtimeHours,
        [Validators.min(0), Validators.max(12)],
      ],
      shiftCategory: [
        this.employeeShift.shiftCategory || 'Standard',
        [Validators.maxLength(50)],
      ],
      createdDate: [
        formatDate(this.employeeShift.createdDate, 'yyyy-MM-dd HH:mm:ss', 'en'),
      ],
      lastModifiedDate: [
        formatDate(
          this.employeeShift.lastModifiedDate,
          'yyyy-MM-dd HH:mm:ss',
          'en'
        ),
      ],
    });
  }

  // Generalized error message method
  getErrorMessage(controlName: string): string {
    const control = this.employeeShiftForm.get(controlName);
    if (!control) return '';

    if (control.hasError('required')) return 'This field is required';
    if (control.hasError('email')) return 'Please enter a valid email';
    if (control.hasError('pattern')) return 'Invalid format';
    if (control.hasError('timeRange'))
      return 'End time must be after start time';

    return '';
  }

  private parseTimeString(timeString: string | undefined): Date | null {
    if (!timeString) return null;
    const [hours, minutes] = timeString.split(':').map(Number);
    const date = new Date();
    date.setHours(hours, minutes, 0, 0);
    return date;
  }

  // Handle form submission
  submit(): void {
    if (this.employeeShiftForm.valid) {
      const employeeShiftData = this.employeeShiftForm.getRawValue();
      const timeFields = [
        'shiftStartTime',
        'shiftEndTime',
        'breakStartTime',
        'breakEndTime',
      ];

      // Format time fields
      timeFields.forEach((field) => {
        if (employeeShiftData[field] instanceof Date) {
          employeeShiftData[field] = this.formatTime(employeeShiftData[field]);
        }
      });

      if (this.action === 'edit') {
        this.employeeShiftService
          .updateEmployeeShift(employeeShiftData)
          .subscribe({
            next: (response) => {
              this.dialogRef.close(response);
            },
            error: (error) => {
              console.error('Update Error:', error);
              // Optionally show an error message to the user
            },
          });
      } else {
        this.employeeShiftService
          .addEmployeeShift(employeeShiftData)
          .subscribe({
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
  private formatTime(date: Date): string {
    return `${date.getHours().toString().padStart(2, '0')}:${date
      .getMinutes()
      .toString()
      .padStart(2, '0')}`;
  }
  onNoClick(): void {
    this.dialogRef.close();
  }
}
