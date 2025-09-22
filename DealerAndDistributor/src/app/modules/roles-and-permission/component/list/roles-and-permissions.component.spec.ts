import { RolesAndPermissionsComponent } from './roles-and-permissions.component';
import { RolesService } from '../../service/roles.service';
import { of } from 'rxjs';
import { PaginatedApiResponse } from '../../../../core/models/interface/ApiResponse';
import { Role } from '../../models/role.model';
import { TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';

describe('RolesAndPermissionsComponent', () => {
  let component: RolesAndPermissionsComponent;
  let rolesService: jasmine.SpyObj<RolesService>;

  beforeEach(() => {
    rolesService = jasmine.createSpyObj('RolesService', ['getRoles']);
    rolesService.getRoles.and.returnValue(
      of({
        pageNumber: 1,
        pageSize: 10,
        totalRecords: 1,
        data: [],
        isSuccess: true,
        isFailure: false,
        error: { code: '', description: '', type: 0 },
      } as PaginatedApiResponse<Role[]>)
    );

    TestBed.configureTestingModule({
      imports: [RolesAndPermissionsComponent],
      providers: [
        { provide: RolesService, useValue: rolesService },
        { provide: Router, useValue: { navigate: () => {} } },
        { provide: ActivatedRoute, useValue: { snapshot: {}, params: of({}) } },
      ],
    });

    const fixture = TestBed.createComponent(RolesAndPermissionsComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch roles on init', () => {
    component.ngOnInit();
    expect(rolesService.getRoles).toHaveBeenCalledWith(1, 10);
  });
});
