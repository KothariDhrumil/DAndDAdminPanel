import { Injectable } from '@angular/core';
import { switchMap, tap } from 'rxjs/operators';
import { NgxRolesService, NgxPermissionsService } from 'ngx-permissions';
import { LoggedInUserService } from './loggedinuser.service';
import { forkJoin } from 'rxjs';
import { LocalStorageService } from '../shared/services';
import { authUser } from "../models/interface/authUser";
import { userInfo } from '../models/interface/LoggedInUserInfo';

@Injectable({
  providedIn: 'root',
})
export class StartupService {
  constructor(
    private rolesService: NgxRolesService,
    private permissonsService: NgxPermissionsService,
    private loggedInUserService: LoggedInUserService,
    private storageService: LocalStorageService
  ) { }

  /**
   * Loads roles and permissions for the logged-in user before app startup.
   */
  load() {
    return forkJoin({
      user: this.loggedInUserService.getUserInfo(),
      permissions: this.loggedInUserService.getPermissions(),
    }).pipe(
      tap(({ user, permissions }) => {
        this.setRolesAndPermissions(user, permissions);
      })
    );
  }

  private setRolesAndPermissions(user: userInfo, permissions: string[]) {
    this.permissonsService.loadPermissions(permissions);
    this.rolesService.flushRoles();
   
    user.authUser.roleNames.forEach(roleName => {
      if (roleName === 'SuperAdmin') {
        this.storageService.set('isSuperAdmin', true);
        this.rolesService.addRole('SUPERADMIN', () => true); // always true
      }
      else {
        this.rolesService.addRole(roleName, []);
      }
    });
    // Add features also as roles (so they can be used)
    user.authUser.tenantFeatures?.forEach(f => this.rolesService.addRole(f, () => true));
  }
}
