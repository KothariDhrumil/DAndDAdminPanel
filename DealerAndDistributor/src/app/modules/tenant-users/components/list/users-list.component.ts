import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
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

@Component({
  selector: 'app-users-list',
  standalone: true,
  imports: [CommonModule, BreadcrumbComponent, GenericTableComponent],
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UsersListComponent {
  private svc = inject(TenantUsersService);
  private dialog = inject(MatDialog);

  private _users = signal<AuthUserItem[]>([]);
  users = computed(() => this._users());

  columns = computed<ColumnDefinition[]>(() => [
    { def: 'userName', label: 'User Name', type: 'text', sortable: true },
    { def: 'email', label: 'Email', type: 'email', sortable: true },
    // { def: 'tenantName', label: 'Tenant', type: 'text', sortable: true },
    // { def: 'hasTenant', label: 'Has Tenant', type: 'check' },
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

  constructor() {
    this.loadUsers();
  }

  loadUsers() {
    this.svc.listUsers().subscribe(res => {
      if (res?.isSuccess && Array.isArray(res.data)) {
        this._users.set(res.data);
      } else {
        this._users.set([]);
      }
    });
  }

  onTableEvent(event: TableEventArgs) {
    switch (event.type) {
      case 'refresh':
        this.loadUsers();
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
    // For now, we'll show a simple action menu. In a real app, you might have a context menu
    // or multiple action buttons. Here I'll just open the roles dialog as an example.
    this.openUpdateRoles(user);
  }

  private openAddUser() {
    const ref = this.dialog.open(AddUserDialogComponent, { width: '640px' });
    ref.afterClosed().subscribe((changed: boolean) => { if (changed) this.loadUsers(); });
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
    ref.afterClosed().subscribe((changed: boolean) => { if (changed) this.loadUsers(); });
  }

  private openUpdateProfile(user: AuthUserItem) {
    const ref = this.dialog.open(AddUserDialogComponent, { width: '640px', data: { userId: user.userId } });
    ref.afterClosed().subscribe((changed: boolean) => { if (changed) this.loadUsers(); });
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
    ref.afterClosed().subscribe((changed: boolean) => { if (changed) this.loadUsers(); });
  }
}
