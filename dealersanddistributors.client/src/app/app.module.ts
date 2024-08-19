import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
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

@NgModule({
  declarations: [
    AppComponent,
    AuthLayoutComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    SharedModule,
    HttpClientModule,
    BrowserAnimationsModule,
  ],
  providers: [
    AuthGuard,
    PermissionGuard,
    RoleGuard,
    AuthService,
    LocalStorageService,

    { provide: LocationStrategy, useClass: HashLocationStrategy },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
