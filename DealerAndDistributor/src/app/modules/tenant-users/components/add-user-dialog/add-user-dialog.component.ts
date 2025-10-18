import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { TenantUsersService, CreateUserRequest, UpdateUserRequest } from '../../service/tenant-users.service';
import { UserTypeService } from '../../user-types/service/user-type.service';
import { MatSelectModule } from "@angular/material/select";

export interface AddUserDialogData {
  userId?: string | null;
  tenantId?: number;
  firstName?: string | null;
  lastName?: string | null;
  phoneNumber?: string | null;
  userTypeId?: number | null;
}

@Component({
  selector: 'app-add-user-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatDialogModule, MatButtonModule, MatSelectModule],

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
    userTypeId: FormControl<number | null>;
  }> = new FormGroup({
    userId: new FormControl<string | null>({ value: null, disabled: true }, { nonNullable: false }),
    firstName: new FormControl<string>('', { nonNullable: true }),
    lastName: new FormControl<string>('', { nonNullable: true }),
    phoneNumber: new FormControl<string>('', { nonNullable: true }),
    userTypeId: new FormControl<number | null>(null, { nonNullable: false }),
  });

  userTypes: { id: number; name: string }[] = [];
  constructor(
    private dialogRef: MatDialogRef<AddUserDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: AddUserDialogData,
    private usersService: TenantUsersService,
    private userTypeService: UserTypeService
  ) {
    if (data?.userId) {
      this.form.patchValue({ userId: data.userId });
    }
    if (data?.firstName) {
      this.form.patchValue({ firstName: data.firstName });
    }
    if (data?.lastName) {
      this.form.patchValue({ lastName: data.lastName });
    }
    if (data?.phoneNumber) {
      this.form.patchValue({ phoneNumber: data.phoneNumber });
    }
    if (data?.userTypeId) {
      this.form.patchValue({ userTypeId: data.userTypeId });
    }
    // Load user types for dropdown using DI
    this.userTypeService.list().subscribe(res => {
      if (Array.isArray(res.data)) {
        this.userTypes = res.data.map(ut => ({ id: ut.userTypeId, name: ut.name }));
      }
    });
  }

  save() {
    if (this.form.invalid) return;
    const raw = this.form.getRawValue();
    if (raw.userId) {
      const update: UpdateUserRequest = {
        userId: raw.userId,
        firstName: raw.firstName,
        lastName: raw.lastName,
        phoneNumber: raw.phoneNumber,
        userTypeId: raw.userTypeId ?? undefined
      } as any;
      this.usersService.updateNameUser(update).subscribe({
        next: r => this.dialogRef.close(!!r?.isSuccess),
        error: () => this.dialogRef.close(false)
      });
    } else {
      const create: CreateUserRequest = {
        firstName: raw.firstName,
        lastName: raw.lastName,
        phoneNumber: raw.phoneNumber,
        tenantId: this.data?.tenantId,
        userTypeId: raw.userTypeId ?? undefined
      } as any;
      this.usersService.createUser(create).subscribe({
        next: r => this.dialogRef.close(!!r?.isSuccess),
        error: () => this.dialogRef.close(false)
      });
    }
  }
  close() { this.dialogRef.close(); }
}
