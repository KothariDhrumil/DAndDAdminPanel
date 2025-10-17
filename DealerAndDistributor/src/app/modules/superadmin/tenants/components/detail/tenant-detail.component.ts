import { ChangeDetectionStrategy, Component, OnInit, signal } from '@angular/core';
import { CommonModule, NgOptimizedImage } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatTabsModule } from '@angular/material/tabs';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatDividerModule } from '@angular/material/divider';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { BreadcrumbComponent } from '../../../../../core/shared/components/breadcrumb/breadcrumb.component';
import { ActivatedRoute } from '@angular/router';
import { TenantDetailService } from '../../service/tenant-detail.service';
import { TenantPlanItem } from '../../models/tenant-plan.model';
import { Tenant } from '../../models/tenant.model';
import { MatCheckbox } from "@angular/material/checkbox";
import { MatInputModule } from "@angular/material/input";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { AddTenantPlanDialogComponent } from './add-tenant-plan-dialog/add-tenant-plan-dialog.component';
import { GenericTableComponent } from '../../../../../core/shared/components/generic-table/generic-table.component';
import { ColumnDefinition, TableConfig, TableEventArgs, RowAction } from '../../../../../core/shared/components/generic-table/generic-table.model';
import { TenantsService } from '../../service/tenants.service';
import { AddChildTenantDialogComponent } from './add-child-tenant-dialog/add-child-tenant-dialog.component';
import { RenameTenantDialogComponent } from './rename-tenant-dialog/rename-tenant-dialog.component';
import { Router } from '@angular/router';
import { SUPERADMIN_TENANT_DETAIL_ROUTE } from '../../../../../core/helpers/routes/app-routes';
import { AddUserDialogComponent } from 'src/app/modules/tenant-users/components/add-user-dialog/add-user-dialog.component';
import { UpdateUserRolesDialogComponent } from 'src/app/modules/tenant-users/components/update-user-roles-dialog/update-user-roles-dialog.component';
import { UpdateUserTenantDialogComponent } from 'src/app/modules/tenant-users/components/update-user-tenant-dialog/update-user-tenant-dialog.component';
import { AuthUserItem } from 'src/app/modules/tenant-users/models/tenant-user.model';
import { TenantUsersService } from 'src/app/modules/tenant-users/service/tenant-users.service';


@Component({
    selector: 'app-tenant-detail',
    templateUrl: './tenant-detail.component.html',
    styleUrls: ['./tenant-detail.component.scss'],
    standalone: true,
    imports: [
        CommonModule,
        MatCardModule,
        MatTabsModule,
        MatIconModule,
        MatListModule,
        MatDividerModule,
        MatButtonModule,
        MatChipsModule,
        BreadcrumbComponent,
        MatCheckbox,
        MatInputModule,
        MatFormFieldModule,
        MatProgressSpinnerModule,
        MatDialogModule,
        GenericTableComponent,
    ],
    changeDetection: ChangeDetectionStrategy.OnPush,
    host: {
        class: 'tenant-detail-page'
    }
})
export class TenantDetailComponent implements OnInit {
    // Data signals
    readonly tenant = signal<Tenant | null>(null);
    readonly activePlan = signal<TenantPlanItem | null>(null);
    readonly planHistory = signal<TenantPlanItem[]>([]);

    // Loading and error states
    readonly loadingTenant = signal(false);
    readonly errorTenant = signal<string | null>(null);
    readonly loadingActivePlan = signal(false);
    readonly errorActivePlan = signal<string | null>(null);
    readonly loadingHistory = signal(false);
    readonly errorHistory = signal<string | null>(null);

