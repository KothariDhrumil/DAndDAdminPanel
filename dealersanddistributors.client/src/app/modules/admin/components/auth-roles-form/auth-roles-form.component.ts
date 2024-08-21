import { ToastrService } from 'ngx-toastr';
import { role } from './../../../../core/models/models';
// auth-roles-form.component.ts
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, FormControl } from '@angular/forms';
import { AuthRole, AuthRoleFormModel, IAuthRole } from '../../models/role.model';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { RoleService } from '../../services/roles.services';
import { SharedModule } from 'src/app/core/shared/shared.module';
import { MaterialModule } from 'src/app/core/material/material.module';

@Component({
  selector: 'app-auth-roles-form',
  templateUrl: './auth-roles-form.component.html',
  styleUrls: ['./auth-roles-form.component.scss'],
  standalone: true,
  imports: [SharedModule, MaterialModule]

})
export class AuthRolesFormComponent implements OnInit {
  authRoleForm !: FormGroup<IAuthRole>;
  permissionsList !: string[];


  formTitle: string = 'Add Role';
  constructor(@Inject(MAT_DIALOG_DATA) public data: AuthRole,
    private roleService: RoleService,
    private fb: FormBuilder,
    private toastr : ToastrService) { }

  ngOnInit(): void {
    this.getPermissions();
    this.initializeForm();

  }
  initializeForm() {
    this.authRoleForm = this.fb.group<IAuthRole>({
      roleName: new FormControl(this.data.roleName),
      description: new FormControl(this.data.description),
      permissionNames: new FormControl(this.data.permissionNames)
    });
  }

  getPermissions() {
    this.roleService.getPermissions().subscribe((result) => {
      this.permissionsList = result.data.map(permission => permission.permissionName);
    });
  }

  onSubmit() {
    if (this.authRoleForm.valid) {

      let role: AuthRoleFormModel = {
        roleName: this.authRoleForm.value.roleName  || '',
        description: this.authRoleForm.value.description || '',
        permissionsWithSelect: this.authRoleForm.value.permissionNames?.map((permission: string) => {
          return { permissionName: permission, selected: true };
        }) || []
      };
      this.roleService.updateRole(role).subscribe((result) => {
        if (result) {
          this.toastr.success('User Updated Successfully');
        }
      }, error => {
        this.toastr.error('Failed to update user');
      }
      );
    }
  }
}
