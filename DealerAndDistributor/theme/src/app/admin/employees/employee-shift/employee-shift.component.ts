import { Direction } from '@angular/cdk/bidi';
import { SelectionModel } from '@angular/cdk/collections';
import { NgClass, DatePipe, formatDate } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import {
  Component,
  ElementRef,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatRippleModule } from '@angular/material/core';
import { MatDialog } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule, MatMenuTrigger } from '@angular/material/menu';
import { MatPaginatorModule, MatPaginator } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import {
  MatSnackBar,
  MatSnackBarVerticalPosition,
  MatSnackBarHorizontalPosition,
} from '@angular/material/snack-bar';
import { MatSortModule, MatSort } from '@angular/material/sort';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';
import { rowsAnimation, TableExportUtil } from '@shared';
import { BreadcrumbComponent } from '@shared/components/breadcrumb/breadcrumb.component';
import { FeatherIconsComponent } from '@shared/components/feather-icons/feather-icons.component';
import { Subject } from 'rxjs';
import { EmployeeShiftDeleteComponent } from './dialog/delete/delete.component';
import { EmployeeShiftFormComponent } from './dialog/form-dialog/form-dialog.component';
import { EmployeeShift } from './employee-shift.model';
import { EmployeeShiftService } from './employee-shift.service';
import { TableShowHideColumnComponent } from '@shared/components/table-show-hide-column/table-show-hide-column.component';

@Component({
  selector: 'app-employee-shift',
  animations: [rowsAnimation],
  imports: [
    BreadcrumbComponent,
    FormsModule,
    MatTooltipModule,
    MatButtonModule,
    MatIconModule,
    MatTableModule,
    MatSortModule,
    NgClass,
    MatCheckboxModule,
    FeatherIconsComponent,
    MatRippleModule,
    MatSelectModule,
    MatProgressSpinnerModule,
    MatMenuModule,
    MatPaginatorModule,
    DatePipe,
    TableShowHideColumnComponent,
  ],
  templateUrl: './employee-shift.component.html',
  styleUrl: './employee-shift.component.scss',
})
export class EmployeeShiftComponent implements OnInit, OnDestroy {
  columnDefinitions = [
    { def: 'select', label: 'Checkbox', type: 'check', visible: true },
    { def: 'shiftId', label: 'Shift ID', type: 'number', visible: false },
    { def: 'employeeId', label: 'Employee ID', type: 'number', visible: false },
    {
      def: 'employeeName',
      label: 'Employee Name',
      type: 'text',
      visible: true,
    },
    { def: 'shiftStartTime', label: 'Start Time', type: 'text', visible: true },
    { def: 'shiftEndTime', label: 'End Time', type: 'text', visible: true },
    { def: 'shiftType', label: 'Shift Type', type: 'text', visible: true },
    { def: 'shiftDate', label: 'Shift Date', type: 'date', visible: true },
    {
      def: 'breakStartTime',
      label: 'Break Start',
      type: 'text',
      visible: true,
    },
    { def: 'breakEndTime', label: 'Break End', type: 'text', visible: true },
    {
      def: 'totalShiftHours',
      label: 'Total Hours',
      type: 'number',
      visible: true,
    },
    { def: 'shiftStatus', label: 'Status', type: 'text', visible: true },
    {
      def: 'shiftDescription',
      label: 'Description',
      type: 'text',
      visible: false,
    },
    {
      def: 'shiftAssignedBy',
      label: 'Assigned By',
      type: 'text',
      visible: true,
    },
    {
      def: 'overtimeHours',
      label: 'Overtime Hours',
      type: 'number',
      visible: true,
    },
    { def: 'shiftCategory', label: 'Category', type: 'text', visible: true },
    { def: 'createdDate', label: 'Created Date', type: 'date', visible: false },
    {
      def: 'lastModifiedDate',
      label: 'Last Modified',
      type: 'date',
      visible: false,
    },
    { def: 'actions', label: 'Actions', type: 'actionBtn', visible: true },
  ];

  dataSource = new MatTableDataSource<EmployeeShift>([]);
  selection = new SelectionModel<EmployeeShift>(true, []);
  contextMenuPosition = { x: '0px', y: '0px' };
  isLoading = true;
  private destroy$ = new Subject<void>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild('filter') filter!: ElementRef;
  @ViewChild(MatMenuTrigger) contextMenu?: MatMenuTrigger;

