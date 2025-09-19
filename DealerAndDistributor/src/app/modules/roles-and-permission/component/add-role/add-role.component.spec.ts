import { AddRoleComponent } from './add-role.component';
import { TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { PermissionService } from '../../service/permission.service';
import { of } from 'rxjs';

describe('AddRoleComponent', () => {
  let component: AddRoleComponent;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [ReactiveFormsModule],
      providers: [
        {
          provide: PermissionService,
          useValue: { getPermissions: () => of({ data: [] }) }
        }
      ]
    });
    const fixture = TestBed.createComponent(AddRoleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should have a form with required controls', () => {
    expect(component.form.contains('roleName')).toBeTrue();
    expect(component.form.contains('description')).toBeTrue();
    expect(component.form.contains('roleType')).toBeTrue();
    expect(component.form.contains('permissions')).toBeTrue();
  });

  it('should be invalid when required fields are empty', () => {
    component.form.patchValue({ roleName: '', roleType: null });
    expect(component.form.invalid).toBeTrue();
  });
});
