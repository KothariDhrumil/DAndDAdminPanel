import { ChangeDetectionStrategy, Component, signal, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ColumnDefinition, TableConfig } from '../../../../../core/shared/components/generic-table/generic-table.model';
import { Tenant } from '../../models/tenant.model';

import { TenantsService } from '../../service/tenants.service';
import { GenericTableComponent } from '../../../../../core/shared/components/generic-table/generic-table.component';
import { BreadcrumbComponent } from '../../../../../core/shared/components/breadcrumb/breadcrumb.component';


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
    ];
  });

  

  tableConfig: TableConfig = {
    enableSelection: false,
    enableSearch: true,
    enableExport: false,
    enableRefresh: true,
    enableColumnHide: true,
    enableAdd: false,
    enableEdit: false,
    enableDelete: false,
    enableContextMenu: false,
    pageSize: 10,
    pageSizeOptions: [5, 10, 25, 50, 100],
    title: 'Tenants'
  };

  constructor() {
    this.tenantsService.getTenants().subscribe(res => {
      if (res.isSuccess && Array.isArray(res.data)) {
        this._tenants.set(res.data);
        this._totalRecords.set(res.totalRecords);
      }
    });
  }

  onTableEvent(event: unknown) {
    // handle table events if needed
  }
}
