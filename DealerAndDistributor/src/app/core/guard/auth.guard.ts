
import { Injectable, inject } from '@angular/core';
import {
  Router,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
} from '@angular/router';
import { LocalStorageService } from '../shared/services';
import { NgxRolesService } from 'ngx-permissions';
import { LOGIN_ROUTE } from '../helpers/routes/app-routes';
import { JwtHelperService } from '@auth0/angular-jwt';


@Injectable({
  providedIn: 'root',
})
export class AuthGuard {
  private router = inject(Router);
  private store = inject(LocalStorageService);
  private rolesService = inject(NgxRolesService);
  private jwt = new JwtHelperService();

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    // Enforce authentication without injecting AuthService (avoids circular DI)
    const token = this.store.getAuthItem<string>('token');
    const isAuthed = typeof token === 'string' && !!token.trim() && token.trim().split('.').length === 3 && !this.jwt.isTokenExpired(token);
    if (!isAuthed) {
      this.router.navigateByUrl(LOGIN_ROUTE);
      return false;
    }

    // Use NgxRolesService to check role if specified
    if (route.data['role']) {
      const hasRole = this.rolesService.getRole(route.data['role']);
      if (!hasRole) {
        this.router.navigateByUrl(LOGIN_ROUTE);
        return false;
      }
    }

    return true;
  }
}
