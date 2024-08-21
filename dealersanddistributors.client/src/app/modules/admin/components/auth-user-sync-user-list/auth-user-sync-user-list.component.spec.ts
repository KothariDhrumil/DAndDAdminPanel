import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthUserSyncUserListComponent } from './auth-user-sync-user-list.component';

describe('AuthUserSyncUserListComponent', () => {
  let component: AuthUserSyncUserListComponent;
  let fixture: ComponentFixture<AuthUserSyncUserListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AuthUserSyncUserListComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AuthUserSyncUserListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
