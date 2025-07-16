import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { ClientpaymentComponent } from './client-payment.component';
describe('ClientpaymentComponent', () => {
  let component: ClientpaymentComponent;
  let fixture: ComponentFixture<ClientpaymentComponent>;
  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      imports: [ClientpaymentComponent],
    }).compileComponents();
  }));
  beforeEach(() => {
    fixture = TestBed.createComponent(ClientpaymentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });
  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
