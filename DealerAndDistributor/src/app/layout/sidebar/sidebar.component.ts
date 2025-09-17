import {
  Router,
  NavigationEnd,
  RouterLinkActive,
  RouterLink,
} from '@angular/router';
import { NgClass } from '@angular/common';
import {
  Component,
  Inject,
  ElementRef,
  OnInit,
  Renderer2,
  HostListener,
  OnDestroy,
  DOCUMENT
} from '@angular/core';
import { RouteInfo } from './sidebar.metadata';
import { TranslateModule } from '@ngx-translate/core';
import { FeatherModule } from 'angular-feather';
import { NgScrollbar } from 'ngx-scrollbar';

import { SidebarService } from './sidebar.service';
import { NgxPermissionsModule } from 'ngx-permissions';
import { AuthService, Role } from '../../core';
import { UnsubscribeOnDestroyAdapter } from '../../core/shared';
import { CdkNoDataRow } from "@angular/cdk/table";

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss'],
  imports: [
    NgScrollbar,
    RouterLinkActive,
    RouterLink,
    NgClass,
    FeatherModule,
    TranslateModule,
    NgxPermissionsModule,
    CdkNoDataRow
],
  host: {
    '(window:resize)': 'windowResizecall()',
    '(document:mousedown)': 'onGlobalClick($event)'
  }
})
export class SidebarComponent
  extends UnsubscribeOnDestroyAdapter
  implements OnInit {
  private isBrowser = typeof window !== 'undefined';
  public sidebarItems!: RouteInfo[];
  public innerHeight?: number;
  public bodyTag!: HTMLElement;
  listMaxHeight?: string;
  listMaxWidth?: string;
  userFullName?: string;
  userImg?: string;
  userType?: string;
  headerHeight = 60;
  currentRoute?: string;
  constructor(
    @Inject(DOCUMENT) private document: Document,
    private renderer: Renderer2,
    public elementRef: ElementRef,
    private authService: AuthService,
    private router: Router,
    private sidebarService: SidebarService
  ) {
    super();
    this.elementRef.nativeElement.closest('body');
    this.subs.sink = this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        // close sidebar on mobile screen after menu select
        this.renderer.removeClass(this.document.body, 'overlay-open');
      }
    });
  }

  windowResizecall() {
    this.setMenuHeight();
    this.checkStatuForResize(false);
  }
  @HostListener('document:mousedown', ['$event'])
  onGlobalClick(event: Event): void {
    if (!this.elementRef.nativeElement.contains(event.target)) {
      this.renderer.removeClass(this.document.body, 'overlay-open');
    }
  }
  callToggleMenu(event: Event, length: number) {
    if (length > 0) {
      const parentElement = (event.target as HTMLInputElement).closest('li');
      const activeClass = parentElement?.classList.contains('active');

      if (activeClass) {
        this.renderer.removeClass(parentElement, 'active');
      } else {
        this.renderer.addClass(parentElement, 'active');
      }
    }
  }
  ngOnInit() {
    this.subs.sink = this.sidebarService
      .getRouteInfo()
      .subscribe((routes: RouteInfo[]) => {
        this.sidebarItems = routes;
      });

    // if (this.authService.currentUserValue) {
    //   const userRole = this.authService.currentUserValue.roles?.[0]?.name;
    //   this.userFullName = this.authService.currentUserValue.name;
    //   this.userImg =
    //     './assets/images/user/' + this.authService.currentUserValue.avatar;
    //   if (userRole === Role.Admin) {
    //     this.userType = this.capitalizeString(Role.Admin);
    //   } else if (userRole === Role.Client) {
    //     this.userType = this.capitalizeString(Role.Client);
    //   } else if (userRole === Role.Employee) {
    //     this.userType = this.capitalizeString(Role.Employee);
    //   } else {
    //     this.userType = this.capitalizeString(Role.Admin);
    //   }
    // }

    // this.sidebarItems = ROUTES.filter((sidebarItem) => sidebarItem);
    this.initLeftSidebar();
    this.bodyTag = this.document.body;
  }
  initLeftSidebar() {
    // eslint-disable-next-line @typescript-eslint/no-this-alias
    const _this = this;
    // Set menu height
    _this.setMenuHeight();
    _this.checkStatuForResize(true);
  }
  setMenuHeight() {
    if (this.isBrowser) {
      this.innerHeight = window.innerHeight;
      const height = this.innerHeight - this.headerHeight;
      this.listMaxHeight = height + '';
      this.listMaxWidth = '500px';
    }
  }
  isOpen() {
    return this.bodyTag.classList.contains('overlay-open');
  }
  checkStatuForResize(firstTime: boolean) {
    if (this.isBrowser) {
      if (window.innerWidth < 1025) {
        this.renderer.addClass(this.document.body, 'ls-closed');
      } else {
        this.renderer.removeClass(this.document.body, 'ls-closed');
      }
    }
  }
  mouseHover() {
    const body = this.elementRef.nativeElement.closest('body');
    if (body.classList.contains('submenu-closed')) {
      this.renderer.addClass(this.document.body, 'side-closed-hover');
      this.renderer.removeClass(this.document.body, 'submenu-closed');
    }
  }
  mouseOut() {
    const body = this.elementRef.nativeElement.closest('body');
    if (body.classList.contains('side-closed-hover')) {
      this.renderer.removeClass(this.document.body, 'side-closed-hover');
      this.renderer.addClass(this.document.body, 'submenu-closed');
    }
  }
  logout() {
    this.authService.logout();
  }
  capitalizeString(str: string) {
    return str.charAt(0).toUpperCase() + str.slice(1).toLowerCase();
  }
}
