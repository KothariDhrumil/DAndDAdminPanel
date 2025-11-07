import { UserTypeListComponent } from './user-types/components/user-type-list.component';
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
    {
      path: 'user-types',
      component: UserTypeListComponent
    }
];
