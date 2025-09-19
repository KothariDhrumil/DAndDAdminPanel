import { Component, ChangeDetectionStrategy, computed, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginatedApiResponse } from '../../../../core/models/interface/ApiResponse';
import { GenericTableComponent } from '../../../../core/shared/components/generic-table/generic-table.component';
import { ColumnDefinition, TableConfig, TableEventArgs } from '../../../../core/shared/components/generic-table/generic-table.model';
import { BreadcrumbComponent } from '../../../../core/shared/components/breadcrumb/breadcrumb.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTooltipModule } from '@angular/material/tooltip';

import { RolesService } from '../../service/roles.service';
import { Role, RoleTypes } from '../../models/role.model';

@Component({
    selector: 'app-roles-and-permissions',
    templateUrl: './roles-and-permissions.component.html',
    styleUrls: ['./roles-and-permissions.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
    standalone: true,
    imports: [
        CommonModule,
        GenericTableComponent,
        BreadcrumbComponent,
        MatProgressSpinnerModule,
        MatTooltipModule
    ],
    host: {
        'class': 'roles-and-permissions'
    }
})
export class RolesAndPermissionsComponent implements OnInit {
    readonly rolesResponse = signal<PaginatedApiResponse<Role[]> | null>(null);
    readonly loading = signal<boolean>(false);

    readonly columns = signal<ColumnDefinition[]>([
        {
            def: 'roleId',
            label: 'Role Id',
            type: 'text',
            sortable: true,
        },
        {
            def: 'roleName',
            label: 'Role Name',
            type: 'text',
            sortable: true
        },
        {
            def: 'description',
            label: 'Description',
            type: 'text',
            sortable: true,
            tooltip: true
        },
        {
            def: 'roleType',
            label: 'Role Type',
            type: 'text',
            sortable: true
        },
        {
            def: 'permissionNamesFormatted',
            label: 'Permissions',
            type: 'text',
            sortable: false,
            tooltip: true
        }
    ]);

    readonly tableConfig = signal<TableConfig>({
        enableSearch: true,
        enableRefresh: true,
        pageSizeOptions: [5, 10, 25, 50],
        title: 'Roles and Permissions',
        enableAdd: true,
        enableEdit: true,
        enableDelete: true,
        pageSize: 10,
    });

    constructor(private readonly rolesService: RolesService) { }

    ngOnInit(): void {
        this.fetchRoles(1, 10);
    }

    fetchRoles(pageNumber: number, pageSize: number): void {
        this.loading.set(true);
        this.rolesService.getRoles(pageNumber, pageSize).subscribe({
            next: (response: PaginatedApiResponse<Role[]>) => {
                // Format the data for display before setting it
                const formattedRoles = response.data.map(role => ({
                    ...role,
                    roleType: this.formatRoleType(role.roleType as RoleTypes),
                    // Format permissions for display
                    permissionNamesFormatted: this.formatPermissions(role.permissionNames)
                }));

                response.data = formattedRoles;
                this.rolesResponse.set(response);
                this.loading.set(false);
            },
            error: () => {
                this.loading.set(false);
            }
        });
    }

    // Helper methods to format data
    private formatRoleType(roleType: RoleTypes): string {
        switch (roleType) {
            case RoleTypes.Normal:
                return 'Normal';
            case RoleTypes.TenantAutoAdd:
                return 'Tenant Auto Add';
            case RoleTypes.TenantAdminAdd:
                return 'Tenant Admin Add';
            case RoleTypes.TenantCreated:
                return 'Tenant Created';
            case RoleTypes.FeatureRole:
                return 'Feature Role';
            case RoleTypes.HiddenFromTenant:
                return 'Hidden From Tenant';
            default:
                return `Role Type ${roleType}`;
        }
    }

    private formatPermissions(permissions: string[] | null | undefined): string {
        if (!permissions || permissions.length === 0) {
            return 'No permissions';
        }

        // Return first 3 permissions + a count of remaining ones
        if (permissions.length > 3) {
            const visiblePermissions = permissions.slice(0, 3).join(', ');
            return `${visiblePermissions}... (+${permissions.length - 3} more)`;
        }

        return permissions.join(', ');
    }

    readonly roles = computed(() => this.rolesResponse()?.data ?? []);
    readonly totalRecords = computed(() => this.rolesResponse()?.totalRecords ?? 0);

    onTableEvent(event: TableEventArgs): void {
        if (event.type === 'page') {
            const pageInfo = event.data;
            this.fetchRoles(pageInfo.pageIndex + 1, pageInfo.pageSize);
        } else if (event.type === 'refresh') {
            this.fetchRoles(1, 10);
        }
    }
}
