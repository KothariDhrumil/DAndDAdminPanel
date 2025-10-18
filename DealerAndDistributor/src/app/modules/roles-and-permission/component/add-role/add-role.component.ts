import { Component, ChangeDetectionStrategy, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatButtonModule } from '@angular/material/button';
import { PermissionService } from '../../service/permission.service';
import { RolesService } from '../../service/roles.service';
import { Permission } from '../../models/permission.model';
import { RoleTypes } from "../../models/enums/roletypes.enum";
import { RoleDetail, PermissionsWithSelect } from '../../models/role-detail.model';
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";
import { BreadcrumbComponent } from "../../../../core/shared/components/breadcrumb/breadcrumb.component";
import { MatListModule } from "@angular/material/list";
import { ActivatedRoute, Router } from '@angular/router';
import { ROLES_PERMISSION_ROUTE } from '@core/helpers/routes/app-routes';

@Component({
  selector: 'app-add-role',
  templateUrl: './add-role.component.html',
  styleUrls: ['./add-role.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatCheckboxModule,
    MatSlideToggleModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    BreadcrumbComponent,
    MatListModule

  ],
  host: {
    'class': 'add-role-page'
  }
})
export class AddRoleComponent implements OnInit {
  isEditMode = false;
  roleIdToEdit: string | null = null;
  isGroupChecked(group: Permission[]): boolean {
    const selected = this.form.value.permissions as string[];
    return group.every(perm => selected.includes(perm.permissionName));
  }

  onGroupCheckboxChange(group: Permission[], checked: boolean): void {
    if (checked) {
      this.selectAllGroup(group);
    } else {
      this.deselectAllGroup(group);
    }
  }
  selectAllGroup(group: Permission[]): void {
    const selected = new Set(this.form.value.permissions as string[]);
    group.forEach(perm => selected.add(perm.permissionName));
    this.form.patchValue({ permissions: Array.from(selected) });
  }

  deselectAllGroup(group: Permission[]): void {
    const selected = new Set(this.form.value.permissions as string[]);
    group.forEach(perm => selected.delete(perm.permissionName));
    this.form.patchValue({ permissions: Array.from(selected) });
  }
  groupedPermissionsArray = computed(() => {
    const groups = this.groupedPermissions();
    return Object.entries(groups);
  });
  readonly loading = signal<boolean>(false);
  readonly permissions = signal<Permission[]>([]);
  readonly groupedPermissions = computed(() => {
    const groups: { [key: string]: Permission[] } = {};
    this.permissions().forEach(p => {
      if (!groups[p.groupName]) groups[p.groupName] = [];
      groups[p.groupName].push(p);
    });
    return groups;
  });

  readonly roleTypes = Object.keys(RoleTypes).filter(k => isNaN(Number(k)));
  form: FormGroup;

  constructor(
    private fb: FormBuilder,
    private permissionService: PermissionService,
    private rolesService: RolesService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.form = this.fb.group({
      roleName: ['', [Validators.required, Validators.maxLength(50)]],
      description: ['', [Validators.maxLength(200)]],
      roleType: [null, Validators.required],
      permissions: [[]]
    });
  }

  ngOnInit(): void {
    this.loading.set(true);
    this.route.paramMap.subscribe(params => {
      this.roleIdToEdit = params.get('roleId');
      this.isEditMode = !!this.roleIdToEdit;
      this.permissionService.getPermissions().subscribe({
        next: (resp) => {
          this.permissions.set(resp.data);
          if (this.isEditMode && this.roleIdToEdit) {
            this.rolesService.getRoleById(Number(this.roleIdToEdit)).subscribe({
              next: (apiResp) => {
                const role = apiResp.data;
                this.form.patchValue({
                  roleName: role.roleName,
                  description: role.description,
                  roleType: (RoleTypes as any)[role.roleType],
                  permissions: role.permissionsWithSelect?.filter(p => p.selected).map(p => p.permissionName) ?? []
                });
              }
            });
          }
          this.loading.set(false);
        },
        error: () => {
          this.loading.set(false);
        }
      });
    });
  }

  onPermissionToggle(permissionName: string, checked: boolean): void {
    const selected = this.form.value.permissions as string[];
    if (checked === true) {
      this.form.patchValue({ permissions: [...selected, permissionName] });
    } else {
      this.form.patchValue({ permissions: selected.filter(p => p !== permissionName) });
    }
  }

  onSubmit(): void {
    if (this.form.invalid) return;
    let roleTypeInt: number;
    if (typeof this.form.value.roleType === 'string') {
      roleTypeInt = (RoleTypes as any)[this.form.value.roleType];
    } else {
      roleTypeInt = this.form.value.roleType;
    }
    const payload: RoleDetail = {
      roleName: this.form.value.roleName,
      description: this.form.value.description,
      roleType: roleTypeInt,
      permissionsWithSelect: this.form.value.permissions.map((perm: string) => ({
        permissionName: perm,
        selected: true,
        groupName: this.permissions().find(p => p.permissionName === perm)?.groupName ?? '',
        description: this.permissions().find(p => p.permissionName === perm)?.description ?? ''
      }))
    };
    if (this.isEditMode && this.roleIdToEdit) {
      this.rolesService.updateRole(Number(this.roleIdToEdit), payload).subscribe({
        next: () => {
          // Navigate back to roles list after update
          this.router.navigateByUrl(ROLES_PERMISSION_ROUTE);
        },
        error: () => {
          // Show error message
        }
      });
    } else {
      this.rolesService.createRole(payload).subscribe({
        next: () => {
          // Navigate back to roles list after create
          this.router.navigateByUrl(ROLES_PERMISSION_ROUTE);
        },
        error: () => {
          // Show error message
        }
      });
    }
  }
}
