import { Direction, BidiModule } from '@angular/cdk/bidi';
import { Component, inject, Renderer2, Inject, DOCUMENT } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { InConfiguration, DirectionService } from '../../../core';
import { ConfigService } from '../../../core/config';
import { UnsubscribeOnDestroyAdapter } from '../../../core/shared';
import { LocalStorageService } from '../../../core/shared/services/storage.service';

@Component({
  selector: 'app-auth-layout',
  templateUrl: './auth-layout.component.html',
  styleUrls: [],
  imports: [BidiModule, RouterOutlet]
})
export class AuthLayoutComponent extends UnsubscribeOnDestroyAdapter {
  direction!: Direction;
  public config!: InConfiguration;

  private document = inject(DOCUMENT);
  private directoryService = inject(DirectionService);
  private configService = inject(ConfigService);
  private renderer = inject(Renderer2);
  private localStorage = inject(LocalStorageService);

  constructor() {
    super();
    this.config = this.configService.configData;
    this.subs.sink = this.directoryService.currentData.subscribe(
      (currentData) => {
        if (currentData) {
          this.direction = currentData === 'ltr' ? 'ltr' : 'rtl';
        } else {
          const isRtl = this.localStorage.get('isRtl');
          if (isRtl) {
            this.direction = isRtl === 'true' ? 'rtl' : 'ltr';
          } else if (this.config) {
            if (this.config.layout.rtl === true) {
              this.direction = 'rtl';
              this.localStorage.set('isRtl', 'true');
            } else {
              this.direction = 'ltr';
              this.localStorage.set('isRtl', 'false');
            }
          }
        }
      }
    );

    // set theme on startup
    const theme = this.localStorage.get('theme');
    const variant = this.config?.layout?.variant;

    if (variant && typeof variant === 'string' && variant.trim() !== '') {
      this.renderer.removeClass(this.document.body, variant);
    }

    if (theme && typeof theme === 'string' && theme.trim() !== '') {
      this.renderer.addClass(this.document.body, theme);
    } else if (variant && typeof variant === 'string' && variant.trim() !== '') {
      this.renderer.addClass(this.document.body, variant);
    }
  }
}
