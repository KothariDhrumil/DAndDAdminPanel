import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { UserTypeService } from '../../user-types/service/user-type.service';
import { UserType } from '../../user-types/models/user-type.model';
import { CommonModule } from '@angular/common';
import { TenantUsersService, CreateUserRequest, UpdateUserRequest } from '../../service/tenant-users.service';
import { AuthUserItem } from '../../models/tenant-user.model';
import { MatDialog } from '@angular/material/dialog';
import { AddUserDialogComponent } from '../../components/add-user-dialog/add-user-dialog.component';
import { UpdateUserRolesDialogComponent } from '../update-user-roles-dialog/update-user-roles-dialog.component';
import { UpdateUserTenantDialogComponent } from '../update-user-tenant-dialog/update-user-tenant-dialog.component';
import { BreadcrumbComponent } from '../../../../core/shared/components/breadcrumb/breadcrumb.component';
import { GenericTableComponent } from '../../../../core/shared/components/generic-table/generic-table.component';
import { ColumnDefinition, TableConfig, TableEventArgs, RowAction } from '../../../../core/shared/components/generic-table/generic-table.model';
import { MatTabGroup, MatTab } from "@angular/material/tabs";

@Component({
  selector: 'app-users-list',
  standalone: true,
  imports: [CommonModule, BreadcrumbComponent, GenericTableComponent, MatTabGroup, MatTab],
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})

export class UsersListComponent {
  private svc = inject(TenantUsersService);
  private dialog = inject(MatDialog);

  // Cache users by userTypeId
  private _usersByType = signal<Record<number, AuthUserItem[]>>({});
  private _users = signal<AuthUserItem[]>([]);
  users = computed(() => this._users());

  private _userTypes = signal<UserType[]>([]);
  userTypes = computed(() => this._userTypes());

  private _selectedUserTypeId = signal<number | null>(null);
  selectedUserTypeId = computed(() => this._selectedUserTypeId());

  columns = computed<ColumnDefinition[]>(() => [
    { def: 'firstName', label: 'First Name', type: 'text', sortable: true },
    { def: 'lastName', label: 'Last Name', type: 'text', sortable: true },
    { def: 'phoneNumber', label: 'Phone Number', type: 'text', sortable: true },
    { def: 'roleNames', label: 'Roles', type: 'text' },
    { def: 'actions', label: 'Actions', type: 'actionBtn' },
  ]);

  rowActions = computed<RowAction[]>(() => [
    { name: 'profile', icon: 'user', tooltip: 'Edit Profile', color: 'primary' },
    { name: 'tenant', icon: 'home', tooltip: 'Change Tenant', color: 'accent' },
  ]);

  tableConfig: TableConfig = {
    enableSelection: false,
    enableSearch: true,
    enableExport: false,
    enableRefresh: true,
    enableColumnHide: true,
    enableAdd: true,
    enableEdit: false,
    enableDelete: false,
    enableContextMenu: false,
    pageSize: 10,
    pageSizeOptions: [5, 10, 25, 50, 100],
    title: 'Users',
  };

  constructor(private userTypeService: UserTypeService) {
    this.userTypeService.list().subscribe(res => {
      if (Array.isArray(res.data)) {
        this._userTypes.set(res.data);
        // Select first user type by default
        if (res.data.length > 0) {
          this._selectedUserTypeId.set(res.data[0].userTypeId);
          this.loadUsers(res.data[0].userTypeId);
        }
      }
    });
  }

  loadUsers(userTypeId?: number | null, forceRefresh = false) {
    const id = userTypeId ?? this._selectedUserTypeId();
    if (id != null) {
      const cached = this._usersByType()[id];
      if (!forceRefresh && cached) {
        this._users.set(cached);
        return;
      }
      this.svc.listUsersByUserType(id).subscribe(res => {
        if (res?.isSuccess && Array.isArray(res.data)) {
          // Cache result
          const updatedCache = { ...this._usersByType() };
          updatedCache[id] = res.data;
          this._usersByType.set(updatedCache);
          this._users.set(res.data);
        } else {
          this._users.set([]);
        }
      });
    } else {
      this._users.set([]);
    }
  }

  onUserTypeTabClick(userTypeId: number) {
    if (userTypeId !== this._selectedUserTypeId()) {
      this._selectedUserTypeId.set(userTypeId);
      this.loadUsers(userTypeId);
    } else {
      // If already selected, do not reload
      this._users.set(this._usersByType()[userTypeId] ?? []);
    }
  }

  onTableEvent(event: TableEventArgs) {
    switch (event.type) {
      case 'refresh':
        this.loadUsers(undefined, true); // force refresh
        break;
      case 'add':
        this.openAddUser();
        break;
      case 'custom':
        if (event.data && event.action) {
          if (event.action === 'profile') this.openUpdateProfile(event.data as AuthUserItem);
          if (event.action === 'tenant') this.openUpdateTenant(event.data as AuthUserItem);
        }
        break;
      case 'row':
      case 'edit':
        if (event.data) {
          this.openUserActions(event.data);
        }
        break;
    }
  }

  private openUserActions(user: AuthUserItem) {
    this.openUpdateRoles(user);
  }

  private openAddUser() {
    const ref = this.dialog.open(AddUserDialogComponent, {
      width: '640px', data: {
        userTypeId: this._selectedUserTypeId()
      }
    });
    ref.afterClosed().subscribe((changed: boolean) => {
      if (changed) this.loadUsers(undefined, true); // force refresh after add
    });
  }

  private openUpdateRoles(user: AuthUserItem) {
    const ref = this.dialog.open(UpdateUserRolesDialogComponent, {
      width: '640px',
      data: {
        userId: user.userId,
        userName: user.userName,
        currentRoles: user.roleNames || []
      }
    });
    ref.afterClosed().subscribe((changed: boolean) => {
      if (changed) this.loadUsers(undefined, true); // force refresh after edit
    });
  }

  private openUpdateProfile(user: AuthUserItem) {
    const ref = this.dialog.open(AddUserDialogComponent, {
      width: '640px', data: {
        userId: user.userId,
        firstName: user.firstName,
        lastName: user.lastName,
        phoneNumber: user.phoneNumber
      }
    });
    ref.afterClosed().subscribe((changed: boolean) => {
      if (changed) this.loadUsers(undefined, true); // force refresh after edit
    });
  }

  openUpdateTenant(user: AuthUserItem) {
    const ref = this.dialog.open(UpdateUserTenantDialogComponent, {
      width: '500px',
      data: {
        userId: user.userId,
        userName: user.userName,
        currentTenantName: user.tenantName
      }
    });
    ref.afterClosed().subscribe((changed: boolean) => {
      if (changed) this.loadUsers(undefined, true); // force refresh after edit
    });
  }

  getSelectedTabIndex(): number {
    const types = this._userTypes();
    const selectedId = this._selectedUserTypeId();
    return types.findIndex(ut => ut.userTypeId === selectedId);
  }
}
