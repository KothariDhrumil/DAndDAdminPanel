import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogTitle,
  MatDialogContent,
  MatDialogActions,
  MatDialogClose,
} from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { MyLeavesService } from '../../my-leaves.service';
import { MatButtonModule } from '@angular/material/button';

export interface DialogData {
  id: number;
  type: string;
  status: string;
  reason: string;
}

@Component({
    selector: 'app-my-leaves-delete',
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
export class MyLeavesDeleteComponent {
  constructor(
    public dialogRef: MatDialogRef<MyLeavesDeleteComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public myLeavesService: MyLeavesService
  ) {}

  confirmDelete(): void {
    this.myLeavesService.deleteMyLeaves(this.data.id).subscribe({
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
