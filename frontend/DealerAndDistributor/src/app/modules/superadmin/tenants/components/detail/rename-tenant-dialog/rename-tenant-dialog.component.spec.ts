import { TestBed } from '@angular/core/testing';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { RenameTenantDialogComponent } from './rename-tenant-dialog.component';

describe('RenameTenantDialogComponent', () => {
  let dialogRefSpy: jasmine.SpyObj<MatDialogRef<RenameTenantDialogComponent>>;

  beforeEach(async () => {
    dialogRefSpy = jasmine.createSpyObj('MatDialogRef', ['close']);

    await TestBed.configureTestingModule({
      imports: [RenameTenantDialogComponent],
      providers: [
        { provide: MatDialogRef, useValue: dialogRefSpy },
        { provide: MAT_DIALOG_DATA, useValue: { tenantName: 'Old Name' } }
      ]
    }).compileComponents();
  });

  it('should create and prefill form with provided tenant name', () => {
    const fixture = TestBed.createComponent(RenameTenantDialogComponent);
    const comp = fixture.componentInstance;
    fixture.detectChanges();
    expect(comp.form.value.tenantName).toBe('Old Name');
  });

  it('should close with new tenant name on save', () => {
    const fixture = TestBed.createComponent(RenameTenantDialogComponent);
    const comp = fixture.componentInstance;
    fixture.detectChanges();
    comp.form.setValue({ tenantName: 'New Name' });
    comp.save();
    expect(dialogRefSpy.close).toHaveBeenCalledWith({ tenantName: 'New Name' });
  });
});
