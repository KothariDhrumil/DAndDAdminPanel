import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { AuthUsersService, CreateUserRequest, UpdateUserRequest } from '../../service/auth-users.service';

export interface AddUserDialogData { userId?: string | null; tenantId?: number }

@Component({
  selector: 'app-add-user-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatDialogModule, MatButtonModule],
  templateUrl: './add-user-dialog.component.html',
  styleUrls: ['./add-user-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AddUserDialogComponent {
  form: FormGroup<{
    userId: FormControl<string | null>;
    firstName: FormControl<string>;
    lastName: FormControl<string>;
    phoneNumber: FormControl<string>;
  }> = new FormGroup({
    userId: new FormControl<string | null>({ value: null, disabled: true }, { nonNullable: false }),
    firstName: new FormControl<string>('', { nonNullable: true }),
    lastName: new FormControl<string>('', { nonNullable: true }),
    phoneNumber: new FormControl<string>('', { nonNullable: true }),
  });

  constructor(
    private dialogRef: MatDialogRef<AddUserDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: AddUserDialogData,
    private usersService: AuthUsersService
  ) {
    if (data?.userId) {
      this.form.patchValue({ userId: data.userId });
    }
  }

  save() {
    if (this.form.invalid) return;
    const raw = this.form.getRawValue();
    if (raw.userId) {
      const update: UpdateUserRequest = {
        userId: raw.userId,
        firstName: raw.firstName,
        lastName: raw.lastName,
        phoneNumber: raw.phoneNumber
      } as any;
      this.usersService.updateUser(update).subscribe({
        next: r => this.dialogRef.close(!!r?.isSuccess),
        error: () => this.dialogRef.close(false)
      });
    } else {
      const create: CreateUserRequest = {
        firstName: raw.firstName,
        lastName: raw.lastName,
        phoneNumber: raw.phoneNumber,
        tenantId: this.data?.tenantId
      } as any;
      this.usersService.createUser(create).subscribe({
        next: r => this.dialogRef.close(!!r?.isSuccess),
        error: () => this.dialogRef.close(false)
      });
    }
  }
  close() { this.dialogRef.close(); }
}
