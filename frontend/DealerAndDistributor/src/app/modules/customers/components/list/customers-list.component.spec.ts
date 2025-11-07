import { TestBed } from '@angular/core/testing';
import { CustomersListComponent } from './customers-list.component';

describe('CustomersListComponent', () => {
  it('should create', () => {
    const fixture = TestBed.configureTestingModule({
      imports: [CustomersListComponent]
    }).createComponent(CustomersListComponent);
    const comp = fixture.componentInstance;
    fixture.detectChanges();
    expect(comp).toBeTruthy();
  });
});
