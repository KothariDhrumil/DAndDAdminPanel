import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TenantDetailComponent } from './tenant-detail.component';

describe('TenantDetailComponent', () => {
  let component: TenantDetailComponent;
  let fixture: ComponentFixture<TenantDetailComponent>; 

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TenantDetailComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(TenantDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render Plan History tab label', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('Plan History');
  });
});
