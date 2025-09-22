import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { AuthService } from './auth.service';
import { LocalStorageService } from '../shared/services';
import { HttpHandler, HttpRequest, HttpResponse } from '@angular/common/http';
import { REFRESH_TOKEN_API } from '../helpers/routes/api-endpoints';
import { ToastrService } from 'ngx-toastr';

class DummyHandler extends HttpHandler {
  handle(req: HttpRequest<unknown>) {
    return new HttpResponse({ status: 200, body: { ok: true } }) as any;
  }
}

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;
  let storage: LocalStorageService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        AuthService,
        LocalStorageService,
        { provide: ToastrService, useValue: { clear: () => {}, info: () => {}, error: () => {} } },
      ],
    });
    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);
    storage = TestBed.inject(LocalStorageService);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('tryRefreshingToken stores new tokens and forwards request', (done) => {
    // arrange tokens
    storage.setRememberMe(true);
    storage.setAuthItem('token', 'oldAccess');
    storage.setAuthItem('refreshToken', 'oldRefresh');

    const req = new HttpRequest('GET', '/api/test');
    const next = new DummyHandler();

    // act
    service.tryRefreshingToken(req, next as any).subscribe({
      next: () => {
        // assert storage updated
        const newAccess = storage.getAuthItem<string>('token');
        const newRefresh = storage.getAuthItem<string>('refreshToken');
        expect(newAccess).toBe('newAccess');
        expect(newRefresh).toBe('newRefresh');
        done();
      },
      error: (e) => done.fail(e),
    });

    // respond to refresh call
    const refreshReq = httpMock.expectOne(REFRESH_TOKEN_API);
    expect(refreshReq.request.method).toBe('POST');
    refreshReq.flush({ token: 'newAccess', refreshToken: 'newRefresh', refreshTokenExpiryTime: new Date() });
  });

  it('tryRefreshingToken logs out on failure', (done) => {
    storage.setAuthItem('token', 'oldAccess');
    storage.setAuthItem('refreshToken', 'oldRefresh');
    const req = new HttpRequest('GET', '/api/test');
    const next = new DummyHandler();

    service.tryRefreshingToken(req, next as any).subscribe({
      next: () => done.fail('Should not succeed'),
      error: () => done(),
    });

    const refreshReq = httpMock.expectOne(REFRESH_TOKEN_API);
    refreshReq.flush({ message: 'invalid' }, { status: 400, statusText: 'Bad Request' });
  });
});
