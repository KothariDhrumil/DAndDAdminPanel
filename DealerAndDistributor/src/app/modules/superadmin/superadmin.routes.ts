import { Routes } from '@angular/router';
import { SuperadminDashboardComponent } from './dashboard/dashboard.component';

export const SUPERADMIN_ROUTES: Routes = [
  {
    path: 'dashboard',
    component: SuperadminDashboardComponent,
  },
  {
    path: 'tenants',
    loadComponent: () => import('./tenants/components/list/tenants.component').then(m => m.TenantsComponent)
  },
  {
    path: 'sharding',
    loadComponent: () => import('./sharding/components/sharding-list/sharding-list.component').then(m => m.ShardingListComponent)
  },
  {
    path: 'roles-and-permissions',
    loadComponent: () => import('../../core/shared/components/roles-and-permissions/roles-and-permissions.component')
      .then(m => m.RolesAndPermissionsComponent)
  }

];
