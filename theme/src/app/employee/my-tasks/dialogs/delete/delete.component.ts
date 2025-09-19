import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogTitle,
  MatDialogContent,
  MatDialogActions,
  MatDialogClose,
} from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { MyTasksService } from '../../my-tasks.service';
import { MatButtonModule } from '@angular/material/button';

export interface DialogData {
  id: number;
  taskNo: string;
  status: string;
  project: string;
}

@Component({
    selector: 'app-my-tasks-delete',
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
export class MyTasksDeleteComponent {
  constructor(
    public dialogRef: MatDialogRef<MyTasksDeleteComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public myTasksService: MyTasksService
  ) {}

  confirmDelete(): void {
    this.myTasksService.deleteMyTasks(this.data.id).subscribe({
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
