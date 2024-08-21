import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthRolesComponent } from './auth-roles.component';

describe('AuthRolesComponent', () => {
  let component: AuthRolesComponent;
  let fixture: ComponentFixture<AuthRolesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AuthRolesComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AuthRolesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
