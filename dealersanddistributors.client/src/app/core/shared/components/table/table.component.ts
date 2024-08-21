import {
  AfterViewInit,
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';
import { TableColumn } from './table-column';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { CustomAction } from './custom-action';
import { PaginatedFilter } from '../../../models/PaginatedFilter';
import { DeleteDialogComponent } from '../delete-dialog/delete-dialog.component';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.scss'],
})
export class TableComponent implements OnInit, AfterViewInit {
  public tableDataSource!: MatTableDataSource<any>;
  public displayedColumns!: string[];
  @Input() customActionOneData!: CustomAction;
  @Input() customActionTwoData!: CustomAction;
  @Input() customActionData!: CustomAction;
  searchString!: string;
  @Input() totalCount: number = 1;
  @Input() pageSize: number = 1;
  @Output() onPageChanged = new EventEmitter<PaginatedFilter>();

  @ViewChild(MatSort, { static: true }) matSort!: MatSort;

  @Input() title!: string;
  @Input() subtitle!: string;

  @Input() isSortable = false;
  @Input() columns!: TableColumn[];

  @Input() set data(data: any[]) {
    this.setTableDataSource(data);
  }

  @Output() onFilter: EventEmitter<string> = new EventEmitter<string>();
  @Output() onReload: EventEmitter<any> = new EventEmitter<any>();
  @Output() onSort: EventEmitter<Sort> = new EventEmitter<Sort>();
  @Output() onCustomActionOne: EventEmitter<any> = new EventEmitter();

  @Output() onCustomActionTwo: EventEmitter<any> = new EventEmitter();
  @Output() onCustomAction: EventEmitter<any> = new EventEmitter();
  @Output() onCreateForm: EventEmitter<any> = new EventEmitter();
  @Output() onEditForm: EventEmitter<any> = new EventEmitter();
  @Output() onView: EventEmitter<any> = new EventEmitter();
  @Output() onDelete: EventEmitter<string> = new EventEmitter<string>();

  constructor(public dialog: MatDialog) { }
  gold!: EventEmitter<{ data: CustomAction; }>[];
  ngOnInit(): void {
    const columnNames = this.columns.map(
      (tableColumn: TableColumn) => tableColumn.name
    );
    this.displayedColumns = columnNames;
  }

  ngAfterViewInit(): void {
    this.tableDataSource.sort = this.matSort;
  }

  setTableDataSource(data: any[]) {
    this.tableDataSource = new MatTableDataSource<any>(data);
  }
  openCustomActionOne($event: any) {
    this.onCustomActionOne.emit($event);
  }
  openCustomActionTwo($event: any) {
    this.onCustomActionOne.emit($event);
  }

  handleCustomAction() {
    this.onCustomAction.emit(this.tableDataSource.data);
  }

  openCreateForm() {
    this.onCreateForm.emit();
  }

  openEditForm($event?: any) {
    this.onEditForm.emit($event);
  }
  openViewForm($event?: any) {
    this.onView.emit($event);
  }

  handleReload() {
    this.searchString = '';
    this.onReload.emit();
  }

  handleFilter() {
    this.onFilter.emit(this.searchString);
  }

  handleSort(sortParams: Sort) {
    const activeColumn = this.columns.find(
      (column) => column.name === sortParams.active
    );
    if (activeColumn) {
      sortParams.active = activeColumn.dataKey;
    }
    if (sortParams.direction == "") {
      sortParams.direction = "asc";
    }
    this.onSort.emit(sortParams);
  }

  openDeleteConfirmationDialog($event: string) {
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      data: 'Do you confirm the removal of this brand?',
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.onDelete.emit($event);
      }
    });
  }
  onPageChange(pageEvent: PageEvent) {
    const event: PaginatedFilter = {
      pageNumber: pageEvent.pageIndex + 1 ?? 1,
      pageSize: pageEvent.pageSize ?? 10,
    };
    this.onPageChanged.emit(event);
  }
}
