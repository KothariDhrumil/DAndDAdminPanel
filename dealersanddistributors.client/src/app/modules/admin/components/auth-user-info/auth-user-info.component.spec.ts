import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthUserInfoComponent } from './user-info.component';

describe('UserInfoComponent', () => {
  let component: AuthUserInfoComponent;
  let fixture: ComponentFixture<AuthUserInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AuthUserInfoComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AuthUserInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
