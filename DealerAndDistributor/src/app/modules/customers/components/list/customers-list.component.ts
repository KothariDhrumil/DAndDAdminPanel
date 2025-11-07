import { ChangeDetectionStrategy, Component, computed, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CustomersService } from '../../../customers/service/customers.service';
import { CustomerWithTenants, TenantCustomerProfile } from '../../../customers/models/customer.model';
import { GenericTableComponent } from '../../../../core/shared/components/generic-table/generic-table.component';
import { ColumnDefinition, TableConfig, RowAction } from '../../../../core/shared/components/generic-table/generic-table.model';
import { BreadcrumbComponent } from "@core/shared/components/breadcrumb/breadcrumb.component";
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";
import { MatDialog } from '@angular/material/dialog';
import { AddCustomerDialogComponent } from '../add-customer-dialog/add-customer-dialog.component';
import { AuthService } from '@core/index';
import { PlaceOrderDialogComponent } from '../place-order-dialog/place-order-dialog.component';
import { ViewOrdersDialogComponent } from '../view-orders-dialog/view-orders-dialog.component';
import { first } from 'rxjs';

@Component({
    selector: 'app-customers-list',
    standalone: true,
    imports: [CommonModule, GenericTableComponent, BreadcrumbComponent, MatProgressSpinnerModule],
    templateUrl: './customers-list.component.html',
    styleUrls: ['./customers-list.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomersListComponent {
    private service = inject(CustomersService);
    private authService = inject(AuthService);
    private _items = signal<CustomerWithTenants[]>([]);
    private _total = signal<number>(0);

    readonly loading = signal<boolean>(false);


    customers = computed(() => this._items());
    totalRecords = computed(() => this._total());

    pageSize = 20;
    pageNumber = 1;

    columns = computed<ColumnDefinition[]>(() => [
        { def: 'phoneNumber', label: 'Phone', type: 'phone', sortable: false },
        { def: 'firstName', label: 'First Name', type: 'text', sortable: false },
        { def: 'lastName', label: 'Last Name', type: 'text', sortable: false },
        //{ def: 'tenantNames', label: 'Tenants', type: 'text', sortable: false },
        { def: 'actions', label: 'Actions', type: 'actionBtn' }
    ]);

    tableConfig: TableConfig = {
        enableSelection: false,
        enableSearch: true,
        enableExport: false,
        enableRefresh: true,
        enableColumnHide: true,
        enableAdd: true,
        enableEdit: false,
        enableDelete: false,
        enableContextMenu: false,
        pageSize: this.pageSize,
        pageSizeOptions: [10, 20, 50],
        title: 'Customers',
    };

    rowActions: RowAction[] = [
        { name: 'addChild', icon: 'user-plus', tooltip: 'Add Child Customer', color: 'primary' },
        { name: 'viewOrders', icon: 'list', tooltip: 'View Orders', color: 'accent' },
        { name: 'placeOrder', icon: 'shopping-cart', tooltip: 'Place Order', color: 'primary' },
    ];

    private dialog = inject(MatDialog);

    constructor() {
        this.load();
    }

    private load() {
        this.loading.set(true);
        if (this.authService.isSuperAdmin) {
            this.service.getCustomersWithTenants(this.pageNumber, this.pageSize).subscribe({
                next: (res) => {
                    // The sample shows outer ApiResponse with inner paginated object in res.data
                    // PaginatedApiResponse<T> shape: data: T, totalRecords, pageNumber, pageSize
                    const inner: any = (res as any).data; // fallback due to mixed sample shape
                    const records: CustomerWithTenants[] | undefined = inner?.data || (res as any).data?.data;
                    if (Array.isArray(records)) {
                        this._items.set(records.map((c: CustomerWithTenants) => ({
                            ...c,
                            // Flatten tenant names for display
                            tenantNames: c.tenants.map(t => t.tenantName).join(', ')
                        }) as any));
                        this._total.set(inner?.totalRecords || (res as any).totalRecords || records.length);
                        this.loading.set(false);
                    }
                },
                error: () => this.loading.set(false)
            });
        } else {
            this.service.getCustomersByTenant(this.pageNumber, this.pageSize).subscribe({
                next: (res) => {
                    const records: TenantCustomerProfile[] | undefined = (res as any).data;
                    if (Array.isArray(records)) {
                        this._items.set(records.map((c: TenantCustomerProfile) => ({
                            ...c,
                            tenantNames: c.tenantId ? c.tenantId.toString() : null,
                            // Flatten tenant names for display
                            firstName: c.firstName || '',
                            lastName: c.lastName || '',
                            phoneNumber: c.phoneNumber || '',
                            globalCustomerId: c.globalCustomerId
                        }) as any));
                        this._total.set((res as any).data?.totalRecords || records.length);
                        this.loading.set(false);
                    }
                },
                error: () => this.loading.set(false)
            });
        }
    }

    onTableEvent(e: any) {
        switch (e.type) {
            case 'custom':
                switch (e.action) {
                    case 'addChild':
                        this.openAddDialog(e.data?.globalCustomerId);
                        break;
                    case 'viewOrders':
                        this.openViewOrdersDialog(e.data?.globalCustomerId);
                        break;
                    case 'placeOrder':
                        this.openPlaceOrderDialog(e.data?.globalCustomerId);
                        break;
                }
                break;
            case 'refresh':
                this.load();
                break;
            case 'page':
                this.pageNumber = e.page?.pageIndex + 1 || 1;
                this.pageSize = e.page?.pageSize || this.pageSize;
                this.load();
                break;
            case 'add':
                this.openAddDialog();
                break;
        }
    }

    private openAddDialog(parentGlobalCustomerId?: string) {
        const ref = this.dialog.open(AddCustomerDialogComponent, {
            width: '450px',
            disableClose: true,
            data: { parentGlobalCustomerId: parentGlobalCustomerId || null }
        });
        ref.afterClosed().subscribe(success => {
            if (success) {
                this.load();
            }
        });
    }

    private openPlaceOrderDialog(globalCustomerId: string) {
        if (!globalCustomerId) return;
        const ref = this.dialog.open(PlaceOrderDialogComponent, {
            width: '400px',
            disableClose: true,
            data: { globalCustomerId }
        });
        ref.afterClosed().subscribe(success => {
            if (success) {
                // optional: refresh orders count/summary if added later
            }
        });
    }

    private openViewOrdersDialog(globalCustomerId: string) {
        if (!globalCustomerId) return;
        this.dialog.open(ViewOrdersDialogComponent, {
            width: '500px',
            data: { globalCustomerId }
        });
    }
}
