import { Component, Inject } from '@angular/core';
import { MatDialogContent, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { UpsertTenantComponent, UpsertTenantFormValue } from '../../../../../core/shared/components/upsert-tenant/upsert-tenant.component';
import { TenantsService } from '../../service/tenants.service';
import { AuthService } from '@core/index';
import { RegisterRequest } from '@core/models/interface/RegisterRequest';
import { ShardingService } from '../../../sharding/service/sharding.service';
import { ApiResponse } from '@core/models/interface/ApiResponse';
import { Sharding } from '../../../sharding/models/sharding.model';

@Component({
    selector: 'app-tenant-dialog',
    standalone: true,
    imports: [MatDialogContent, UpsertTenantComponent],
    templateUrl: './tenant-dialog.component.html',
    styleUrls: ['./tenant-dialog.component.scss']
})
export class TenantDialogComponent {
    shardingOptions: Array<{ name: string; connectionName: string }> = [];
    constructor(
        private dialogRef: MatDialogRef<TenantDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: { item?: any; action?: 'add' | 'edit' },
        private authService: AuthService,
        private shardingService: ShardingService
    ) {
        // Load sharding options for dropdown
        this.shardingService.getAll().subscribe({
            next: (res: ApiResponse<Sharding[]>) => {
                this.shardingOptions = (res?.data ?? []).map(s => ({ name: s.name, connectionName: s.connectionName }));
            },
            error: () => {
                this.shardingOptions = [];
            }
        });
    }

    onSubmitted(value: UpsertTenantFormValue) {
        // For now, create a top-level tenant using the company name
        const name = value.tenantName?.trim();
        if (!name) {
            this.dialogRef.close();
            return;
        }
        this.authService.register(value).subscribe({
            next: (res) => this.dialogRef.close(res),
            error: () => this.dialogRef.close()
        });
    }
}
