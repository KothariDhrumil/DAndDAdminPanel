import { NgClass } from '@angular/common';
import {
  Component,
  Inject,
  ElementRef,
  OnInit,
  AfterViewInit,
  Renderer2,
  ChangeDetectionStrategy,
  DOCUMENT
} from '@angular/core';

import {
  MatSlideToggleChange,
  MatSlideToggleModule,
} from '@angular/material/slide-toggle';


import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { NgScrollbar } from 'ngx-scrollbar';
import { FeatherIconsComponent } from '../../core/shared/components/feather-icons/feather-icons.component';
import { UnsubscribeOnDestroyAdapter } from '../../core/shared';
import { DirectionService, InConfiguration, RightSidebarService } from '../../core';
import { ConfigService } from '../../core/config';
import { LocalStorageService } from '../../core/shared/services';


@Component({
    changeDetection: ChangeDetectionStrategy.OnPush,
    selector: 'app-right-sidebar',
    templateUrl: './right-sidebar.component.html',
    styleUrls: ['./right-sidebar.component.scss'],
    imports: [
        NgClass,
        FeatherIconsComponent,
        NgScrollbar,
        MatButtonToggleModule,
        MatSlideToggleModule,
    ]
})
export class RightSidebarComponent
  extends UnsubscribeOnDestroyAdapter
  implements OnInit, AfterViewInit {
  private isBrowser = typeof window !== 'undefined';
  selectedBgColor = 'white';
  maxHeight?: string;
  maxWidth?: string;
  showpanel = false;
  isOpenSidebar?: boolean;
  isDarkSidebar = false;
  isDarTheme = false;
  public innerHeight?: number;
  headerHeight = 60;
  isRtl = false;
  public config!: InConfiguration;

  constructor(
    @Inject(DOCUMENT) private document: Document,
    private renderer: Renderer2,
    public elementRef: ElementRef,
    private rightSidebarService: RightSidebarService,
    private configService: ConfigService,
    private directionService: DirectionService,
    private storageSevice: LocalStorageService,
  ) {
    super();
  }
  ngOnInit() {
    this.config = this.configService.configData;
    this.subs.sink = this.rightSidebarService.sidebarState.subscribe(
      (isRunning) => {
        this.isOpenSidebar = isRunning;
      }
    );
    this.setRightSidebarWindowHeight();
  }

  ngAfterViewInit() {
    this.selectedBgColor = this.storageSevice.get('choose_skin_active') as string;

    if (this.storageSevice.has('menuOption')) {
      if (this.storageSevice.get('menuOption') === 'menu_dark') {
        this.isDarkSidebar = true;
      } else if (this.storageSevice.get('menuOption') === 'menu_light') {
        this.isDarkSidebar = false;
      }
    }

    if (this.storageSevice.has('theme')) {
      if (this.storageSevice.get('theme') === 'dark') {
        this.isDarTheme = true;
      } else if (this.storageSevice.get('theme') === 'light') {
        this.isDarTheme = false;
      }
    }

    if (this.storageSevice.has('isRtl')) {
      if (this.storageSevice.get('isRtl') === 'true') {
        this.isRtl = true;
      } else if (this.storageSevice.get('isRtl') === 'false') {
        this.isRtl = false;
      }
    }
  }

  selectTheme(e: string) {
    this.selectedBgColor = e;
    const prevTheme = this.elementRef.nativeElement
      .querySelector('.settingSidebar .choose-theme li.active')
      .getAttribute('data-theme');
    this.renderer.removeClass(this.document.body, 'theme-' + prevTheme);
    this.renderer.addClass(this.document.body, 'theme-' + this.selectedBgColor);
    this.storageSevice.set('choose_skin', 'theme-' + this.selectedBgColor);
    this.storageSevice.set('choose_skin_active', this.selectedBgColor);
  }
  lightSidebarBtnClick() {
    this.renderer.removeClass(this.document.body, 'menu_dark');
    this.renderer.removeClass(this.document.body, 'logo-black');
    this.renderer.addClass(this.document.body, 'menu_light');
    this.renderer.addClass(this.document.body, 'logo-white');
    const menuOption = 'menu_light';
    this.storageSevice.set('choose_logoheader', 'logo-white');
    this.storageSevice.set('menuOption', menuOption);
  }
  darkSidebarBtnClick() {
    this.renderer.removeClass(this.document.body, 'menu_light');
    this.renderer.removeClass(this.document.body, 'logo-white');
    this.renderer.addClass(this.document.body, 'menu_dark');
    this.renderer.addClass(this.document.body, 'logo-black');
    const menuOption = 'menu_dark';
    this.storageSevice.set('choose_logoheader', 'logo-black');
    this.storageSevice.set('menuOption', menuOption);
  }
  lightThemeBtnClick() {
    this.renderer.removeClass(this.document.body, 'dark');
    this.renderer.removeClass(this.document.body, 'submenu-closed');
    this.renderer.removeClass(this.document.body, 'menu_dark');
    this.renderer.removeClass(this.document.body, 'logo-black');
    if (this.storageSevice.has('choose_skin')) {
      this.renderer.removeClass(
        this.document.body,
        this.storageSevice.get('choose_skin') as string
      );
    } else {
      this.renderer.removeClass(
        this.document.body,
        'theme-' + this.config.layout.theme_color
      );
    }

    this.renderer.addClass(this.document.body, 'light');
    this.renderer.addClass(this.document.body, 'submenu-closed');
    this.renderer.addClass(this.document.body, 'menu_light');
    this.renderer.addClass(this.document.body, 'logo-white');
    this.renderer.addClass(this.document.body, 'theme-white');
    const theme = 'light';
    const menuOption = 'menu_light';
    this.selectedBgColor = 'white';
    this.isDarkSidebar = false;
    this.storageSevice.set('choose_logoheader', 'logo-white');
    this.storageSevice.set('choose_skin', 'theme-white');
    this.storageSevice.set('theme', theme);
    this.storageSevice.set('menuOption', menuOption);
  }
  darkThemeBtnClick() {
    this.renderer.removeClass(this.document.body, 'light');
    this.renderer.removeClass(this.document.body, 'submenu-closed');
    this.renderer.removeClass(this.document.body, 'menu_light');
    this.renderer.removeClass(this.document.body, 'logo-white');
    if (this.storageSevice.has('choose_skin')) {
      this.renderer.removeClass(
        this.document.body,
        this.storageSevice.get('choose_skin') as string
      );
    } else {
      this.renderer.removeClass(
        this.document.body,
        'theme-' + this.config.layout.theme_color
      );
    }
    this.renderer.addClass(this.document.body, 'dark');
    this.renderer.addClass(this.document.body, 'submenu-closed');
    this.renderer.addClass(this.document.body, 'menu_dark');
    this.renderer.addClass(this.document.body, 'logo-black');
    this.renderer.addClass(this.document.body, 'theme-black');
    const theme = 'dark';
    const menuOption = 'menu_dark';
    this.selectedBgColor = 'black';
    this.isDarkSidebar = true;
    this.storageSevice.set('choose_logoheader', 'logo-black');
    this.storageSevice.set('choose_skin', 'theme-black');
    this.storageSevice.set('theme', theme);
    this.storageSevice.set('menuOption', menuOption);
  }
  setRightSidebarWindowHeight() {
    if (this.isBrowser) {
      this.innerHeight = window.innerHeight;
      const height = this.innerHeight - this.headerHeight;
      this.maxHeight = height + '';
      this.maxWidth = '500px';
    }
  }
  onClickedOutside(event: Event) {
    const button = event.target as HTMLButtonElement;
    if (button.id !== 'settingBtn') {
      if (this.isOpenSidebar === true) {
        this.toggleRightSidebar();
      }
    }
  }
  toggleRightSidebar(): void {
    this.rightSidebarService.setRightSidebar(
      (this.isOpenSidebar = !this.isOpenSidebar)
    );
  }
  switchDirection(event: MatSlideToggleChange) {
    const isrtl = String(event.checked);
    if (
      isrtl === 'false' &&
      document.getElementsByTagName('html')[0].hasAttribute('dir')
    ) {
      document.getElementsByTagName('html')[0].removeAttribute('dir');
      this.renderer.removeClass(this.document.body, 'rtl');
      this.directionService.updateDirection('ltr');
    } else if (
      isrtl === 'true' &&
      !document.getElementsByTagName('html')[0].hasAttribute('dir')
    ) {
      document.getElementsByTagName('html')[0].setAttribute('dir', 'rtl');
      this.renderer.addClass(this.document.body, 'rtl');
      this.directionService.updateDirection('rtl');
    }
    this.storageSevice.set('isRtl', isrtl);
    this.isRtl = event.checked;
  }
  setRTLSettings() {
    document.getElementsByTagName('html')[0].setAttribute('dir', 'rtl');
    this.renderer.addClass(this.document.body, 'rtl');
    this.isRtl = true;
    this.storageSevice.set('isRtl', 'true');
  }
  setLTRSettings() {
    document.getElementsByTagName('html')[0].removeAttribute('dir');
    this.renderer.removeClass(this.document.body, 'rtl');
    this.isRtl = false;
    this.storageSevice.set('isRtl', 'false');
  }
}
