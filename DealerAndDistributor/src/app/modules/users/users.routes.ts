import { Routes } from '@angular/router';


export const USERS_ROUTES: Routes = [
  {
    path: '',
    children: [
      {
        path: '',
        loadComponent: () => import('../users/components/list/users-list.component').then(m => m.UsersListComponent)
      }
    ]
  },
  // Add more user child routes here
];
