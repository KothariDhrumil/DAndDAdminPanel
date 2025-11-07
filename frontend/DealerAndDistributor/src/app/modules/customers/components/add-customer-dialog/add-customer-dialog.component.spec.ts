import { TestBed } from '@angular/core/testing';
import { AddCustomerDialogComponent } from './add-customer-dialog.component';

describe('AddCustomerDialogComponent', () => {
  it('should create', () => {
    const fixture = TestBed.configureTestingModule({
      imports: [AddCustomerDialogComponent]
    }).createComponent(AddCustomerDialogComponent);
    const comp = fixture.componentInstance;
    fixture.detectChanges();
    expect(comp).toBeTruthy();
  });
});
