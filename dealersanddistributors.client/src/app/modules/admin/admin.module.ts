import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminRoutingModule } from './admin-routing.module';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { DeleteDialogComponent } from './shared/components/delete-dialog/delete-dialog.component';
import { LogoutDialogComponent } from '../../core/shared/components/logout-dialog/logout-dialog.component';
import { SettingsComponent } from './components/settings/settings.component';
import { TranslateModule } from '@ngx-translate/core';
import { MaterialModule } from '../../core/material/material.module';
import { SharedModule } from '../../core/shared/shared.module';
import { LoggedInUserApiService } from '../../core/service/logged-in-user.services';
import { AuthUserListComponent } from './components/auth-user-list/auth-user-list.component';
import { AuthRole } from './models/role.model';
import { RoleService } from './services/roles.services';
import { TenantsService } from './services/tenants.service';


@NgModule({
  declarations: [
    DashboardComponent,
    DeleteDialogComponent,
    LogoutDialogComponent,
    SettingsComponent,
    AuthUserListComponent
  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    MaterialModule,
    SharedModule,
    TranslateModule
  ],
  providers:[
    LoggedInUserApiService,
    RoleService,
    TenantsService
  ]
})
export class AdminModule { }
