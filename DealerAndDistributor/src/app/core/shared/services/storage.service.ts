import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class LocalStorageService {
  private isBrowser = typeof window !== 'undefined' && !!window.localStorage;
  private rememberKey = '__remember_me__';

  get(key: string) {
    if (!this.isBrowser) return {};
    const val = localStorage.getItem(key);
    if (val == null) return {};
    try {
      return JSON.parse(val) ?? {};
    } catch {
      // If value was stored as a plain string or invalid JSON, return an empty object to be safe
      return {};
    }
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
    sessionStorage.clear();
  }

  // Remember me helpers
  setRememberMe(remember: boolean) {
    if (!this.isBrowser) return;
    localStorage.setItem(this.rememberKey, JSON.stringify(remember));
  }

  getRememberMe(): boolean {
    if (!this.isBrowser) return false;
    const raw = localStorage.getItem(this.rememberKey);
    return raw ? JSON.parse(raw) === true : false;
  }

  // Token helpers: write to localStorage if remembered, else sessionStorage
  setAuthItem(key: string, value: unknown, remember?: boolean) {
    if (!this.isBrowser) return false;
    const useLocal = remember ?? this.getRememberMe();
    const json = JSON.stringify(value);
    if (useLocal) {
      localStorage.setItem(key, json);
    } else {
      sessionStorage.setItem(key, json);
      // also clear from local in case it existed
      localStorage.removeItem(key);
    }
    return true;
  }

  getAuthItem<T = unknown>(key: string): T | null {
    if (!this.isBrowser) return null;
    const raw = sessionStorage.getItem(key) ?? localStorage.getItem(key);
    if (raw == null) return null;
    try {
      return JSON.parse(raw) as T;
    } catch {
      // If the stored value is a non-JSON string, return it as-is (useful for legacy values)
      return (raw as unknown) as T;
    }
  }

  removeAuthItem(key: string) {
    if (!this.isBrowser) return;
    sessionStorage.removeItem(key);
    localStorage.removeItem(key);
  }
}
