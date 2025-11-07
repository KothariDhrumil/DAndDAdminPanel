import { ChangeDetectionStrategy, Component, Inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { TenantsService } from '../../../superadmin/tenants/service/tenants.service';
import { TenantUsersService, UpdateUserRequest } from '../../service/tenant-users.service';
import { Tenant } from '../../../superadmin/tenants/models/tenant.model';
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";

export interface UpdateUserTenantDialogData {
  userId: string;
  userName: string;
  currentTenantId?: number;
  currentTenantName?: string;
}

@Component({
  selector: 'app-update-user-tenant-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatFormFieldModule, MatSelectModule, MatDialogModule, MatButtonModule, MatProgressSpinnerModule],
  templateUrl: './update-user-tenant-dialog.component.html',
  styleUrls: ['./update-user-tenant-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UpdateUserTenantDialogComponent implements OnInit {
  tenants = signal<Tenant[]>([]);
  loading = signal(false);

  form = new FormGroup({
    tenantId: new FormControl<number | null>(null, { validators: [Validators.required] })
  });

  constructor(
    private dialogRef: MatDialogRef<UpdateUserTenantDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: UpdateUserTenantDialogData,
    private tenantsService: TenantsService,
    private usersService: TenantUsersService
  ) {}

  ngOnInit() {
    this.loadTenants();
    if (this.data?.currentTenantId) {
      this.form.patchValue({ tenantId: this.data.currentTenantId });
    }
  }

  private loadTenants() {
    this.loading.set(true);
    this.tenantsService.getTenants().subscribe({
      next: (res) => {
        if (res?.isSuccess && Array.isArray(res.data)) {
          this.tenants.set(res.data);
        }
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
      }
    });
  }

  save() {
    if (this.form.invalid) return;
    const tenantId = this.form.value.tenantId;
    const payload: UpdateUserRequest = { userId: this.data.userId, tenantId: tenantId ?? undefined } as any;
    this.usersService.updateUser(payload).subscribe({
      next: r => this.dialogRef.close(!!r?.isSuccess),
      error: () => this.dialogRef.close(false)
    });
  }

  close() {
    this.dialogRef.close();
  }
}