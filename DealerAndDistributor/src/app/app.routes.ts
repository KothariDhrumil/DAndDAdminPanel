import { Routes } from '@angular/router';
import { AuthGuard } from './core/guard/auth.guard';
import { MainLayoutComponent } from './layout/app-layout/main-layout/main-layout.component';
import { AuthLayoutComponent } from './layout/app-layout/auth-layout/auth-layout.component';

export const routes: Routes = [
  {
    path: '',
    component: MainLayoutComponent,
    canActivate: [AuthGuard],
    children: [
      // Place your protected child routes here
      // Example:
      // { path: 'dashboard', component: DashboardComponent },
    ],
  },
  {
    path: 'authentication',
    component: AuthLayoutComponent,
    loadChildren: () =>
      import('./modules/authentication/auth.routes').then((m) => m.AUTH_ROUTE),
  },
  // Optional: handle unknown routes
  { path: '**', redirectTo: '/authentication/signin' }
];