    // Child tenants state
    readonly childTenants = signal<Tenant[]>([]);
    readonly loadingChildren = signal(false);
    readonly errorChildren = signal<string | null>(null);
    readonly childColumns = signal<ColumnDefinition[]>([
        { def: 'tenantId', label: 'ID', type: 'text', sortable: true },
        { def: 'tenantFullName', label: 'Full Name', type: 'text', sortable: true },
        { def: 'tenantName', label: 'Name', type: 'text', sortable: true },
        { def: 'dataKey', label: 'Data Key', type: 'text' },
        { def: 'hasOwnDb', label: 'Own DB', type: 'check' },
        { def: 'databaseInfoName', label: 'Database', type: 'text' },
        { def: 'parentId', label: 'Parent ID', type: 'text' },
        { def: 'actions', label: 'Actions', type: 'actionBtn' }
    ]);
    readonly childTableConfig: TableConfig = {
        enableSelection: true,
        enableSearch: true,
        enableExport: false,
        enableRefresh: true,
        enableColumnHide: true,
        enableAdd: true,
        enableEdit: true,
        enableDelete: false,
        enableContextMenu: false,
        pageSize: 10,
        pageSizeOptions: [5, 10, 25, 50, 100],
        title: 'Child Tenants'
    };

    // Tenant users
    readonly tenantUsers = signal<AuthUserItem[]>([]);
    readonly loadingUsers = signal(false);
    readonly errorUsers = signal<string | null>(null);
    readonly userColumns = signal<ColumnDefinition[]>([
        // { def: 'userName', label: 'User Name', type: 'text', sortable: true },
        // { def: 'email', label: 'Email', type: 'email', sortable: true },
        // { def: 'hasTenant', label: 'Has Tenant', type: 'check' },

        //{ def: 'tenantName', label: 'Tenant', type: 'text' },
        { def: 'firstName', label: 'First Name', type: 'text', sortable: true },
        { def: 'lastName', label: 'Last Name', type: 'text', sortable: true },
        { def: 'phoneNumber', label: 'Phone Number', type: 'text', sortable: true },
        { def: 'roleNames', label: 'Roles', type: 'text' },
        { def: 'actions', label: 'Actions', type: 'actionBtn' },
    ]);
    readonly userTableConfig: TableConfig = {
        enableSelection: false,
        enableSearch: true,
        enableExport: false,
        enableRefresh: true,
        enableColumnHide: true,
        enableAdd: true,
        enableEdit: true,
        enableDelete: false,
        enableContextMenu: false,
        pageSize: 10,
        pageSizeOptions: [5, 10, 25, 50, 100],
        title: 'Users'
    };

    readonly userRowActions = signal<RowAction[]>([
        { name: 'profile', icon: 'user', tooltip: 'Edit Profile', color: 'primary' },
        { name: 'tenant', icon: 'home', tooltip: 'Change Tenant', color: 'accent' },
    ]);

    constructor(
        private readonly route: ActivatedRoute,
        private readonly service: TenantDetailService,
        private readonly dialog: MatDialog,
        private readonly tenantsService: TenantsService,
        private readonly router: Router,
        private readonly tenantUsersService: TenantUsersService
    ) { }

    ngOnInit(): void {
        this.route.paramMap.subscribe(pm => {
            const idParam = pm.get('id');
            if (!idParam) return; // no id provided
            const tenantId = Number(idParam);
            if (Number.isNaN(tenantId)) return;

            // Load tenant
            this.loadingTenant.set(true);
            this.errorTenant.set(null);
            this.service.getTenantById(tenantId).subscribe({
                next: res => {
                    if (res?.isSuccess && res.data) {
                        this.tenant.set(res.data);
                    } else {
                        this.errorTenant.set('Failed to load tenant');
                    }
                    this.loadingTenant.set(false);
                },
                error: _ => {
                    this.errorTenant.set('Failed to load tenant');
                    this.loadingTenant.set(false);
                }
            });

            // Load active plan, then plan history
            this.loadingActivePlan.set(true);
            this.errorActivePlan.set(null);
            this.service.getActivePlan(tenantId).subscribe({
                next: ap => {
                    if (ap?.isSuccess && ap.data) {
                        this.activePlan.set(ap.data);
                    } else {
                        this.errorActivePlan.set('No active plan');
                    }
                    this.loadingActivePlan.set(false);
                },
                error: _ => {
                    this.errorActivePlan.set('Failed to load active plan');
                    this.loadingActivePlan.set(false);
                }
            });

            this.loadPlanHistory(tenantId);
            this.loadChildTenants(tenantId);
            this.loadTenantUsers(tenantId);
        });
    }

