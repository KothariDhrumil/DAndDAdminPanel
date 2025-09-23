import { Component, Inject } from '@angular/core';
import { MatDialogContent, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { UpsertTenantComponent, UpsertTenantFormValue } from '../../../../../core/shared/components/upsert-tenant/upsert-tenant.component';
import { TenantsService } from '../../service/tenants.service';
import { AuthService } from '@core/index';
import { RegisterRequest } from '@core/models/interface/RegisterRequest';

@Component({
    selector: 'app-tenant-dialog',
    standalone: true,
    imports: [MatDialogContent, UpsertTenantComponent],
    templateUrl: './tenant-dialog.component.html',
    styleUrls: ['./tenant-dialog.component.scss']
})
export class TenantDialogComponent {
    constructor(
        private dialogRef: MatDialogRef<TenantDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: { item?: any; action?: 'add' | 'edit' },
        private authService: AuthService
    ) { }

    onSubmitted(value: UpsertTenantFormValue) {
        // For now, create a top-level tenant using the company name
        const name = value.tenantName?.trim();
        if (!name) {
            this.dialogRef.close();
            return;
        }
        let registerRequest: RegisterRequest = {
            tenantName: name,
            phoneNumber: value.phoneNumber,
            password: value.password,
            designationId: value.designationId,
            firstName: value.firstName,
            lastName: value.lastName

        }


        this.authService.register(registerRequest).subscribe({
            next: (res) => this.dialogRef.close(res),
            error: () => this.dialogRef.close()
        });
    }
}
