import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { GenericTableComponent } from '../../../../core/shared/components/generic-table/generic-table.component';
import { ColumnDefinition, TableEventArgs, RowAction } from '../../../../core/shared/components/generic-table/generic-table.model';
import { TicketsService } from '../../service/tickets.service';
import { UpsertTicketDialogComponent } from '../upsert-ticket-dialog/upsert-ticket-dialog.component';
import { Ticket, TicketPriority, TicketStatus } from '../../models/ticket.model';
import { BreadcrumbComponent } from "@core/shared/components/breadcrumb/breadcrumb.component";

@Component({
    selector: 'app-tickets-list',
    standalone: true,
    imports: [CommonModule, GenericTableComponent, BreadcrumbComponent],
    templateUrl: './tickets-list.component.html',
    styleUrls: ['./tickets-list.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TicketsListComponent {
    private service = inject(TicketsService);
    private router = inject(Router);

    rows = signal<Ticket[]>([]);
    total = signal(0);
    pageNumber = 1;
    pageSize = 10;
    search = '';
    sortBy = '';
    sortDir: 'asc' | 'desc' | '' = '';

    columns: ColumnDefinition[] = [
        { def: 'id', label: 'ID', type: 'text', sortable: true  },
        { def: 'message', label: 'Message', type: 'text', sortable: true},
        { def: 'url', label: 'URL', type: 'text', sortable: true},
        { def: 'method', label: 'Method', type: 'text', sortable: true},        
        { def: 'ticketStatus', label: 'Ticket Status', type: 'badge', badgeColorField: 'ticketStatusColor' },
        { def: 'priorityText', label: 'Priority', type: 'badge', badgeColorField: 'priorityColor' },
        { def: 'createdAt', label: 'Created', type: 'date', sortable: true},
        { def: 'updatedAt', label: 'Updated', type: 'date', sortable: true},
        { def: 'actionBtn', label: 'Actions', type: 'actionBtn'},
    ];

    config = {
        enableSearch: true,
        enableRefresh: true,
        enableAdd: true,
        pageSize: 10,
        pageSizeOptions: [5, 10, 25, 50, 100],
        title: 'Support Tickets'
    }

    actions: RowAction[] = [
        { name: 'open', icon: 'external-link', tooltip: 'Open' },
    ];

    constructor() {
        this.load();
    }

    // Expose component type for GenericTable formDialogComponent binding
    UpsertTicketDialogComponent = UpsertTicketDialogComponent;

    onTableEvent(e: TableEventArgs) {
        if (e.type === 'page' && e.page) {
            this.pageNumber = e.page.pageIndex + 1;
            this.pageSize = e.page.pageSize;
            this.load();
        }
        if (e.type === 'filter') {
            this.search = e.filter ?? '';
            this.load();
        }
        if (e.type === 'sort' && e.sort) {
            this.sortBy = e.sort.active;
            this.sortDir = (e.sort.direction as any) ?? '';
            this.load();
        }
        if (e.type === 'refresh') {
            this.load();
        }
        if (e.type === 'row' && e.data) {
            // Row click navigates to detail
            this.router.navigate(['/tickets', e.data.id]);
        }
        if (e.type === 'custom' && e.action === 'open' && e.data) {
            this.router.navigate(['/tickets', e.data.id]);
        }
    }

    private load() {
        this.service.list({
            pageNumber: this.pageNumber,
            pageSize: this.pageSize,
            search: this.search,
            sortBy: this.sortBy,
            sortDir: (this.sortDir as any) || undefined,
        }).subscribe(res => {
            const list = (res?.data ?? []) as Ticket[];
            const mapped = list.map(t => ({
                ...t,               
                ticketStatus: this.statusToText(t.ticketStatus as unknown as TicketStatus),
                priorityText: this.priorityToText(t.priority as unknown as TicketPriority),
                ticketStatusColor: (() => {
                    switch (t.ticketStatus) {
                        case TicketStatus.Open: return 'red';
                        case TicketStatus.InProgress: return 'orange';
                        case TicketStatus.Resolved: return 'purple';
                        case TicketStatus.Closed: return 'green';
                        default: return 'red';
                    }
                })(),
                priorityColor: (() => {
                    switch (t.priority) {
                        case TicketPriority.Low: return 'green';
                        case TicketPriority.Medium: return 'orange';
                        case TicketPriority.High: return 'purple';
                        case TicketPriority.Urgent: return 'red';
                        default: return 'red';
                    }
                })()
            }));
            this.rows.set(mapped as any);
            this.total.set(res?.totalRecords ?? list.length);
        });
    }

    private statusToText(s: TicketStatus): string {
        switch (s) {
            case TicketStatus.Open: return 'Open';
            case TicketStatus.InProgress: return 'InProgress';
            case TicketStatus.Resolved: return 'Resolved';
            case TicketStatus.Closed: return 'Closed';
            default: return String(s);
        }
    }

    private priorityToText(p: TicketPriority): string {
        switch (p) {
            case TicketPriority.Low: return 'Low';
            case TicketPriority.Medium: return 'Medium';
            case TicketPriority.High: return 'High';
            case TicketPriority.Urgent: return 'Urgent';
            default: return String(p);
        }
    }
}
