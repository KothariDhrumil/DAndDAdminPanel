import { MatTableDataSource } from '@angular/material/table';

export interface ColumnDefinition {
  def: string;
  label: string;
  type: 'text' | 'date' | 'phone' | 'email' | 'address' | 'image' | 'badge' | 'check' | 'actionBtn';
  hide?: boolean;
  sortable?: boolean;
  tooltip?: boolean;
}

// Optional per-row action buttons rendered in the actionBtn column
export interface RowAction {
  // Unique name used to identify the action in table events
  name: string;
  // Feather icon name to render inside the button
  icon: string;
  // Optional tooltip shown on hover
  tooltip?: string;
  // Optional color for the button (e.g., 'primary' | 'accent' | 'warn' or any CSS class)
  color?: string;
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
  type: 'add' | 'edit' | 'delete' | 'refresh' | 'export' | 'selection' | 'row' | 'filter' | 'sort' | 'page' | 'custom';
  data?: any;
  // Present when type is 'custom' to indicate which action was triggered
  action?: string;
  sort?: SortInfo;
  page?: PageInfo;
  filter?: string;
}

export interface ContextMenuPosition {
  x: string;
  y: string;
}
