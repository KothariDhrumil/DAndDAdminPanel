import { ChangeDetectionStrategy, Component, computed, input, output, signal, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { SelectionModel } from '@angular/cdk/collections';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSort, MatSortHeader, Sort } from '@angular/material/sort';
import { MatTooltip, MatTooltipModule } from '@angular/material/tooltip';
import { MatMenu, MatMenuModule, MatMenuTrigger } from '@angular/material/menu';
import { CommonModule, DatePipe, NgClass, NgOptimizedImage, NgStyle } from '@angular/common';
import { MatFormField, MatInput, MatLabel } from '@angular/material/input';
import { MatCheckbox } from '@angular/material/checkbox';
import { MatButton, MatButtonModule } from '@angular/material/button';
import { MatIcon, MatIconModule } from '@angular/material/icon';
import { ColumnDefinition, TableConfig, TableEventArgs, ContextMenuPosition, SortInfo, PageInfo } from './generic-table.model';
import { MatDivider } from '@angular/material/divider';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-generic-table',
  templateUrl: './generic-table.component.html',
  styleUrl: './generic-table.component.scss',
  standalone: true,
  imports: [
    MatPaginator,
    MatSort,
    MatTooltipModule,
    MatMenuModule,
    NgOptimizedImage,
    MatFormField,
    MatInput,
    MatCheckbox,
    MatButtonModule,
    MatIconModule,
    MatTableModule ,
    MatSort,
    MatSortHeader,
    NgStyle,
    MatMenuTrigger,
    MatLabel,
    DatePipe,
    NgClass,
    
    MatDivider,
    FormsModule    
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class GenericTableComponent implements OnInit, OnChanges {
  // Inputs
  data = input.required<any[]>();
  columns = input.required<ColumnDefinition[]>();
  totalRecords = input.required<number>();
  config = input<TableConfig>({
    enableSelection: false,
    enableSearch: true,
    enableExport: false,
    enableRefresh: true,
    enableColumnHide: true,
    enableAdd: false,
    enableEdit: false,
    enableDelete: false,
    enableContextMenu: false,
    pageSize: 10,
    pageSizeOptions: [5, 10, 25, 50, 100],
    title: ''
  });

  // Outputs
  tableEvent = output<TableEventArgs>();

  // Signals
  public filterValue = signal('');
  private visibleColumns = signal<string[]>([]);
  private contextMenuPosition = signal<ContextMenuPosition>({ x: '0', y: '0' });
  private currentSort = signal<SortInfo>({ active: '', direction: '' });
  private currentPage = signal<PageInfo>({ pageIndex: 0, pageSize: 10 });

  // Computed values
  readonly displayedColumns = computed(() => {
    const columns = this.visibleColumns();
    return this.config().enableSelection ? ['select', ...columns] : columns;
  });

  // Material Table properties
  dataSource!: MatTableDataSource<any>;
  selection = new SelectionModel<any>(true, []);

  constructor() {
    // Do not rely on inputs in constructor
  }

  ngOnInit() {
    // Initialize columns and data when component is initialized
    if (this.columns() && Array.isArray(this.columns())) {
      this.visibleColumns.set(this.columns().map(col => col.def));
    }
    this.initDataSource();
  }

  ngOnChanges(changes: SimpleChanges) {
    // React to input changes
    if (changes['columns'] && this.columns() && Array.isArray(this.columns())) {
      this.visibleColumns.set(this.columns().map(col => col.def));
    }
    if (changes['data'] && this.data()) {
      this.initDataSource();
    }
  }

  private initDataSource() {
    this.dataSource = new MatTableDataSource(this.data());
    this.dataSource.filterPredicate = (data: any, filter: string) => {
      const searchStr = JSON.stringify(data).toLowerCase();
      return searchStr.indexOf(filter.toLowerCase()) !== -1;
    };
  }

  applyFilter(event: Event) {
    const value = (event.target as HTMLInputElement).value.trim().toLowerCase();
    this.filterValue.set(value);
    this.tableEvent.emit({ 
      type: 'filter', 
      filter: value,
      sort: this.currentSort(),
      page: this.currentPage()
    });
  }

  onSort(event: Sort) {
    const sortInfo: SortInfo = {
      active: event.active,
      direction: event.direction
    };
    this.currentSort.set(sortInfo);
    this.tableEvent.emit({
      type: 'sort',
      sort: sortInfo,
      page: this.currentPage(),
      filter: this.filterValue()
    });
  }

  onPageChange(event: PageEvent) {
    const pageInfo: PageInfo = {
      pageIndex: event.pageIndex,
      pageSize: event.pageSize,
    };
    this.currentPage.set(pageInfo);
    this.tableEvent.emit({
      type: 'page',
      page: pageInfo,
      sort: this.currentSort(),
      filter: this.filterValue()
    });
  }

  toggleColumnVisibility(column: ColumnDefinition) {
    const currentColumns = this.visibleColumns();
    const index = currentColumns.indexOf(column.def);
    
    if (index === -1) {
      this.visibleColumns.update(cols => [...cols, column.def]);
    } else {
      this.visibleColumns.update(cols => cols.filter(col => col !== column.def));
    }
  }

  isColumnVisible(column: ColumnDefinition): boolean {
    return this.visibleColumns().includes(column.def);
  }

  masterToggle() {
    if (this.isAllSelected()) {
      this.selection.clear();
    } else {
      this.dataSource.data.forEach(row => this.selection.select(row));
    }
    this.tableEvent.emit({ type: 'selection', data: this.selection.selected });
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  onRowClick(row: any) {
    this.tableEvent.emit({ type: 'row', data: row });
  }

  onContextMenu(event: MouseEvent, row: any) {
    if (!this.config().enableContextMenu) return;
    
    event.preventDefault();
    this.contextMenuPosition.set({
      x: `${event.clientX}px`,
      y: `${event.clientY}px`
    });
    this.tableEvent.emit({ type: 'row', data: row });
  }

  refresh() {
    this.tableEvent.emit({ type: 'refresh' });
  }

  add() {
    this.tableEvent.emit({ type: 'add' });
  }

  edit(row: any) {
    this.tableEvent.emit({ type: 'edit', data: row });
  }

  delete(row: any) {
    this.tableEvent.emit({ type: 'delete', data: row });
  }

  export() {
    this.tableEvent.emit({ type: 'export' });
  }
}
