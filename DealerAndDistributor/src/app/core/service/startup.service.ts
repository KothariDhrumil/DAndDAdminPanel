import { Injectable, PLATFORM_ID, Inject } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { switchMap, tap } from 'rxjs/operators';
import { NgxRolesService, NgxPermissionsService } from 'ngx-permissions';
import { LoggedInUserService } from './loggedinuser.service';
import { forkJoin, lastValueFrom } from 'rxjs';
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
    private storageService: LocalStorageService,
    @Inject(PLATFORM_ID) private platformId: Object
  ) { }

  /**
   * Loads roles and permissions for the logged-in user before app startup.
   */
  async load() {
    try {
      // Skip on server-side rendering
      if (!isPlatformBrowser(this.platformId)) {
        return;
      }

      // If we have no auth token, return early
      const token = this.storageService.getAuthItem<string>('token');
      if (!token) {
        return;
      }
      
      const result = await lastValueFrom(
        forkJoin({
          user: this.loggedInUserService.getUserInfo(),
          permissions: this.loggedInUserService.getPermissions(),
        })
      );
      
      if (!result.user || !result.permissions) {
        throw new Error('Invalid user data or permissions received');
      }
      this.setAuthUserDetails(result.user.authUser);
      this.setRolesAndPermissions(result.user, result.permissions);
    } catch (error) {
      // console.error('Startup service error:', error);
      //throw error instanceof Error ? error : new Error('Failed to load user permissions');
    }
  }
  setAuthUserDetails(user: authUser) {
     // TODO : lets implement later on
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
