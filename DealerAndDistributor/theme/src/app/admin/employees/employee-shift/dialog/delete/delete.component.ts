import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogTitle,
  MatDialogContent,
  MatDialogActions,
  MatDialogClose,
} from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { EmployeeShiftService } from '../../employee-shift.service';

export interface DialogData {
  shiftId: number;
  employeeName: string;
  shiftStartTime: string;
}

@Component({
    selector: 'app-employeeShift-delete',
    templateUrl: './delete.component.html',
    styleUrls: ['./delete.component.scss'],
    imports: [
        MatDialogTitle,
        MatDialogContent,
        MatDialogActions,
        MatButtonModule,
        MatDialogClose,
    ]
})
export class EmployeeShiftDeleteComponent {
  constructor(
    public dialogRef: MatDialogRef<EmployeeShiftDeleteComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public employeeShiftService: EmployeeShiftService
  ) {}
  confirmDelete(): void {
    this.employeeShiftService.deleteEmployeeShift(this.data.shiftId).subscribe({
      next: (response) => {
        // console.log('Delete Response:', response);
        this.dialogRef.close(response); // Close with the response data
        // Handle successful deletion, e.g., refresh the table or show a notification
      },
      error: (error) => {
        console.error('Delete Error:', error);
        // Handle the error appropriately
      },
    });
  }
}
