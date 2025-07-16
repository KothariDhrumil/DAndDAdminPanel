import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogTitle,
  MatDialogContent,
  MatDialogActions,
  MatDialogClose,
} from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { TrainersService } from '../../trainers.service';
import { MatButtonModule } from '@angular/material/button';

export interface DialogData {
  trainer_id: number;
  name: string;
  phone_number: string;
}

@Component({
    selector: 'app-trainers-delete',
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
export class TrainersDeleteComponent {
  constructor(
    public dialogRef: MatDialogRef<TrainersDeleteComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public trainersService: TrainersService
  ) {}

  confirmDelete(): void {
    this.trainersService.deleteTrainer(this.data.trainer_id).subscribe({
      next: (response) => {
        // Handle successful deletion
        this.dialogRef.close(response); // Close the dialog with the response
        // Optionally, refresh a list or show a notification
      },
      error: (error) => {
        console.error('Delete Error:', error);
        // Handle error appropriately
      },
    });
  }
}
