import { Routes } from '@angular/router';
import { AuthGuard } from './core/guard/auth.guard';
import { MainLayoutComponent } from './layout/app-layout/main-layout/main-layout.component';
import { AuthLayoutComponent } from './layout/app-layout/auth-layout/auth-layout.component';
import { Page404Component } from './modules/authentication/page404/page404.component';

export const routes: Routes = [
  {
    path: '',
    component: MainLayoutComponent,
    canActivate: [AuthGuard],
    children: [
       {
        path: 'user',
        loadChildren: () =>
          import('./modules/domain/domain.routes').then(m => m.DOMAIN_ROUTES),
      },
      {
        path: 'tenant-users',
        loadChildren: () =>
          import('./modules/tenant-users/tenant-users.routes').then(m => m.TENANT_USERS_ROUTES),
      },
      {
        path: 'roles-and-permission',
        loadChildren: () =>
          import('./modules/roles-and-permission/roles-and-permission.routes').then(m => m.rolesAndPermissionsRoutes),
      },
      {
        path: 'superadmin',
        loadChildren: () =>
          import('./modules/superadmin/superadmin.routes').then(m => m.SUPERADMIN_ROUTES),
      },
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
  { path: '**', component: Page404Component },
];
