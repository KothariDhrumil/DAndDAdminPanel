import { ChangeDetectionStrategy, Component, Inject, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { TicketsService } from '../../service/tickets.service';
import { TicketDetails, TicketId, TicketPriority, TicketStatus } from '../../models/ticket.model';

export interface UpdateTicketDialogData { id: TicketId }

@Component({
  selector: 'app-update-ticket-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
  ],
  templateUrl: './update-ticket-dialog.component.html',
  styleUrls: ['./update-ticket-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UpdateTicketDialogComponent {
  private fb = inject(FormBuilder);
  private tickets = inject(TicketsService);
  private dialogRef = inject(MatDialogRef<UpdateTicketDialogComponent, boolean>);

  statusOptions: TicketStatus[] = [TicketStatus.Open, TicketStatus.InProgress, TicketStatus.Resolved, TicketStatus.Closed];
  priorityOptions: TicketPriority[] = [TicketPriority.Low, TicketPriority.Medium, TicketPriority.High, TicketPriority.Urgent];

  loading = false;

  form = this.fb.nonNullable.group({
    notes: [''],
    ticketStatus: [TicketStatus.Open, Validators.required],
    priority: [TicketPriority.Low, Validators.required],
    resolution: [''],
  });

  constructor(@Inject(MAT_DIALOG_DATA) public data: UpdateTicketDialogData) {
    // Prefill with current values
    if (data?.id) {
      this.loading = true;
      this.tickets.getById(data.id).subscribe({
        next: (res) => {
          const t = res.data as TicketDetails;
          this.form.patchValue({
            notes: t.notes ?? '',
            ticketStatus: t.ticketStatus as TicketStatus,
            priority: t.priority as TicketPriority,
            resolution: t.resolution ?? '',
          });
          this.loading = false;
        },
        error: () => { this.loading = false; }
      });
    }
  }

  save() {
    if (this.form.invalid || !this.data?.id) return;
    const { notes, ticketStatus, priority, resolution } = this.form.getRawValue();
    this.loading = true;
    this.tickets.update(this.data.id, {
      id: this.data.id,
      notes: notes ?? '',
      ticketStatus: ticketStatus as TicketStatus,
      priority: priority as TicketPriority,
      resolution: resolution ?? '',
    }).subscribe({
      next: () => { this.loading = false; this.dialogRef.close(true); },
      error: () => { this.loading = false; this.dialogRef.close(false); }
    });
  }

  cancel() { this.dialogRef.close(false); }
}
