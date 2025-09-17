import { MatTableDataSource } from '@angular/material/table';

export interface ColumnDefinition {
  def: string;
  label: string;
  type: 'text' | 'date' | 'phone' | 'email' | 'address' | 'image' | 'badge' | 'check' | 'actionBtn';
  hide?: boolean;
  sortable?: boolean;
  tooltip?: boolean;
}

export interface TableConfig {
  enableSelection?: boolean;
  enableSearch?: boolean;
  enableExport?: boolean;
  enableRefresh?: boolean;
  enableColumnHide?: boolean;
  enableAdd?: boolean;
  enableEdit?: boolean;
  enableDelete?: boolean;
  enableContextMenu?: boolean;
  pageSize?: number;
  pageSizeOptions: number[];
  title?: string;
}

export interface SortInfo {
  active: string;
  direction: 'asc' | 'desc' | '';
}

export interface PageInfo {
  pageIndex: number;
  pageSize: number;
  length?: number;
}

export interface TableEventArgs {
  type: 'add' | 'edit' | 'delete' | 'refresh' | 'export' | 'selection' | 'row' | 'filter' | 'sort' | 'page';
  data?: any;
  sort?: SortInfo;
  page?: PageInfo;
  filter?: string;
}

export interface ContextMenuPosition {
  x: string;
  y: string;
}
