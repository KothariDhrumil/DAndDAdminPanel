import { AuthService } from './../../../../core/service/auth.service';
import { TenantsService } from '../../services/tenants.service';
import { RoleService } from './../../services/roles.services';
import { foundChangeType } from './../../models/authuserinfo.model';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { SharedModule } from './../../../../core/shared/shared.module';
import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { AuthUserInfo } from '../../models/authuserinfo.model';
import { MaterialModule } from 'src/app/core/material/material.module';
import { AuthUsersService } from '../../services/auth-users.service';

@Component({
  selector: 'app-auth-user-edit',
  standalone: true,
  imports: [SharedModule, MaterialModule],
  templateUrl: './auth-user-edit.component.html',
  styleUrl: './auth-user-edit.component.scss'
})

export class AuthUserEditComponent {
  userForm !: FormGroup<IAuthUserInfoFormGroup>;
  formTitle !: string;
  roles !: string[];
  tenants !: string[];

  constructor(@Inject(MAT_DIALOG_DATA) public data: AuthUserInfo,
    private toastr: ToastrService, private fb: FormBuilder, private roleService: RoleService, private tenantsService: TenantsService,
    private authUserService: AuthUsersService) {

  }

  ngOnInit(): void {
    this.getTenants();
    this.getRoles();
    this.initializeForm();
  }

  getTenants(): void {
    this.tenantsService.getTenants().subscribe((result) => {
      this.tenants = result.map(tenant => tenant.tenantName);
      console.log(this.tenants);

    });
  }

  getRoles(): void {
    this.roleService.getRoles().subscribe((result) => {
      this.roles = result.data.map(role => role.roleName);
    });
  }
  initializeForm() {
    this.userForm = this.fb.group<IAuthUserInfoFormGroup>({
      userName: new FormControl(this.data.userName, [Validators.required]),
      email: new FormControl(this.data.email, [Validators.required, Validators.email]),
      userId: new FormControl(this.data.userId, [Validators.required]),
      roleNames: new FormControl(this.data.roleNames, [Validators.required]),
      tenantName: new FormControl(this.data.tenantName, [Validators.required]),
      foundChangeType: new FormControl(this.data.foundChangeType, [Validators.required])
    })
    this.formTitle = "Edit User";
  }

  onSubmit() {
    if (this.userForm.valid) {

      const user: AuthUserInfo = {
        userId: this.userForm.value.userId || '',
        email: this.userForm.value.email || '',
        roleNames: this.userForm.value.roleNames || [],
        tenantName: this.userForm.value.tenantName || '',
        userName: this.userForm.value.userName || '',
        foundChangeType: this.userForm.value.foundChangeType || foundChangeType.Update,
        hasTenant: false // Provide a default value for the missing property
      };
      this.authUserService.updateUser(user).subscribe((result) => {
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

interface IAuthUserInfoFormGroup {
  email: FormControl<string | null>;
  userName: FormControl<string | null>;

  userId: FormControl<string | null>;
  roleNames: FormControl<string[] | null>;
  tenantName: FormControl<string | null>;
  foundChangeType: FormControl<foundChangeType | null>;
}
