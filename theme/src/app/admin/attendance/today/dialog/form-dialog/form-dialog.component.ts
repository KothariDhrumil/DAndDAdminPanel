import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogContent,
  MatDialogClose,
} from '@angular/material/dialog';
import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import {
  Validators,
  UntypedFormGroup,
  UntypedFormBuilder,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { Today } from '../../today.model';
import { TodayService } from '../../today.service';
import { provideNativeDateAdapter } from '@angular/material/core';
import { MatTimepickerModule } from '@angular/material/timepicker';

export interface DialogData {
  id: number;
  action: string;
  today: Today;
}

@Component({
  selector: 'app-today-form',
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
    MatSelectModule,
    MatDatepickerModule,
    MatDialogClose,
    MatTimepickerModule,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TodayFormComponent {
  action: string;
  dialogTitle: string;
  todayForm: UntypedFormGroup;
  today: Today;

  constructor(
    public dialogRef: MatDialogRef<TodayFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public todayService: TodayService,
    private fb: UntypedFormBuilder
  ) {
    // Set the defaults
    this.action = data.action;
    this.today = this.action === 'edit' ? data.today : new Today({} as Today);
    this.dialogTitle =
      this.action === 'edit' ? `${this.today.name}` : 'New Entry'; // Updated title for clarity
    this.todayForm = this.createTodayForm();
  }

  private createTodayForm(): UntypedFormGroup {
    return this.fb.group({
      id: [this.today.id],
      img: [this.today.img],
      name: [this.today.name, [Validators.required]],
      first_in: [
        this.parseTimeString(this.today.first_in),
        [Validators.required],
      ],
      break: [this.parseTimeString(this.today.break), [Validators.required]],
      last_out: [
        this.parseTimeString(this.today.last_out),
        [Validators.required],
      ],
      total: [this.today.total],
      status: [this.today.status, [Validators.required]],
      shift: [this.today.shift, [Validators.required]],
    });
  }

  getErrorMessage(controlName: string): string {
    const control = this.todayForm.get(controlName);
    if (control?.hasError('required')) {
      return 'Required field';
    }
    // Add other validations if necessary
    return '';
  }

  private parseTimeString(timeString: string | undefined): Date | null {
    if (!timeString) return null;
    const [hours, minutes] = timeString.split(':').map(Number);
    const date = new Date();
    date.setHours(hours, minutes, 0, 0);
    return date;
  }

  submit(): void {
    if (this.todayForm.valid) {
      const todayData = this.todayForm.getRawValue();
      const timeFields = ['first_in', 'break', 'last_out'];

      // Format time fields
      timeFields.forEach((field) => {
        if (todayData[field] instanceof Date) {
          todayData[field] = this.formatTime(todayData[field]);
        }
      });
      if (this.action === 'edit') {
        this.todayService.updateToday(todayData).subscribe({
          next: (response) => {
            this.dialogRef.close(response);
          },
          error: (error) => {
            console.error('Update Error:', error);
            // Optionally show an error message to the user
          },
        });
      } else {
        this.todayService.addToday(todayData).subscribe({
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
