import { Routes } from '@angular/router';
import { SuperadminDashboardComponent } from './dashboard/dashboard.component';

export const SUPERADMIN_ROUTES: Routes = [
  {
    path: 'dashboard',
    component: SuperadminDashboardComponent,
  },
  // Add more superadmin child routes here
];
