import { RoleService } from './../../services/roles.services';
import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Sort } from '@angular/material/sort';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { PermissionEnum } from 'src/app/core/enums/permissions.enum';
import { PaginatedFilter } from 'src/app/core/models/PaginatedFilter';
import { PaginatedResult } from 'src/app/core/models/wrappers/PaginatedResult';
import { CustomAction } from 'src/app/core/shared/components/table/custom-action';
import { TableColumn } from 'src/app/core/shared/components/table/table-column';
import { AuthUserInfo, foundChangeType } from '../../models/authuserinfo.model';
import { UserParams } from '../../models/userParams';
import { AuthUsersService } from '../../services/auth-users.service';
import { AuthUserEditComponent } from '../auth-user-edit/auth-user-edit.component';
import { AuthRole } from '../../models/role.model';
import { SharedModule } from 'src/app/core/shared/shared.module';
import { AuthRolesFormComponent } from '../auth-roles-form/auth-roles-form.component';

@Component({
  selector: 'app-auth-roles',
  standalone: true,
  imports: [SharedModule],
  templateUrl: './auth-roles.component.html',
  styleUrl: './auth-roles.component.scss'
})
export class AuthRolesComponent {


  roles !: PaginatedResult<AuthRole>;
  roleColumns!: TableColumn[];
  searchString!: string;

  constructor(

    public dialog: MatDialog,
    public toastr: ToastrService,
    public router: Router,
    private roleService: RoleService
  ) { }

  ngOnInit(): void {
    this.getRoles();
    this.initColumns();
  }

  getRoles(): void {
    this.roleService.getRoles().subscribe((result) => {
      this.roles = result;
    });
  }

  initColumns(): void {
    this.roleColumns = [
      { name: 'Role Name', dataKey: 'roleName', isSortable: true, isShowable: true },
      { name: 'Description', dataKey: 'description', isSortable: true, isShowable: true },
      { name: 'Permissions', dataKey: 'permissionNames', isSortable: true, isShowable: true },
      { name: 'Role Type', dataKey: 'roleType', isSortable: true, isShowable: true },
      { name: 'Action', dataKey: 'action', isSortable: true, isShowable: true },
    ];
  }

  openForm(role?: AuthRole): void {
    if (role) {

      const dialogRef = this.dialog.open(AuthRolesFormComponent, {
        data: role
      });
      dialogRef.afterClosed().subscribe((result) => {
        if (result) {
          this.getRoles();
        }
      });
    }
    else {
      let role = new AuthRole();
      const dialogRef = this.dialog.open(AuthRolesFormComponent, {
        data: role
      });
      dialogRef.afterClosed().subscribe((result) => {
        if (result) {
          this.getRoles();
        }
      });
    }
  }

  remove($event: string): void {
    this.roleService.deleteRole($event).subscribe(() => {
      this.getRoles();
      this.toastr.info('Role Removed');
    });
  }


  reload(): void {
    this.getRoles();
  }
}
