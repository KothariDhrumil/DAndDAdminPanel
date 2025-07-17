import { Injectable } from '@angular/core';
import { switchMap, tap } from 'rxjs/operators';
import { NgxRolesService, NgxPermissionsService } from 'ngx-permissions';
import { LoggedInUserService, LoggedInUserInfo } from './loggedinuser.service';
import { forkJoin } from 'rxjs';
import { LocalStorageService } from '../shared/services';

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
      user: this.loggedInUserService.getAuthUserInfo(),
      permissions: this.loggedInUserService.getPermissions(),
    }).pipe(
      tap(({ user, permissions }) => {
        this.setRolesAndPermissions(user, permissions);
      })
    );
  }

  private setRolesAndPermissions(user: LoggedInUserInfo, permissions: string[]) {
    this.permissonsService.loadPermissions(permissions);
    this.rolesService.flushRoles();
    const roles: Record<string, string[]> = {};
    user.roleNames.forEach(roleName => {
      roles[roleName] = permissions;
      if (roleName === 'SuperAdmin') {
        this.storageService.set('isSuperAdmin', true);
      }
    });
    this.rolesService.addRolesWithPermissions(roles);
  }
}
