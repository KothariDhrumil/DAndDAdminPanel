import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { SettingsComponent } from './components/settings/settings.component';
import { AdminLayoutComponent } from '../../layouts/admin-layout/admin-layout.component';
import { AuthGuard } from '../../core/guard/auth.guard';
import { AuthUserInfoComponent } from './components/auth-user-info/user-info.component';
import { AuthUserListComponent } from './components/auth-user-list/auth-user-list.component';
import { AuthUserSyncUserListComponent } from './components/auth-user-sync-user-list/auth-user-sync-user-list.component';
import { AuthRolesComponent } from './components/auth-roles/auth-roles.component';

const routes: Routes = [
  {
    path: '',
    component: AdminLayoutComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full',
      },
      {
        path: 'dashboard',
        component: DashboardComponent
      },
      {
        path: 'user-info',
        component: AuthUserInfoComponent
      },
      {
        path: 'auth-users',
        component: AuthUserListComponent
      },
      {
        path: 'sync-auth-user-with-change-list',
        component: AuthUserSyncUserListComponent
      },
      {
        path: 'auth-roles',
        component: AuthRolesComponent
      }
    ]
  },
  {
    path: 'settings',
    component: SettingsComponent
  },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule {
}
