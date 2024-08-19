import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ToastrModule } from 'ngx-toastr';
import { RouterModule } from '@angular/router';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { ServerErrorComponent } from './components/server-error/server-error.component';
import { MaterialModule } from '../material/material.module';
import { TableComponent } from './components/table/table.component';
import { DataPropertyGetterPipe } from '../pipes/data-property-getter.pipe';
import { TranslateModule } from '@ngx-translate/core';
import { AccessDenialComponent } from './components/access-denial/access-denial.component';
import { HasPermissionDirective } from '../directives/has-permission.directive';
import { HasRoleDirective } from '../directives/has-role.directive';

import { DeleteDialogComponent } from './components/delete-dialog/delete-dialog.component';
import { UploaderComponent } from './components/uploader/uploader.component';
import { NgxBootstrapModule } from './ngx-bootstrap/ngx-bootstrap.module';

@NgModule({
  declarations: [
    NotFoundComponent,
    ServerErrorComponent,
    TableComponent,
    DataPropertyGetterPipe,
    AccessDenialComponent,
    HasPermissionDirective,
    HasRoleDirective,
    DeleteDialogComponent,
    UploaderComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    MaterialModule,
    FormsModule,
    TranslateModule,
    NgxBootstrapModule,
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
    }),
  ],
  providers: [

  ],
  exports: [
    ReactiveFormsModule,
    FormsModule,
    TableComponent,
    HasPermissionDirective,
    HasRoleDirective,
    UploaderComponent,
    NgxBootstrapModule,
  ],
})
export class SharedModule {}
