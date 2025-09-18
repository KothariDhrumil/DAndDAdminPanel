import { ChangeDetectionStrategy, Component, computed, OnInit, signal } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ShardingService } from '../../service/sharding.service';
import { Sharding } from '../../models/sharding.model';
import { ShardingDialogComponent } from '../sharding-dialog/sharding-dialog.component';
import { DeleteConfirmDialogComponent } from '../sharding-dialog/delete-confirm-dialog.component';

import { GenericTableComponent } from '../../../../../core/shared/components/generic-table/generic-table.component';
import { BreadcrumbComponent } from '../../../../../core/shared/components/breadcrumb/breadcrumb.component';
import { ApiResponse } from '../../../../../core/models/interface/ApiResponse';
import { ColumnDefinition, TableConfig } from '../../../../../core/shared/components/generic-table/generic-table.model';

@Component({
    selector: 'app-sharding-list',
    templateUrl: './sharding-list.component.html',
    styleUrls: ['./sharding-list.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [
        GenericTableComponent,
        BreadcrumbComponent
    ]
})
export class ShardingListComponent implements OnInit {
    _shardings = signal<Sharding[]>([]);
    isLoading = signal(true);

    _columns = signal<ColumnDefinition[]>([
        { def: 'name', label: 'Name', type: 'text', sortable: true, },
        { def: 'databaseName', label: 'Database Name', type: 'text', sortable: true },
        { def: 'connectionName', label: 'Connection Name', type: 'text', sortable: true },
        { def: 'databaseType', label: 'Database Type', type: 'text', sortable: true },
        { def: 'actions', label: 'Actions', type: 'actionBtn', sortable: false }
    ]);

    columns = computed(() => this._columns());
    shardings = computed(() => this._shardings());


    tableConfig : TableConfig = {
        enableAdd: true,
        enableEdit: true,
        enableDelete: true,
        enableRefresh: true,
        title: 'Sharding List',
        enableSelection: false,
        enableSearch: true,
        enableExport: false,
        enableColumnHide: false,
        enableContextMenu: false,
        pageSize: 10,
        pageSizeOptions: [5, 10, 25, 50, 100],
    };

    constructor(private shardingService: ShardingService, private dialog: MatDialog) { }

    ngOnInit(): void {
        this.loadShardings();
    }

    loadShardings(): void {
        this.isLoading.set(true);
        this.shardingService.getAll().subscribe({
            next: (res: ApiResponse<Sharding[]>) => {
                this._shardings.set(res.data ?? []);
                this.isLoading.set(false);
            },
            error: () => {
                this.isLoading.set(false);
            },
        });
    }
    onTableEvent(event: { type: string; data?: any; result?: any }): void {
        switch (event.type) {
            case 'add':
                if (event.result) {
                    this.shardingService.create(event.result).subscribe(() => {
                        this.loadShardings();
                    });
                }
                break;
            case 'edit':
                if (event.result && event.data) {
                    this.shardingService.update(event.data.name, event.result).subscribe(() => {
                        this.loadShardings();
                    });
                }
                break;
            case 'delete':
                if (event.result && event.data) {
                    this.shardingService.delete(event.data.name).subscribe(() => {
                        this.loadShardings();
                    });
                }
                break;
            case 'refresh':
                this.loadShardings();
                break;
            default:
                break;
        }
    }

}
