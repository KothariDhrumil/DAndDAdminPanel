
import { Injectable, inject } from '@angular/core';
import {
  Router,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
} from '@angular/router';
import { LocalStorageService } from '../shared/services';
import { NgxRolesService } from 'ngx-permissions';
import { LOGIN_ROUTE } from '../helpers/routes/app-routes';


@Injectable({
  providedIn: 'root',
})
export class AuthGuard {
  private router = inject(Router);
  private store = inject(LocalStorageService);
  private rolesService = inject(NgxRolesService);

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {

    // Use NgxRolesService to check role
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
