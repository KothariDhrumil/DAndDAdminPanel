import { ChangeDetectionStrategy, Component, Inject, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { SupportTicketPayload, SupportTicketService } from '../../services/support-ticket.service';
import { MatProgressBarModule } from '@angular/material/progress-bar';

@Component({
  selector: 'app-error-ticket',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule, MatProgressBarModule],
  templateUrl: './error-ticket.component.html',
  styleUrls: ['./error-ticket.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ErrorTicketComponent {
  private service = inject(SupportTicketService);
  private dialogRef = inject(MatDialogRef<ErrorTicketComponent>);
  loading = false;

  constructor(@Inject(MAT_DIALOG_DATA) public data: { payload: SupportTicketPayload, dialogDisplay?: { correlationId?: string; timestamp?: string; message?: string } }) {}

  submit() {
    this.loading = true;
    this.service.raise(this.data.payload).subscribe({
      next: () => { this.loading = false; this.dialogRef.close(true); },
      error: () => { this.loading = false; this.dialogRef.close(false); }
    });
  }

  cancel() { this.dialogRef.close(false); }
}
