import { Routes } from '@angular/router';
import { RolesAndPermissionsComponent } from './component/list/roles-and-permissions.component';
import { AddRoleComponent } from './component/add-role/add-role.component';

export const rolesAndPermissionRoutes: Routes = [
  {
    path: '',
    component: RolesAndPermissionsComponent,
    title: 'Roles List'
  },
  {
    path: 'add',
    component: AddRoleComponent,
    title: 'Add Role'
  }
];
