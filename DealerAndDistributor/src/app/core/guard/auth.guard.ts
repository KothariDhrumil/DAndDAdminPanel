
import { Injectable, inject } from '@angular/core';
import {
  Router,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
} from '@angular/router';
import { LocalStorageService } from '../shared/services';
import { NgxRolesService } from 'ngx-permissions';
import { LOGIN_ROUTE } from '../helpers/routes/app-routes';
import { AuthService } from '../service/auth.service';


@Injectable({
  providedIn: 'root',
})
export class AuthGuard {
  private router = inject(Router);
  private store = inject(LocalStorageService);
  private rolesService = inject(NgxRolesService);
  private authService = inject(AuthService);

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    // Enforce authentication
    if (!this.authService.isAuthenticated) {
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
