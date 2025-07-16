import { BehaviorSubject } from 'rxjs';

export class routes {
  public static layoutDirection: BehaviorSubject<string> =
    new BehaviorSubject<string>('');
  public static Url = '';

  static rtl = this.layoutDirection.subscribe((res: string) => {
    if (res == 'rtl') this.Url = res;
  });
  static adminDashboard: any;



  public static get baseUrl(): string {
    return routes.Url;
  }
  public static get login(): string {
    return this.baseUrl + '/login';
  }

  public static get forgot_password(): string {
    return this.baseUrl + '/forgot-password';
  }
  public static get register(): string {
    return this.baseUrl + '/register';
  }
  public static get lock_screen(): string {
    return this.baseUrl + '/lock-screen';
  }

  /*
  Added By Dhrumil

   */
  public static get userInfo(): string {
    return this.baseUrl + '/admin/user-info';
  }
  public static get authRoles(): string {
    return this.baseUrl + '/admin/auth-roles';
  }
  public static get dashboard(): string {
    return this.baseUrl + '/admin/dashboard';
  }
  public static get listAllAuthUsers(): string {
    return this.baseUrl + '/admin/auth-users';
  }

  public static get syncAuthUserWithChangeList(): string {
    return this.baseUrl + '/admin/sync-auth-user-with-change-list';
  }
}
