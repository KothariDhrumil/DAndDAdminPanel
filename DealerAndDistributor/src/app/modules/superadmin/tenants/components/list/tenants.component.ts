import { ChangeDetectionStrategy, Component, signal, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ColumnDefinition, TableConfig } from '../../../../../core/shared/components/generic-table/generic-table.model';
import { ApiRequest } from '../../../../../core/models/interface/ApiRequest';
import { Tenant } from '../../models/tenant.model';

import { TenantsService } from '../../service/tenants.service';
import { GenericTableComponent } from '../../../../../core/shared/components/generic-table/generic-table.component';
import { BreadcrumbComponent } from '../../../../../core/shared/components/breadcrumb/breadcrumb.component';
import { Router } from '@angular/router';
import { SUPERADMIN_TENANT_DETAIL_ROUTE } from '../../../../../core/helpers/routes/app-routes';


@Component({
  selector: 'app-tenants',
  templateUrl: './tenants.component.html',
   // styleUrl: './tenants.component.scss',
  standalone: true,
  imports: [CommonModule, GenericTableComponent, BreadcrumbComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TenantsComponent {
  private tenantsService = inject(TenantsService);
  private router = inject(Router);
  private _tenants = signal<Tenant[]>([]);
  private _totalRecords = signal<number>(0);

  tenants = computed(() => this._tenants());
  totalRecords = computed(() => this._totalRecords());

  columns = computed<ColumnDefinition[]>(() => {
    // Always return a value to avoid NG0950 error
    return [
      { def: 'tenantId', label: 'ID', type: 'text', sortable: true },
      { def: 'tenantFullName', label: 'Full Name', type: 'text', sortable: true },
      { def: 'tenantName', label: 'Name', type: 'text', sortable: true },
      { def: 'dataKey', label: 'Data Key', type: 'text' },
      { def: 'hasOwnDb', label: 'Own DB', type: 'check' },
      { def: 'databaseInfoName', label: 'Database', type: 'text' },
      { def: 'parentId', label: 'Parent ID', type: 'text' },
      { def: 'actions', label: 'Actions', type: 'actionBtn', visible: true },
    ];
  });

  

  tableConfig: TableConfig = {
    enableSelection: true,
    enableSearch: true,
    enableExport: false,
    enableRefresh: true,
    enableColumnHide: true,
    enableAdd: true,
    enableEdit: true,
    enableDelete: true,
    enableContextMenu: false,
    pageSize: 10,
    pageSizeOptions: [5, 10, 25, 50, 100],
    title: 'Tenants',

  };

  constructor() {
    this.loadTenants();
  }

  loadTenants(request?: ApiRequest) {
    this.tenantsService.getTenants(request).subscribe(res => {
      if (res.isSuccess && Array.isArray(res.data)) {
        this._tenants.set(res.data);
        this._totalRecords.set(res.totalRecords);
      }
    });
  }

  onTableEvent(event: any) {
    let request: ApiRequest = {};
    switch (event.type) {
      case 'filter':
        request.filter = event.filter;
        break;
      case 'sort':
        request.sort = event.sort;
        break;
      case 'page':
        request.page = event.page;
        break;
      case 'refresh':
        // No extra params, just reload
        break;
      // You can extend for search, etc.
    }
    if (['filter', 'sort', 'page', 'refresh'].includes(event.type)) {
      this.loadTenants(request);
    }
    // Handle other events
    switch (event.type) {
      case 'selection':
        // event.data contains selected rows
        break;
      case 'row':
        // Navigate to detail on row click
        this.navigateToDetail(event.data);
        break;
      case 'add':
        // Open add tenant dialog or route
        break;
      case 'edit':
        // Navigate to detail for edit action
        this.navigateToDetail(event.data);
        break;
      case 'delete':
        // event.data contains row to delete
        break;
      case 'export':
        // Export tenants data (CSV, Excel, etc.)
        break;
      default:
        break;
    }
  }

  private navigateToDetail(tenant: Tenant | undefined) {
    if (!tenant) return;
    const id = (tenant as any).tenantId ?? (tenant as any).id;
    if (id !== undefined && id !== null) {
      this.router.navigate([`${SUPERADMIN_TENANT_DETAIL_ROUTE}/${id}`]);
    } else {
      this.router.navigate([SUPERADMIN_TENANT_DETAIL_ROUTE]);
    }
  }
}
