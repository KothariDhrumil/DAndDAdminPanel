import { Routes } from '@angular/router';
import { RolesAndPermissionsComponent } from './component/list/roles-and-permissions.component';
import { AddRoleComponent } from './component/add-role/add-role.component';

export const rolesAndPermissionsRoutes: Routes = [
  {
    path: '',
    children: [
      {
        path: '',
        component: RolesAndPermissionsComponent,
        title: 'Roles List'
      },
      {
        path: 'add',
        component: AddRoleComponent,
        title: 'Add Role'
      },
      {
        path: 'edit/:roleId',
        component: AddRoleComponent,
        title: 'Edit Role'
      }
    ]
  }
];
