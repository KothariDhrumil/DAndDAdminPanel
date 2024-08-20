import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthUserListComponent } from './auth-user-list.component';

describe('AuthUserListComponent', () => {
  let component: AuthUserListComponent;
  let fixture: ComponentFixture<AuthUserListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AuthUserListComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AuthUserListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
