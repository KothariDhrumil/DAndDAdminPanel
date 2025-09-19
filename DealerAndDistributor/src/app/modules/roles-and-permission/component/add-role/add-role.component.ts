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
import { Permission } from '../../add-role/models/permission.model';
import { RoleTypes } from '../../models/role.model';
import {  MatProgressSpinnerModule } from "@angular/material/progress-spinner";
import { BreadcrumbComponent } from "../../../../core/shared/components/breadcrumb/breadcrumb.component";
import { MatListModule } from "@angular/material/list";

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

  readonly roleTypes = Object.values(RoleTypes);
  form: FormGroup;

  constructor(private fb: FormBuilder, private permissionService: PermissionService) {
    this.form = this.fb.group({
      roleName: ['', [Validators.required, Validators.maxLength(50)]],
      description: ['', [Validators.maxLength(200)]],
      roleType: [null, Validators.required],
      permissions: [[]]
    });
  }

  ngOnInit(): void {
    this.loading.set(true);
    this.permissionService.getPermissions().subscribe({
      next: (resp) => {
        this.permissions.set(resp.data);
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
      }
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
    // TODO: Call role creation service here
    // ...existing code...
  }
}
