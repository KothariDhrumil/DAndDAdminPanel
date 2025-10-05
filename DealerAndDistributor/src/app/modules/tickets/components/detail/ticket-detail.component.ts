import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { TicketsService } from '../../service/tickets.service';
import { Ticket, TicketDetails, TicketPriority, TicketStatus } from '../../models/ticket.model';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatListModule } from '@angular/material/list';
import { MatDividerModule } from '@angular/material/divider';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { FormsModule } from '@angular/forms';
import { BreadcrumbComponent } from "@core/shared/components/breadcrumb/breadcrumb.component";

@Component({
  selector: 'app-ticket-detail',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDialogModule,
    MatTooltipModule,
    MatListModule,
    MatDividerModule,
    MatExpansionModule,
    MatChipsModule,
    MatIconModule,
    FormsModule,
    BreadcrumbComponent,
  ],
  templateUrl: './ticket-detail.component.html',
  styleUrls: ['./ticket-detail.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TicketDetailComponent {
  private route = inject(ActivatedRoute);
  private service = inject(TicketsService);
  private dialog = inject(MatDialog);
  ticket = signal<TicketDetails | null>(null);
  statusOptions: TicketStatus[] = [TicketStatus.Open, TicketStatus.InProgress, TicketStatus.Resolved, TicketStatus.Closed];

  // Safely parsed JSON fields with minimal redaction
  readonly parsedHeaders = computed(() => {
    const t = this.ticket();
    if (!t || !t.headers) return null;
    const obj = this.safeParse(t.headers);
    if (obj && typeof obj === 'object') {
      // Redact Authorization if present
      if ('Authorization' in obj && typeof (obj as any).Authorization === 'string') {
        (obj as any).Authorization = '***';
      }
    }
    return obj;
  });
  readonly parsedRequestBody = computed(() => {
    const t = this.ticket();
    if (!t || !t.requestBody) return null;
    return this.safeParse(t.requestBody);
  });
  readonly parsedResponseBody = computed(() => {
    const t = this.ticket();
    if (!t || !t.responseBody) return null;
    return this.safeParse(t.responseBody);
  });

  constructor() {
    const id = this.route.snapshot.paramMap.get('id')!;
    this.service.getById(id).subscribe(res => {
      this.ticket.set(res.data);
    });
  }

  refresh() {
    const id = this.route.snapshot.paramMap.get('id')!;
    this.service.getById(id).subscribe(res => this.ticket.set(res.data));
  }

  // Helpers
  private safeParse(input: string) {
    try {
      return JSON.parse(input);
    } catch {
      return input; // return raw string if not valid JSON
    }
  }

  statusLabel(s: TicketStatus | number | undefined) {
    switch (s) {
      case TicketStatus.Open: return 'Open';
      case TicketStatus.InProgress: return 'InProgress';
      case TicketStatus.Resolved: return 'Resolved';
      case TicketStatus.Closed: return 'Closed';
      default: return String(s ?? '');
    }
  }

  priorityLabel(p: TicketPriority | number | undefined) {
    switch (p) {
      case TicketPriority.Low: return 'Low';
      case TicketPriority.Medium: return 'Medium';
      case TicketPriority.High: return 'High';
      case TicketPriority.Urgent: return 'Urgent';
      default: return String(p ?? '');
    }
  }

  // Chip colors using Angular Material palette (no custom CSS)
  statusChipColor(s: TicketStatus | number | undefined): 'primary' | 'accent' | 'warn' {
    switch (s) {
      case TicketStatus.Open: return 'warn';
      case TicketStatus.InProgress: return 'accent';
      case TicketStatus.Resolved: return 'primary';
      case TicketStatus.Closed: return 'primary';
      default: return 'primary';
    }
  }

  priorityChipColor(p: TicketPriority | number | undefined): 'primary' | 'accent' | 'warn' {
    switch (p) {
      case TicketPriority.Low: return 'primary';
      case TicketPriority.Medium: return 'accent';
      case TicketPriority.High: return 'accent';
      case TicketPriority.Urgent: return 'warn';
      default: return 'primary';
    }
  }

  // Stringify helpers for rendering inside textareas
  stringifiedHeaders(): string {
    const v = this.parsedHeaders();
    try { return JSON.stringify(v, null, 2); } catch { return String(v ?? ''); }
  }
  stringifiedRequestBody(): string {
    const v = this.parsedRequestBody();
    try { return JSON.stringify(v, null, 2); } catch { return String(v ?? ''); }
  }
  stringifiedResponseBody(): string {
    const v = this.parsedResponseBody();
    try { return JSON.stringify(v, null, 2); } catch { return String(v ?? ''); }
  }


  openUpdateDialog() {
    const t = this.ticket();
    if (!t) return;
    import('../../components/update-ticket-dialog/update-ticket-dialog.component').then(m => {
      const dialogRef = this.dialog.open(m.UpdateTicketDialogComponent, {
        data: { id: t.id },
        width: '560px',
      });
      dialogRef.afterClosed().subscribe(changed => {
        if (changed) this.refresh();
      });
    });
  }
}
