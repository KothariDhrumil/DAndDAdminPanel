import { Component } from '@angular/core';
import { PaginatedResult } from '../../../../core/models/wrappers/PaginatedResult';
import { TableColumn } from '../../../../core/shared/components/table/table-column';
import { MatDialog } from '@angular/material/dialog';
import { Sort } from '@angular/material/sort';
import { ToastrService } from 'ngx-toastr';
import { PaginatedFilter } from '../../../../core/models/PaginatedFilter';
import { CustomAction } from '../../../../core/shared/components/table/custom-action';
import { UserParams } from '../../models/userParams';
import { AuthUsersService } from '../../services/auth-users.service';
import { AuthUserInfo } from '../../models/authuserinfo.model';
import { AuthUserEditComponent } from '../auth-user-edit/auth-user-edit.component';

@Component({
  selector: 'app-auth-user-list',
  templateUrl: './auth-user-list.component.html',
  styleUrl: './auth-user-list.component.scss'
})
export class AuthUserListComponent {

  users !: PaginatedResult<AuthUserInfo>;
  userColumns!: TableColumn[];
  userParams = new UserParams();
  searchString!: string;
  userRoleActionData: CustomAction = new CustomAction('Manage User Roles');

  constructor(
    public userService: AuthUsersService,
    public dialog: MatDialog,
    public toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.getUsers();
    this.initColumns();
  }

  getUsers(): void {
    this.userService.getUsers(this.userParams).subscribe((result) => {
      this.users = result;
    });
  }

  initColumns(): void {
    this.userColumns = [
      { name: 'userId', dataKey: 'userId', isSortable: true, isShowable: true },
      { name: 'UserName', dataKey: 'userName', isSortable: true, isShowable: true },
      //{ name: 'FirstName', dataKey: 'firstName', isSortable: true, isShowable: true },
      //{ name: 'LastName', dataKey: 'lastName', isSortable: true, isShowable: true },
      { name: 'Email', dataKey: 'email', isSortable: true, isShowable: true },
      { name: 'hasTenant', dataKey: 'hasTenant', isSortable: true, isShowable: true },
      { name: 'tenantName', dataKey: 'tenantName', isSortable: true, isShowable: true },
      //{ name: 'PhoneNumber', dataKey: 'phoneNumber', isSortable: true, isShowable: true },
      { name: 'Action', dataKey: 'action', position: 'right' },
    ];
  }

  pageChanged(event: PaginatedFilter): void {
    this.userParams.pageNumber = event.pageNumber;
    this.userParams.pageSize = event.pageSize;
    this.getUsers();
  }

  openForm(user?: AuthUserInfo): void {
    const dialogRef = this.dialog.open(AuthUserEditComponent, {
      data: user,
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.getUsers();
      }
    });
  }

  remove($event: string): void {
    this.userService.deleteUser($event).subscribe(() => {
      this.getUsers();
      this.toastr.info('User Removed');
    });
  }

  sort($event: Sort): void {
    this.userParams.orderBy = $event.active + ' ' + $event.direction;
    console.log(this.userParams.orderBy);
    this.getUsers();
  }

  filter($event: string): void {
    this.userParams.searchString = $event.trim().toLocaleLowerCase();
    this.userParams.pageNumber = 0;
    this.userParams.pageSize = 0;
    this.getUsers();
  }

  reload(): void {
    this.userParams.searchString = '';
    this.userParams.pageNumber = 0;
    this.userParams.pageSize = 0;
    this.getUsers();
  }

  openUserRolesForm(user: AuthUserInfo): void {
    const dialogRef = this.dialog.open(AuthUserEditComponent, {
      data: user,
      panelClass: 'mat-dialog-container-no-padding'
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.getUsers();
      }
    });
  }
}
