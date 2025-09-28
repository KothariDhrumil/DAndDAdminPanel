import { Routes } from '@angular/router';

export const CUSTOMERS_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./components/list/customers-list.component').then(m => m.CustomersListComponent)
  }
];
