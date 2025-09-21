import { TestBed } from '@angular/core/testing';
import { UsersListComponent } from './users-list.component';

describe('UsersListComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UsersListComponent]
    }).compileComponents();
  });

  it('should create', () => {
    const fixture = TestBed.createComponent(UsersListComponent);
    const comp = fixture.componentInstance;
    expect(comp).toBeTruthy();
  });
});
