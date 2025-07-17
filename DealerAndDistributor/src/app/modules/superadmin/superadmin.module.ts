
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SuperadminDashboardComponent } from './dashboard/dashboard.component';
import { RouterModule } from '@angular/router';
import { SUPERADMIN_ROUTES } from './superadmin.routes';

@NgModule({
  imports: [
    CommonModule,
    SuperadminDashboardComponent,
    RouterModule.forChild(SUPERADMIN_ROUTES),
  ],
})
export class SuperadminModule {}