    private loadPlanHistory(key: number) {
        this.loadingHistory.set(true);
        this.errorHistory.set(null);
        this.service.getPlanHistory(key).subscribe({
            next: ph => {
                const data = ph?.data as any;
                if (!data) { this.planHistory.set([]); this.errorHistory.set('No plan history'); }
                else {
                    // Handle both array or single object responses gracefully
                    this.planHistory.set(Array.isArray(data) ? data as TenantPlanItem[] : [data as TenantPlanItem]);
                }
                this.loadingHistory.set(false);
            },
            error: _ => {
                this.errorHistory.set('Failed to load plan history');
                this.loadingHistory.set(false);
            }
        });
    }

    openAddPlan(): void {
        const tenantId = this.tenant()?.tenantId ?? Number(this.route.snapshot.paramMap.get('id'));
        if (!tenantId) return;
        const hasActive = !!this.activePlan();
        const ref = this.dialog.open(AddTenantPlanDialogComponent, { data: { tenantId, hasActivePlan: hasActive }, width: '640px' });
        ref.afterClosed().subscribe(ok => {
            if (ok) {
                // Refresh active plan and history
                this.ngOnInit();
            }
        });
    }

    openEditPlan(): void {
        const tenantId = this.tenant()?.tenantId ?? Number(this.route.snapshot.paramMap.get('id'));
        if (!tenantId) return;
        const ap = this.activePlan();
        if (!ap) return;
        const ref = this.dialog.open(AddTenantPlanDialogComponent, {
            data: { tenantId, hasActivePlan: true, tenantPlanId: ap.tenantPlanId, activePlan: ap },
            width: '640px'
        });
        ref.afterClosed().subscribe(ok => {
            if (ok) {
                this.ngOnInit();
            }
        });
    }

    // Child tenants
    private loadChildTenants(parentId: number) {
        this.loadingChildren.set(true);
        this.errorChildren.set(null);
        this.tenantsService.getChildTenants(parentId).subscribe({
            next: (res) => {
                if (res?.isSuccess && Array.isArray(res.data)) {
                    this.childTenants.set(res.data);
                } else {
                    this.childTenants.set([]);
                    this.errorChildren.set('No child tenants');
                }
                this.loadingChildren.set(false);
            },
            error: _ => { this.errorChildren.set('Failed to load child tenants'); this.loadingChildren.set(false); }
        });
    }

    onChildTableEvent(event: any) {
        const parentId = this.tenant()?.tenantId ?? Number(this.route.snapshot.paramMap.get('id'));
        switch (event?.type) {
            case 'refresh':
                if (parentId) this.loadChildTenants(parentId);
                break;
            case 'row':
            case 'edit':
                if (event?.data?.tenantId) this.routeToTenant(event.data.tenantId);
                break;
            case 'add':
                this.openAddChild();
                break;
        }
    }

    private openAddChild() {
        const parentId = this.tenant()?.tenantId ?? Number(this.route.snapshot.paramMap.get('id'));
        if (!parentId) return;
        const ref = this.dialog.open(AddChildTenantDialogComponent, { data: { parentTenantId: parentId } });
        ref.afterClosed().subscribe(ok => { if (ok) this.loadChildTenants(parentId); });
    }

    private routeToTenant(tenantId: number) {
        this.router.navigate([`${SUPERADMIN_TENANT_DETAIL_ROUTE}/${tenantId}`]);
    }

