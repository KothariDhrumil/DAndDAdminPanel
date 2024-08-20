import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminRoutingModule } from './admin-routing.module';
import { DashboardComponent } from './dashboard/dashboard.component';
import { DeleteDialogComponent } from './shared/components/delete-dialog/delete-dialog.component';
import { LogoutDialogComponent } from '../../core/shared/components/logout-dialog/logout-dialog.component';
import { SettingsComponent } from './settings/settings.component';
import { TranslateModule } from '@ngx-translate/core';
import { MaterialModule } from '../../core/material/material.module';
import { SharedModule } from '../../core/shared/shared.module';
import { HeaderOneComponent } from '../../layouts/admin-layout/header-one/header-one.component';
import { SideMenuOneComponent } from '../../layouts/admin-layout/side-menu-one/side-menu-one.component';
import { UserInfoComponent } from './user-info/user-info.component';

@NgModule({
  declarations: [
    DashboardComponent,
    DeleteDialogComponent,
    LogoutDialogComponent,
    SettingsComponent,
  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    MaterialModule,
    SharedModule,
    TranslateModule
  ]
})
export class AdminModule { }
