import { ChangeDetectionStrategy, Component, computed, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CustomersService } from '../../../customers/service/customers.service';
import { CustomerWithTenants } from '../../../customers/models/customer.model';
import { GenericTableComponent } from '../../../../core/shared/components/generic-table/generic-table.component';
import { ColumnDefinition, TableConfig } from '../../../../core/shared/components/generic-table/generic-table.model';
import { BreadcrumbComponent } from "@core/shared/components/breadcrumb/breadcrumb.component";
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";
import { MatDialog } from '@angular/material/dialog';
import { AddCustomerDialogComponent } from '../add-customer-dialog/add-customer-dialog.component';

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
        { def: 'tenantNames', label: 'Tenants', type: 'text', sortable: false },
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

    private dialog = inject(MatDialog);

    constructor() {
        this.load();
    }

    private load() {
        this.loading.set(true);
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
    }

    onTableEvent(e: any) {
        switch (e.type) {
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

    private openAddDialog() {
        const ref = this.dialog.open(AddCustomerDialogComponent, {
            width: '450px',
            disableClose: true,
            data: {}
        });
        ref.afterClosed().subscribe(success => {
            if (success) {
                this.load();
            }
        });
    }
}
