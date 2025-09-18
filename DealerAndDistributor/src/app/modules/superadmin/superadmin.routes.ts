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
];
