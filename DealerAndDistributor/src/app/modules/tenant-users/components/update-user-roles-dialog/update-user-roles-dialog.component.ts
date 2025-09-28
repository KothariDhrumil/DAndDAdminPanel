import { ChangeDetectionStrategy, Component, Inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormControl } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { RolesSelectorComponent } from '../../../../core/shared/components/roles-selector/roles-selector.component';
import { TenantUsersService, UpdateUserRequest } from '../../service/tenant-users.service';

export interface UpdateUserRolesData {
  userId: string;
  selectedRoleIds: number[];
}

@Component({
  selector: 'app-update-user-roles-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatDialogModule, MatButtonModule, RolesSelectorComponent],
  template: `
    <h2 mat-dialog-title>Update Roles</h2>
    <form [formGroup]="form" (ngSubmit)="save()">
      <div mat-dialog-content>
        <app-roles-selector formControlName="roleIds"></app-roles-selector>
      </div>
      <div mat-dialog-actions align="end">
        <button type="button" mat-stroked-button (click)="close()">Cancel</button>
        <button type="submit" mat-flat-button color="primary">Save</button>
      </div>
    </form>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UpdateUserRolesDialogComponent {
  roles = signal<number[]>([]);
  form = new FormGroup({
    roleIds: new FormControl<number[]>([], { nonNullable: true })
  });
  constructor(
    private dialogRef: MatDialogRef<UpdateUserRolesDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: UpdateUserRolesData,
    private usersService: TenantUsersService
  ) {
    const initial = data?.selectedRoleIds ?? [];
    this.roles.set(initial);
    this.form.controls['roleIds'].setValue(initial);
  }
  save() {
    const roleIds = this.form.controls['roleIds'].value ?? [];
    const payload: UpdateUserRequest = { userId: this.data.userId, roleIds } as any;
    this.usersService.updateUser(payload).subscribe({
      next: r => this.dialogRef.close(!!r?.isSuccess),
      error: () => this.dialogRef.close(false)
    });
  }
  close() { this.dialogRef.close(); }
}
