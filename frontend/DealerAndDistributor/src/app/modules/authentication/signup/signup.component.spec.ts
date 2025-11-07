import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { SignupComponent } from './signup.component';
import { AuthService } from '../../../core/service/auth.service';
import { of } from 'rxjs';

describe('SignupComponent', () => {
  let component: SignupComponent;
  let fixture: ComponentFixture<SignupComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      imports: [SignupComponent],
      providers: [
        { provide: AuthService, useValue: jasmine.createSpyObj<AuthService>('AuthService', ['register']) }
      ]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SignupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call register on shared submit', () => {
    const auth = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
    auth.register.and.returnValue(of({ isSuccess: true } as any));
    component.onSharedSubmit({
      firstName: 'John', lastName: 'Doe', phoneNumber: '+12345678901', password: 'password1', tenantName: 'Acme', designationId: 1
    });
    expect(auth.register).toHaveBeenCalled();
  });
});
