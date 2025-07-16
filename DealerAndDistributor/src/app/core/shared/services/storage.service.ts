import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class LocalStorageService {
  private isBrowser = typeof window !== 'undefined' && !!window.localStorage;

  get(key: string) {
    if (!this.isBrowser) return {};
    return JSON.parse(localStorage.getItem(key) || '{}') || {};
  }

  set(key: string, value: unknown): boolean {
    if (!this.isBrowser) return false;
    localStorage.setItem(key, JSON.stringify(value));
    return true;
  }

  has(key: string): boolean {
    if (!this.isBrowser) return false;
    return !!localStorage.getItem(key);
  }

  remove(key: string) {
    if (!this.isBrowser) return;
    localStorage.removeItem(key);
  }

  clear() {
    if (!this.isBrowser) return;
    localStorage.clear();
  }
}
