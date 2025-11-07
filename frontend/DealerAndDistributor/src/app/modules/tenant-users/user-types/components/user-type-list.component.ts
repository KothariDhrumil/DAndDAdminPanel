import { ChangeDetectionStrategy, Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { GenericTableComponent } from '../../../../core/shared/components/generic-table/generic-table.component';
import { ColumnDefinition, TableConfig, TableEventArgs } from '../../../../core/shared/components/generic-table/generic-table.model';
import { UserTypeService } from '../service/user-type.service';
import { UserType } from '../models/user-type.model';
import { UserTypeDialogComponent } from './user-type-dialog.component';
import { ConfirmDeleteUserTypeDialogComponent } from './confirm-delete-user-type-dialog.component';

@Component({
  selector: 'app-user-type-list',
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatDialogModule, GenericTableComponent],
  templateUrl: './user-type-list.component.html',
  styleUrls: ['./user-type-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UserTypeListComponent implements OnInit {
  readonly userTypes = signal<UserType[]>([]);
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);

  readonly columns: ColumnDefinition[] = [
    { def: 'userTypeId', label: 'ID', type: 'text', sortable: true },
    { def: 'name', label: 'Name', type: 'text', sortable: true },
    { def: 'description', label: 'Description', type: 'text' },
    { def: 'actions', label: 'Actions', type: 'actionBtn' }
  ];
  readonly tableConfig: TableConfig = {
    enableSelection: false,
    enableSearch: true,
    enableExport: false,
    enableRefresh: true,
    enableColumnHide: true,
    enableAdd: true,
    enableEdit: true,
    enableDelete: true,
    enableContextMenu: false,
    pageSize: 10,
    pageSizeOptions: [5, 10, 25, 50, 100],
    title: 'User Types'
  };

  constructor(private readonly service: UserTypeService, private readonly dialog: MatDialog) {}

  ngOnInit(): void {
    this.loadUserTypes();
  }

  loadUserTypes() {
    this.loading.set(true);
    this.error.set(null);
    this.service.list().subscribe({
      next: res => {
        this.userTypes.set(Array.isArray(res.data) ? res.data : []);
        this.loading.set(false);
      },
      error: _ => {
        this.error.set('Failed to load user types');
        this.loading.set(false);
      }
    });
  }

  onTableEvent(event: TableEventArgs) {
    switch (event.type) {
      case 'refresh':
        this.loadUserTypes();
        break;
      case 'add':
        this.openUserTypeDialog();
        break;
      case 'edit':
        if (event.data?.userTypeId) this.openUserTypeDialog(event.data);
        break;
      case 'delete':
        if (event.data?.userTypeId) this.openDeleteDialog(event.data);
        break;
    }
  }

  openUserTypeDialog(userType?: UserType) {
    const ref = this.dialog.open(UserTypeDialogComponent, {
      width: '420px',
      data: userType ? { userType } : undefined
    });
    ref.afterClosed().subscribe(ok => { if (ok) this.loadUserTypes(); });
  }

  openDeleteDialog(userType: UserType) {
    const ref = this.dialog.open(ConfirmDeleteUserTypeDialogComponent, {
      width: '340px',
      data: { userType }
    });
    ref.afterClosed().subscribe(ok => { if (ok) this.loadUserTypes(); });
  }
}
