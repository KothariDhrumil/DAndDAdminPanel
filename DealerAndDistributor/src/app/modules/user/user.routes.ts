import { Routes } from '@angular/router';
import { UserDashboardComponent } from './dashboard/dashboard.component';

export const USER_ROUTES: Routes = [
  {
    path: 'dashboard',
    component: UserDashboardComponent,
  },
  // Add more user child routes here
];
