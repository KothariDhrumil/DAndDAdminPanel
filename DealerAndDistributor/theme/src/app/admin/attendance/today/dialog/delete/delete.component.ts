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
import { TodayService } from '../../today.service';

export interface DialogData {
  id: number;
  name: string;
  first_in: string;
  last_out: string;
}

@Component({
  selector: 'app-today-delete',
  templateUrl: './delete.component.html',
  styleUrls: ['./delete.component.scss'],
  imports: [
    MatDialogTitle,
    MatDialogContent,
    MatDialogActions,
    MatButtonModule,
    MatDialogClose,
  ],
})
export class TodayDeleteComponent {
  constructor(
    public dialogRef: MatDialogRef<TodayDeleteComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public todayService: TodayService
  ) {}
  confirmDelete(): void {
    this.todayService.deleteToday(this.data.id).subscribe({
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
