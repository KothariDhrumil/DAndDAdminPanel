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
    loadChildren: () => import('../roles-and-permission/roles-and-permission.routes').then(m => m.rolesAndPermissionRoutes)
  },

];
