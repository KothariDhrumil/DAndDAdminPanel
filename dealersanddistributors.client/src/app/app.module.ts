import { HTTP_INTERCEPTORS, HttpClient, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HashLocationStrategy, LocationStrategy } from '@angular/common';
import { ErrorInterceptor } from './core/interceptor/error.interceptor';
import { JwtInterceptor } from './core/interceptor/jwt.interceptor';
import { AuthLayoutComponent } from './layouts/auth-layout/auth-layout.component';
import { MaterialModule } from './core/material/material.module';
import { SharedModule } from './core/shared/shared.module';
import { AuthGuard } from './core/guard/auth.guard';
import { PermissionGuard } from './core/guard/permission.guard';
import { RoleGuard } from './core/guard/role.guard';
import { AuthService } from './core/service/auth.service';
import { LocalStorageService } from './core/service/local-storage.service';
import { AdminLayoutComponent } from './layouts/admin-layout/admin-layout.component';
import { ToolbarComponent } from './layouts/admin-layout/toolbar/toolbar.component';
import { BusyService } from './core/service/busy.service';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { HeaderOneComponent } from './layouts/admin-layout/header-one/header-one.component';
import { SideMenuOneComponent } from './layouts/admin-layout/side-menu-one/side-menu-one.component';
export function rootLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, 'assets/i18n/', '.json');
}
@NgModule({
  declarations: [
    AppComponent,
    AuthLayoutComponent,
    AdminLayoutComponent,
    ToolbarComponent,
    HeaderOneComponent,
    SideMenuOneComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    SharedModule,
    BrowserAnimationsModule,
    MaterialModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: rootLoaderFactory,
        deps: [HttpClient]
      }
    })
  ],
  providers: [
    AuthGuard,
    PermissionGuard,
    RoleGuard,
    AuthService,
    LocalStorageService,
    BusyService,
    { provide: LocationStrategy, useClass: HashLocationStrategy },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
