import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthRolesFormComponent } from './auth-roles-form.component';

describe('AuthRolesFormComponent', () => {
  let component: AuthRolesFormComponent;
  let fixture: ComponentFixture<AuthRolesFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AuthRolesFormComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AuthRolesFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
