import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogTitle,
  MatDialogContent,
  MatDialogActions,
  MatDialogClose,
} from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { TrainingListService } from '../../training-list.service';
import { MatButtonModule } from '@angular/material/button';

export interface DialogData {
  id: number;
  trainingType: string;
  trainer: string;
}

@Component({
    selector: 'app-trainingList-delete',
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
export class TrainingListDeleteComponent {
  constructor(
    public dialogRef: MatDialogRef<TrainingListDeleteComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public trainingListService: TrainingListService
  ) {}

  confirmDelete(): void {
    this.trainingListService.deleteTrainingList(this.data.id).subscribe({
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
