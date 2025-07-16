import { Route } from '@angular/router';
import { AddPaymentComponent } from './add-payment/add-payment.component';
import { InvoiceComponent } from './invoice/invoice.component';
import { Page404Component } from '../../authentication/page404/page404.component';
import { ClientpaymentComponent } from './client-payment/client-payment.component';
export const ACCOUNT_ROUTE: Route[] = [
  {
    path: 'client-payment',
    component: ClientpaymentComponent,
  },
  {
    path: 'add-payment',
    component: AddPaymentComponent,
  },
  {
    path: 'invoice',
    component: InvoiceComponent,
  },
  { path: '**', component: Page404Component },
];
