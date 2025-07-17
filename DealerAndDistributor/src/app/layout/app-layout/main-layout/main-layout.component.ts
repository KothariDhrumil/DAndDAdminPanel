import { Direction, BidiModule } from '@angular/cdk/bidi';
import { AfterViewInit, Component, Inject, Renderer2, DOCUMENT } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { RightSidebarComponent } from '../../right-sidebar/right-sidebar.component';
import { HeaderComponent } from '../../header/header.component';
import { SidebarComponent } from '../../sidebar/sidebar.component';
import { RightSidebarService, InConfiguration, DirectionService } from '../../../core';
import { ConfigService } from '../../../core/config';
import { UnsubscribeOnDestroyAdapter } from '../../../core/shared';
import { LocalStorageService } from '../../../core/shared/services';

@Component({
    selector: 'app-main-layout',
    templateUrl: './main-layout.component.html',
    styleUrls: [],
    imports: [
        HeaderComponent,
        SidebarComponent,
        RightSidebarComponent,
        BidiModule,
        RouterOutlet,
    ],
    providers: [RightSidebarService]
})
export class MainLayoutComponent
  extends UnsubscribeOnDestroyAdapter
  implements AfterViewInit {
  private isBrowser = typeof window !== 'undefined' && typeof document !== 'undefined';
  direction!: Direction;
  public config!: InConfiguration;
  constructor(
    private directoryService: DirectionService,
    private configService: ConfigService,
    @Inject(DOCUMENT) private document: Document,
    private renderer: Renderer2,
    private storageSevice: LocalStorageService
  ) {
    super();
    this.config = this.configService.configData;
    this.subs.sink = this.directoryService.currentData.subscribe(
      (currentData) => {
        if (currentData) {
          this.direction = currentData === 'ltr' ? 'ltr' : 'rtl';
        } else {
          if (this.storageSevice.has('isRtl')) {
            if (this.storageSevice.get('isRtl') === 'true') {
              this.direction = 'rtl';
            } else if (this.storageSevice.get('isRtl') === 'false') {
              this.direction = 'ltr';
            }
          } else {
            if (this.config) {
              if (this.config.layout.rtl === true) {
                this.direction = 'rtl';
                this.storageSevice.set('isRtl', 'true');
              } else {
                this.direction = 'ltr';
                this.storageSevice.set('isRtl', 'false');
              }
            }
          }
        }
      }
    );
  }
  ngAfterViewInit(): void {
    if (this.isBrowser) {
      //------------ set varient start----------------
      if (this.storageSevice.has('theme')) {
        this.renderer.removeClass(this.document.body, this.config.layout.variant);
        this.renderer.addClass(
          this.document.body,
          this.storageSevice.get('theme') as string
        );
      } else {
        this.renderer.addClass(this.document.body, this.config.layout.variant);
        this.storageSevice.set('theme', this.config.layout.variant);
      }

      //------------ set varient end----------------

      //------------ set theme start----------------

      if (this.storageSevice.has('choose_skin')) {
        this.renderer.removeClass(
          this.document.body,
          'theme-' + this.config.layout.theme_color
        );

        this.renderer.addClass(
          this.document.body,
          this.storageSevice.get('choose_skin') as string
        );
        this.storageSevice.set(
          'choose_skin_active',
          (this.storageSevice.get('choose_skin') as string).substring(6)
        );
      } else {
        this.renderer.addClass(
          this.document.body,
          'theme-' + this.config.layout.theme_color
        );

        this.storageSevice.set(
          'choose_skin',
          'theme-' + this.config.layout.theme_color
        );
        this.storageSevice.set(
          'choose_skin_active',
          this.config.layout.theme_color
        );
      }

      //------------ set theme end----------------

      //------------ set RTL start----------------

      if (this.storageSevice.has('isRtl')) {
        if (this.storageSevice.get('isRtl') === 'true') {
          this.setRTLSettings();
        } else if (this.storageSevice.get('isRtl') === 'false') {
          this.setLTRSettings();
        }
      } else {
        if (this.config.layout.rtl == true) {
          this.setRTLSettings();
        } else {
          this.setLTRSettings();
        }
      }
      //------------ set RTL end----------------

      //------------ set sidebar color start----------------

      if (this.storageSevice.has('menuOption')) {
        this.renderer.addClass(
          this.document.body,
          this.storageSevice.get('menuOption') as string
        );
      } else {
        this.renderer.addClass(
          this.document.body,
          'menu_' + this.config.layout.sidebar.backgroundColor
        );
        this.storageSevice.set(
          'menuOption',
          'menu_' + this.config.layout.sidebar.backgroundColor
        );
      }

      //------------ set sidebar color end----------------

      //------------ set logo color start----------------

      if (this.storageSevice.has('choose_logoheader')) {
        this.renderer.addClass(
          this.document.body,
          this.storageSevice.get('choose_logoheader') as string
        );
      } else {
        this.renderer.addClass(
          this.document.body,
          'logo-' + this.config.layout.logo_bg_color
        );
      }

      //------------ set logo color end----------------

      //------------ set sidebar collapse start----------------
      if (this.storageSevice.has('collapsed_menu')) {
        if (this.storageSevice.get('collapsed_menu') === 'true') {
          this.renderer.addClass(this.document.body, 'side-closed');
          this.renderer.addClass(this.document.body, 'submenu-closed');
        }
      } else {
        if (this.config.layout.sidebar.collapsed == true) {
          this.renderer.addClass(this.document.body, 'side-closed');
          this.renderer.addClass(this.document.body, 'submenu-closed');
          this.storageSevice.set('collapsed_menu', 'true');
        } else {
          this.renderer.removeClass(this.document.body, 'side-closed');
          this.renderer.removeClass(this.document.body, 'submenu-closed');
          this.storageSevice.set('collapsed_menu', 'false');
        }
      }

      //------------ set sidebar collapse end----------------
    }
  }

  setRTLSettings() {
    if (this.isBrowser) {
      document.getElementsByTagName('html')[0].setAttribute('dir', 'rtl');
      this.renderer.addClass(this.document.body, 'rtl');
      this.storageSevice.set('isRtl', 'true');
    }
  }
  setLTRSettings() {
    if (this.isBrowser) {
      document.getElementsByTagName('html')[0].removeAttribute('dir');
      this.renderer.removeClass(this.document.body, 'rtl');
      this.storageSevice.set('isRtl', 'false');
    }
  }
}
