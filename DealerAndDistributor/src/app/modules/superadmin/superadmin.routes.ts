import { Routes } from '@angular/router';
import { SuperadminDashboardComponent } from './dashboard/dashboard.component';

export const SUPERADMIN_ROUTES: Routes = [
  {
    path: 'dashboard',
    component: SuperadminDashboardComponent,
  },
  {
    path: 'tenants',
    children: [
      {
        path: '',
        loadComponent: () => import('./tenants/components/list/tenants.component').then(m => m.TenantsComponent)
      },
      {
        path: 'detail',
        loadComponent: () => import('./tenants/components/detail/tenant-detail.component').then(m => m.TenantDetailComponent)
      },
      {
        path: 'detail/:id',
        loadComponent: () => import('./tenants/components/detail/tenant-detail.component').then(m => m.TenantDetailComponent)
      }
    ]
  },
  {
    path: 'sharding',
    loadComponent: () => import('./sharding/components/sharding-list/sharding-list.component').then(m => m.ShardingListComponent)
  },
  {
    path: 'roles-and-permissions',
    loadChildren: () => import('../roles-and-permission/roles-and-permission.routes').then(m => m.rolesAndPermissionRoutes)
  },
  {
    path: 'users',
    children: [
      {
        path: '',
        loadComponent: () => import('../users/components/list/users-list.component').then(m => m.UsersListComponent)
      }
    ]
  },
  {
    path: 'plans',
    children: [
      {
        path: '',
        loadComponent: () => import('./plans/components/plans-list/plans-list.component').then(m => m.PlansListComponent)
      },
      {
        path: 'add',
        loadComponent: () => import('./plans/components/plans-upsert/plans-upsert.component').then(m => m.PlansUpsertComponent)
      },
      {
        path: 'edit/:id',
        loadComponent: () => import('./plans/components/plans-upsert/plans-upsert.component').then(m => m.PlansUpsertComponent)
      }
    ]
  },

];
