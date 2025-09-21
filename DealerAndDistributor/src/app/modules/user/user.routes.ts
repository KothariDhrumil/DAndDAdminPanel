import { Routes } from '@angular/router';
import { UserDashboardComponent } from './dashboard/dashboard.component';

export const USER_ROUTES: Routes = [
  {
    path: 'dashboard',
    component: UserDashboardComponent,
  },
  {
    path: 'todos',
    loadComponent: () => import('./todos/components/list/todo-list/todo-list').then(m => m.TodoList)
  },
  {
    path: 'roles-and-permissions',
    loadComponent: () => import('../roles-and-permission/component/list/roles-and-permissions.component')
      .then(m => m.RolesAndPermissionsComponent)
  }
  ,
  {
    path: 'roles-and-permissions/add',
    loadComponent: () => import('../roles-and-permission/component/add-role/add-role.component')
      .then(m => m.AddRoleComponent)
  }
  // Add more user child routes here
];
