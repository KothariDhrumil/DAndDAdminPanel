import { Routes } from '@angular/router';
import { TicketsListComponent } from './components/list/tickets-list.component';
import { TicketDetailComponent } from './components/detail/ticket-detail.component';

export const TICKETS_ROUTES: Routes = [
  { path: '', component: TicketsListComponent },
  { path: ':id', component: TicketDetailComponent },
];
