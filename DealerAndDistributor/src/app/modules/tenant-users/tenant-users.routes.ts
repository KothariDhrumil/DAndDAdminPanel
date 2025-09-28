import { Routes } from '@angular/router';


export const TENANT_USERS_ROUTES: Routes = [
  {
    path: '',
    children: [
      {
        path: '',
        loadComponent: () => import('./components/list/users-list.component').then(m => m.UsersListComponent)
      }
    ]
  },
  // Add more user child routes here
];
