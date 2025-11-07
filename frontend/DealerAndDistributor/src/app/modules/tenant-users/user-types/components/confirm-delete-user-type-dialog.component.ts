import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { UserTypeService } from '../service/user-type.service';
import { UserType } from '../models/user-type.model';

@Component({
  selector: 'app-confirm-delete-user-type-dialog',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule],
  templateUrl: './confirm-delete-user-type-dialog.component.html',
  styleUrls: ['./confirm-delete-user-type-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ConfirmDeleteUserTypeDialogComponent {
  loading = false;
  constructor(
    private dialogRef: MatDialogRef<ConfirmDeleteUserTypeDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { userType: UserType },
    private service: UserTypeService
  ) {}

  delete() {
    this.loading = true;
    this.service.delete(this.data.userType.userTypeId).subscribe({
      next: res => {
        this.loading = false;
        if (res.isSuccess) this.dialogRef.close(true);
      },
      error: _ => { this.loading = false; }
    });
  }

  cancel() {
    this.dialogRef.close(false);
  }
}