    // Users
    private loadTenantUsers(tenantId: number) {
        this.loadingUsers.set(true);
        this.errorUsers.set(null);
        this.tenantUsersService.listUsersByTenant(tenantId).subscribe({
            next: (res) => {
                if (res?.isSuccess && Array.isArray(res.data)) this.tenantUsers.set(res.data);
                else { this.tenantUsers.set([]); this.errorUsers.set('No users found'); }
                this.loadingUsers.set(false);
            },
            error: _ => { this.errorUsers.set('Failed to load users'); this.loadingUsers.set(false); }
        });
    }

    onUsersTableEvent(event: TableEventArgs) {
        const tenantId = this.tenant()?.tenantId ?? Number(this.route.snapshot.paramMap.get('id'));
        if (!tenantId) return;
        switch (event.type) {
            case 'refresh':
                this.loadTenantUsers(tenantId);
                break;
            case 'add':
                this.openAddUserDialog(tenantId);
                break;
            case 'custom':
                if (event.data && event.action) {
                    if (event.action === 'profile') this.openUpdateUserProfileDialog(event.data as AuthUserItem);
                    if (event.action === 'tenant') this.openUpdateUserTenantDialog(event.data as AuthUserItem);
                }
                break;
            case 'edit':
                if (event.data?.userId) {
                    this.openUpdateUserRolesDialog(event.data);
                }
                break;
        }
    }

    private openAddUserDialog(tenantId: number) {
        const ref = this.dialog.open(AddUserDialogComponent, { data: { tenantId }, width: '640px' });
        ref.afterClosed().subscribe(ok => { if (ok) this.loadTenantUsers(tenantId); });
    }

    private openUpdateUserRolesDialog(user: AuthUserItem) {
        const ref = this.dialog.open(UpdateUserRolesDialogComponent, {
            width: '520px',
            data: { userId: user.userId, selectedRoleIds: [] } // TODO: map current roles to ids if available
        });
        const tenantId = this.tenant()?.tenantId ?? Number(this.route.snapshot.paramMap.get('id'));
        ref.afterClosed().subscribe(ok => { if (ok && tenantId) this.loadTenantUsers(tenantId); });
    }

    private openUpdateUserProfileDialog(user: AuthUserItem) {
        const tenantId = this.tenant()?.tenantId ?? Number(this.route.snapshot.paramMap.get('id'));
        const ref = this.dialog.open(AddUserDialogComponent, {
            data: {
                userId: user.userId,
                firstName: user.firstName,
                lastName: user.lastName,
                phoneNumber: user.phoneNumber
            }, width: '640px'
        });
        ref.afterClosed().subscribe(ok => { if (ok && tenantId) this.loadTenantUsers(tenantId); });
    }

    private openUpdateUserTenantDialog(user: AuthUserItem) {
        const tenantId = this.tenant()?.tenantId ?? Number(this.route.snapshot.paramMap.get('id'));
        const ref = this.dialog.open(UpdateUserTenantDialogComponent, {
            width: '500px',
            data: { userId: user.userId, userName: user.userName, currentTenantName: user.tenantName }
        });
        ref.afterClosed().subscribe(ok => { if (ok && tenantId) this.loadTenantUsers(tenantId); });
    }

    // Rename tenant
    openRenameTenantDialog() {
        const current = this.tenant();
        if (!current) return;
        const ref = this.dialog.open(RenameTenantDialogComponent, {
            data: { tenantName: current.tenantName },
            width: '420px'
        });
        ref.afterClosed().subscribe(result => {
            if (result?.tenantName && result.tenantName !== current.tenantName) {
                this.renameTenant(current.tenantId, result.tenantName);
            }
        });
    }

    private renameTenant(tenantId: number, tenantName: string) {
        this.loadingTenant.set(true);
        this.errorTenant.set(null);
        this.tenantsService.renameTenant(tenantId, tenantName).subscribe({
            next: res => {
                // Reload everything to reflect changes
                this.ngOnInit();
            },
            error: _ => {
                this.errorTenant.set('Failed to rename tenant');
                this.loadingTenant.set(false);
            }
        });
    }
}
