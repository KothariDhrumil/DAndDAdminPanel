import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ShardingDialogComponent } from './sharding-dialog.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

describe('ShardingDialogComponent', () => {
  let component: ShardingDialogComponent;
  let fixture: ComponentFixture<ShardingDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, MatDialogModule],
      declarations: [ShardingDialogComponent],
      providers: [
        { provide: MAT_DIALOG_DATA, useValue: {} },
        { provide: MatDialogRef, useValue: { close: jasmine.createSpy('close') } }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(ShardingDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have a valid form when all fields are filled', () => {
    component.form.setValue({
      name: 'Test',
      databaseName: 'DB',
      connectionName: 'Conn',
      databaseType: 'Type'
    });
    expect(component.form.valid).toBe(true);
  });

  it('should close dialog on cancel', () => {
    const dialogRef = TestBed.inject(MatDialogRef) as unknown as { close: jasmine.Spy };
    component.cancel();
    expect(dialogRef.close).toHaveBeenCalled();
  });
});