  constructor(
    public httpClient: HttpClient,
    public dialog: MatDialog,
    public employeeShiftService: EmployeeShiftService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    this.loadData();
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  refresh() {
    this.loadData();
  }

  getDisplayedColumns(): string[] {
    return this.columnDefinitions
      .filter((cd) => cd.visible)
      .map((cd) => cd.def);
  }

  loadData() {
    this.employeeShiftService.getEmployeeShifts().subscribe({
      next: (data) => {
        this.dataSource.data = data;
        this.isLoading = false;
        this.refreshTable();
        this.dataSource.filterPredicate = (
          data: EmployeeShift,
          filter: string
        ) =>
          Object.values(data).some((value) =>
            value.toString().toLowerCase().includes(filter)
          );
      },
      error: (err) => console.error(err),
    });
  }

  private refreshTable() {
    this.paginator.pageIndex = 0;
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value
      .trim()
      .toLowerCase();
    this.dataSource.filter = filterValue;
  }

  addNew() {
    this.openDialog('add');
  }

  editCall(row: EmployeeShift) {
    this.openDialog('edit', row);
  }

  openDialog(action: 'add' | 'edit', data?: EmployeeShift) {
    let varDirection: Direction;
    if (localStorage.getItem('isRtl') === 'true') {
      varDirection = 'rtl';
    } else {
      varDirection = 'ltr';
    }
    const dialogRef = this.dialog.open(EmployeeShiftFormComponent, {
      width: '60vw',
      maxWidth: '100vw',
      data: { employeeShift: data, action },
      direction: varDirection,
      autoFocus: false,
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        if (action === 'add') {
          this.dataSource.data = [result, ...this.dataSource.data];
        } else {
          this.updateRecord(result);
        }
        this.refreshTable();
        this.showNotification(
          action === 'add' ? 'snackbar-success' : 'black',
          `${action === 'add' ? 'Add' : 'Edit'} Record Successfully...!!!`,
          'bottom',
          'center'
        );
      }
    });
  }

  private updateRecord(updatedRecord: EmployeeShift) {
    const index = this.dataSource.data.findIndex(
      (record) => record.shiftId === updatedRecord.shiftId
    );
    if (index !== -1) {
      this.dataSource.data[index] = updatedRecord;
      this.dataSource._updateChangeSubscription();
    }
  }

  deleteItem(row: EmployeeShift) {
    const dialogRef = this.dialog.open(EmployeeShiftDeleteComponent, {
      data: row,
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.dataSource.data = this.dataSource.data.filter(
          (record) => record.shiftId !== row.shiftId
        );
        this.refreshTable();
        this.showNotification(
          'snackbar-danger',
          'Delete Record Successfully...!!!',
          'bottom',
          'center'
        );
      }
    });
  }

  showNotification(
    colorName: string,
    text: string,
    placementFrom: MatSnackBarVerticalPosition,
    placementAlign: MatSnackBarHorizontalPosition
  ) {
    this.snackBar.open(text, '', {
      duration: 2000,
      verticalPosition: placementFrom,
      horizontalPosition: placementAlign,
      panelClass: colorName,
    });
  }

  exportExcel() {
    const exportData = this.dataSource.filteredData.map((x) => ({
      'Shift ID': x.shiftId,
      'Employee ID': x.employeeId,
      'Employee Name': x.employeeName,
      'Start Time': x.shiftStartTime,
      'End Time': x.shiftEndTime,
      'Shift Type': x.shiftType,
      'Shift Date': x.shiftDate,
      'Break Start': x.breakStartTime,
      'Break End': x.breakEndTime,
      'Total Hours': x.totalShiftHours,
      Status: x.shiftStatus,
      Description: x.shiftDescription,
      'Assigned By': x.shiftAssignedBy,
      'Overtime Hours': x.overtimeHours,
      Category: x.shiftCategory,
      'Created Date': x.createdDate,
      'Last Modified': x.lastModifiedDate,
    }));

    TableExportUtil.exportToExcel(exportData, 'employeeShift_contracts_export');
  }

  isAllSelected() {
    return this.selection.selected.length === this.dataSource.data.length;
  }

  masterToggle() {
    this.isAllSelected()
      ? this.selection.clear()
      : this.dataSource.data.forEach((row) => this.selection.select(row));
  }

  removeSelectedRows() {
    const totalSelect = this.selection.selected.length;
    this.dataSource.data = this.dataSource.data.filter(
      (item) => !this.selection.selected.includes(item)
    );
    this.selection.clear();
    this.showNotification(
      'snackbar-danger',
      `${totalSelect} Record(s) Deleted Successfully...!!!`,
      'bottom',
      'center'
    );
  }
  onContextMenu(event: MouseEvent, item: EmployeeShift) {
    event.preventDefault();
    this.contextMenuPosition = {
      x: `${event.clientX}px`,
      y: `${event.clientY}px`,
    };
    if (this.contextMenu) {
      this.contextMenu.menuData = { item };
      this.contextMenu.menu?.focusFirstItem('mouse');
      this.contextMenu.openMenu();
    }
  }
}
