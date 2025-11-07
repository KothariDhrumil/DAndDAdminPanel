import { Component, signal, inject } from '@angular/core';
import { NavigationEnd, NavigationStart, Router, RouterOutlet, Event } from '@angular/router';
import { PageLoaderComponent } from './layout/page-loader/page-loader.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, PageLoaderComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('DealerAndDistributor');
  currentUrl!: string;
  private router = inject(Router);

  constructor() {
    this.router.events.subscribe((routerEvent: Event) => {
      if (routerEvent instanceof NavigationStart) {
        this.currentUrl = routerEvent.url.substring(
          routerEvent.url.lastIndexOf('/') + 1
        );
      }
      if (routerEvent instanceof NavigationEnd) {
        /* empty */
      }
      // Only scroll if window is available (browser context)
      if (typeof window !== 'undefined') {
        window.scrollTo(0, 0);
      }
    });
  }
}
