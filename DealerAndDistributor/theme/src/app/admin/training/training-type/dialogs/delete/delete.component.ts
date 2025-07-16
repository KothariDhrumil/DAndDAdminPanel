import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogTitle,
  MatDialogContent,
  MatDialogActions,
  MatDialogClose,
} from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { TrainingTypeService } from '../../training-type.service';
import { MatButtonModule } from '@angular/material/button';

export interface DialogData {
  trainingTypeId: number;
  trainingTypeName: string;
  category: string;
}

@Component({
    selector: 'app-trainingType-delete',
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
export class TrainingTypeDeleteComponent {
  constructor(
    public dialogRef: MatDialogRef<TrainingTypeDeleteComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public trainingTypeService: TrainingTypeService
  ) {}

  confirmDelete(): void {
    this.trainingTypeService
      .deleteTrainingType(this.data.trainingTypeId)
      .subscribe({
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
