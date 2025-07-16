import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogTitle,
  MatDialogContent,
  MatDialogActions,
  MatDialogClose,
} from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { MyProjectsService } from '../../my-projects.service';
import { MatButtonModule } from '@angular/material/button';

export interface DialogData {
  id: number;
  pName: string;
  open_task: string;
  status: string;
}

@Component({
    selector: 'app-my-projects-delete',
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
export class MyProjectsDeleteComponent {
  constructor(
    public dialogRef: MatDialogRef<MyProjectsDeleteComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public myProjectsService: MyProjectsService
  ) {}
  confirmDelete(): void {
    this.myProjectsService.deleteMyProjects(this.data.id).subscribe({
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
