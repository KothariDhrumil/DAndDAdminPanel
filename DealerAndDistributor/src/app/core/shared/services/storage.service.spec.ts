import { LocalStorageService } from './storage.service';

describe('LocalStorageService', () => {
  let service: LocalStorageService;

  beforeEach(() => {
    service = new LocalStorageService();
    // Clear storages before each test to avoid cross-test pollution
    window.localStorage.clear();
    window.sessionStorage.clear();
  });

  it('stores remember flag in localStorage', () => {
    const setSpy = spyOn(window.localStorage, 'setItem').and.callThrough();
    service.setRememberMe(true);
    expect(setSpy).toHaveBeenCalled();
    expect(service.getRememberMe()).toBe(true);
  });

  it('setAuthItem uses sessionStorage when not remembered', () => {
    service.setRememberMe(false);
    const sessionSet = spyOn(window.sessionStorage, 'setItem').and.callThrough();
    const localRemove = spyOn(window.localStorage, 'removeItem').and.callThrough();
    service.setAuthItem('token', 'abc');
    expect(sessionSet).toHaveBeenCalledWith('token', JSON.stringify('abc'));
    expect(localRemove).toHaveBeenCalledWith('token');
  });

  it('setAuthItem uses localStorage when remembered', () => {
    service.setRememberMe(true);
    const localSet = spyOn(window.localStorage, 'setItem').and.callThrough();
    service.setAuthItem('token', 'abc');
    expect(localSet).toHaveBeenCalledWith('token', JSON.stringify('abc'));
  });

  it('getAuthItem prefers sessionStorage over localStorage', () => {
    window.sessionStorage.setItem('token', JSON.stringify('sessionVal'));
    window.localStorage.setItem('token', JSON.stringify('localVal'));
    const val = service.getAuthItem<string>('token');
    expect(val).toBe('sessionVal');
  });

  it('removeAuthItem clears both storages', () => {
    const sessionRemove = spyOn(window.sessionStorage, 'removeItem').and.callThrough();
    const localRemove = spyOn(window.localStorage, 'removeItem').and.callThrough();
    service.removeAuthItem('token');
    expect(sessionRemove).toHaveBeenCalledWith('token');
    expect(localRemove).toHaveBeenCalledWith('token');
  });

  it('clear clears both storages', () => {
    const localClear = spyOn(window.localStorage, 'clear').and.callThrough();
    const sessionClear = spyOn(window.sessionStorage, 'clear').and.callThrough();
    service.clear();
    expect(localClear).toHaveBeenCalled();
    expect(sessionClear).toHaveBeenCalled();
  });
});
