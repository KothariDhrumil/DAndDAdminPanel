import { TestBed } from '@angular/core/testing';
import { UpsertTenantComponent } from './upsert-tenant.component';

describe('UpsertTenantComponent', () => {
  it('should create and emit submit with valid form', () => {
    const fixture = TestBed.configureTestingModule({
      imports: [UpsertTenantComponent],
    }).createComponent(UpsertTenantComponent);
    const comp = fixture.componentInstance;
    comp.showPassword = true;
    comp.designations = [{ id: 1, name: 'CEO' }];
    fixture.detectChanges();

    comp.form.setValue({
      firstName: 'John',
      lastName: 'Doe',
      phoneNumber: '+12345678901',
      password: 'password1',
      tenantName: 'Acme',
      designationId: 1,
      hasOwnDb: false,
      shardingName: '',
    });

    let emitted: any;
    comp.submitted.subscribe(v => emitted = v);
    comp.onSubmit();
    expect(emitted).toBeTruthy();
    expect(emitted.firstName).toBe('John');
  });
});
