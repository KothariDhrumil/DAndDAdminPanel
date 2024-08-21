import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Sort } from '@angular/material/sort';
import { ToastrService } from 'ngx-toastr';
import { PaginatedFilter } from 'src/app/core/models/PaginatedFilter';
import { PaginatedResult } from 'src/app/core/models/wrappers/PaginatedResult';
import { CustomAction } from 'src/app/core/shared/components/table/custom-action';
import { TableColumn } from 'src/app/core/shared/components/table/table-column';
import { AuthUserInfo, SyncAuthUserWithChange } from '../../models/authuserinfo.model';
import { UserParams } from '../../models/userParams';
import { AuthUsersService } from '../../services/auth-users.service';
import { AuthUserEditComponent } from '../auth-user-edit/auth-user-edit.component';
import { SharedModule } from "../../../../core/shared/shared.module";

@Component({
  selector: 'app-auth-user-sync-user-list',
  standalone: true,
  templateUrl: './auth-user-sync-user-list.component.html',
  styleUrl: './auth-user-sync-user-list.component.scss',
  imports: [SharedModule]
})
export class AuthUserSyncUserListComponent {

  users !: PaginatedResult<SyncAuthUserWithChange>;
  userColumns!: TableColumn[];
  userParams = new UserParams();
  searchString!: string;
  userRoleActionData: CustomAction = new CustomAction('Manage User Roles', 'gear', 'primary');

  constructor(
    public userService: AuthUsersService,
    public dialog: MatDialog,
    public toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.getSyncUsers();
    this.initColumns();
  }

  getSyncUsers(): void {
    this.userService.getSyncUsers(this.userParams).subscribe((result) => {
      this.users = result;
    });
  }

  initColumns(): void {
    this.userColumns = [
      { name: 'Change type', dataKey: 'foundChangeType', isSortable: true, isShowable: true },
      { name: 'userId', dataKey: 'userId', isSortable: true, isShowable: true },
      { name: 'UserName', dataKey: 'userName', isSortable: true, isShowable: true },
      { name: 'OldUserName', dataKey: 'oldUserName', isSortable: true, isShowable: true },
      { name: 'Email', dataKey: 'email', isSortable: true, isShowable: true },
      { name: 'OldEmail', dataKey: 'oldEmail', isSortable: true, isShowable: true },
      { name: 'EmailChanged', dataKey: 'emailChanged', isSortable: true, isShowable: true },
      { name: 'UserNameChanged', dataKey: 'userNameChanged', isSortable: true, isShowable: true },
      { name: 'NumRoles', dataKey: 'numRoles', isSortable: true, isShowable: true },
      { name: 'RoleNames', dataKey: 'roleNames', isSortable: true, isShowable: true },
      { name: 'hasTenant', dataKey: 'hasTenant', isSortable: true, isShowable: true },
      { name: 'tenantName', dataKey: 'tenantName', isSortable: true, isShowable: true },
      { name: 'Action', dataKey: 'action', position: 'right' },
    ];
  }

  pageChanged(event: PaginatedFilter): void {
    this.userParams.pageNumber = event.pageNumber;
    this.userParams.pageSize = event.pageSize;
    this.getSyncUsers();
  }

  openForm(user?: AuthUserInfo): void {
    const dialogRef = this.dialog.open(AuthUserEditComponent, {
      data: user,
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.getSyncUsers();
      }
    });
  }

  remove($event: string): void {
    this.userService.deleteUser($event).subscribe(() => {
      this.getSyncUsers();
      this.toastr.info('User Removed');
    });
  }

  sort($event: Sort): void {
    this.userParams.orderBy = $event.active + ' ' + $event.direction;
    console.log(this.userParams.orderBy);
    this.getSyncUsers();
  }

  filter($event: string): void {
    this.userParams.searchString = $event.trim().toLocaleLowerCase();
    this.userParams.pageNumber = 0;
    this.userParams.pageSize = 0;
    this.getSyncUsers();
  }

  reload(): void {
    this.userParams.searchString = '';
    this.userParams.pageNumber = 0;
    this.userParams.pageSize = 0;
    this.getSyncUsers();
  }

  openUserRolesForm(user: AuthUserInfo): void {
    const dialogRef = this.dialog.open(AuthUserEditComponent, {
      data: user,
      panelClass: 'mat-dialog-container-no-padding'
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.getSyncUsers();
      }
    });
  }
  syncUsers(): void {
    this.userService.applySyncUsers(this.users.data).subscribe(() => {
      this.toastr.info('Users Synced');
      this.getSyncUsers();
    });
  }
}
