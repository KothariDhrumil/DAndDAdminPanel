import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthUserEditComponent } from './auth-user-edit.component';

describe('AuthUserEditComponent', () => {
  let component: AuthUserEditComponent;
  let fixture: ComponentFixture<AuthUserEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AuthUserEditComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AuthUserEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
