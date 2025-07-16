import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { EstimatesFormComponent } from './form-dialog.component';
describe('EstimatesFormComponent', () => {
  let component: EstimatesFormComponent;
  let fixture: ComponentFixture<EstimatesFormComponent>;
  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      imports: [EstimatesFormComponent],
    }).compileComponents();
  }));
  beforeEach(() => {
    fixture = TestBed.createComponent(EstimatesFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });
  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
