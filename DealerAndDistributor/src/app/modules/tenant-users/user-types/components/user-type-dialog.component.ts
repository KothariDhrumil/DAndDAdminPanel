import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { UserTypeService } from '../service/user-type.service';
import { UserType } from '../models/user-type.model';

@Component({
  selector: 'app-user-type-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatDialogModule],
  templateUrl: './user-type-dialog.component.html',
  styleUrls: ['./user-type-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UserTypeDialogComponent {
  form: FormGroup;
  loading = false;
  isEdit = false;

  constructor(
    private dialogRef: MatDialogRef<UserTypeDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { userType?: UserType },
    private service: UserTypeService,
    private fb: FormBuilder
  ) {
    this.isEdit = !!data?.userType;
    this.form = this.fb.group({
      name: [data?.userType?.name || '', [Validators.required, Validators.maxLength(100)]],
      description: [data?.userType?.description || '', [Validators.maxLength(250)]]
    });
  }

  save() {
    if (this.form.invalid) return;
    this.loading = true;
    const value = this.form.value;
    const req$ = this.isEdit
      ? this.service.update(this.data.userType!.userTypeId, value)
      : this.service.create(value);
    req$.subscribe({
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
