import {
  Component,
  ElementRef,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { MyProjectsService } from './my-projects.service';
import { HttpClient } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MyProjects } from './my-projects.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatMenuTrigger, MatMenuModule } from '@angular/material/menu';
import { Subject } from 'rxjs';
import { SelectionModel } from '@angular/cdk/collections';
import { rowsAnimation } from '@shared';
import { Direction } from '@angular/cdk/bidi';
import { TableExportUtil } from '@shared';
import { formatDate, DatePipe } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatRippleModule } from '@angular/material/core';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { BreadcrumbComponent } from '@shared/components/breadcrumb/breadcrumb.component';
import { MatSelectModule } from '@angular/material/select';
import { FormsModule } from '@angular/forms';
import { MyProjectsFormComponent } from './form/form.component';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { TableShowHideColumnComponent } from '@shared/components/table-show-hide-column/table-show-hide-column.component';

@Component({
  selector: 'app-my-projects',
  templateUrl: './my-projects.component.html',
  styleUrls: ['./my-projects.component.scss'],
  animations: [rowsAnimation],
  imports: [
    BreadcrumbComponent,
    FormsModule,
    MatTooltipModule,
    MatButtonModule,
    MatIconModule,
    MatTableModule,
    MatSortModule,
    MatCheckboxModule,
    MatRippleModule,
    MatProgressBarModule,
    MatSelectModule,
    MatProgressSpinnerModule,
    MatMenuModule,
    MatPaginatorModule,
    DatePipe,
    TableShowHideColumnComponent,
  ],
})
export class MyProjectsComponent implements OnInit, OnDestroy {
  columnDefinitions = [
    { def: 'id', label: 'ID', type: 'number', visible: true },
    { def: 'title', label: 'Project Title', type: 'text', visible: true },
    { def: 'clientName', label: 'Client Name', type: 'text', visible: true },
    { def: 'startDate', label: 'Start Date', type: 'date', visible: true },
    { def: 'endDate', label: 'End Date', type: 'date', visible: true },
    { def: 'deadLine', label: 'Deadline', type: 'date', visible: true },
    { def: 'noOfMembers', label: 'No of Members', type: 'text', visible: true },
    { def: 'priority', label: 'Priority', type: 'text', visible: true },
    { def: 'progress', label: 'Progress', type: 'progress', visible: true },
    { def: 'status', label: 'Status', type: 'text', visible: true },
    { def: 'details', label: 'Details', type: 'text', visible: false },
  ];

  dataSource = new MatTableDataSource<MyProjects>([]);
  selection = new SelectionModel<MyProjects>(true, []);
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
    public myProjectsService: MyProjectsService,
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
  detailsCall(row: MyProjects) {
    let tempDirection: Direction;
    if (localStorage.getItem('isRtl') === 'true') {
      tempDirection = 'rtl';
    } else {
      tempDirection = 'ltr';
    }
    this.dialog.open(MyProjectsFormComponent, {
      data: {
        myProjects: row,
        action: 'details',
      },
      direction: tempDirection,
      height: '85%',
      width: '35%',
    });
  }

  loadData() {
    this.myProjectsService.getAllMyProjects().subscribe({
      next: (data) => {
        this.dataSource.data = data;
        this.isLoading = false;
        this.refreshTable();
        this.dataSource.filterPredicate = (data: MyProjects, filter: string) =>
          Object.values(data).some((value) =>
            value.toString().toLowerCase().includes(filter)
          );
      },
      error: (err) => console.error(err),
    });
  }

  getMembers(noOfMembers: number): number[] {
    return Array.from({ length: Math.min(noOfMembers, 3) }, (_, i) => i);
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

  exportExcel() {
    const exportData = this.dataSource.filteredData.map((x) => ({
      'Project Title': x.title,
      'Client Name': x.clientName,
      'Start Date': formatDate(new Date(x.startDate), 'yyyy-MM-dd', 'en') || '',
      'End Date': formatDate(new Date(x.endDate), 'yyyy-MM-dd', 'en') || '',
      Deadline: formatDate(new Date(x.deadLine), 'yyyy-MM-dd', 'en') || '',
      'No of Members': x.noOfMembers,
      Priority: x.priority,
      Progress: x.progress,
      Status: x.status,
      Details: x.details,
    }));

    TableExportUtil.exportToExcel(exportData, 'projects_export');
  }
}
