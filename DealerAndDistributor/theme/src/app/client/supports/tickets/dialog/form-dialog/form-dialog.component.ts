import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogContent,
  MatDialogClose,
} from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import {
  UntypedFormControl,
  Validators,
  UntypedFormGroup,
  UntypedFormBuilder,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { TicketsService } from '../../tickets.service';
import { Tickets } from '../../tickets.model';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

export interface DialogData {
  id: number;
  action: string;
  ticket: Tickets;
}

@Component({
  selector: 'app-tickets-form',
  templateUrl: './form-dialog.component.html',
  styleUrls: ['./form-dialog.component.scss'],
  imports: [
    MatButtonModule,
    MatIconModule,
    MatDialogContent,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatOptionModule,
    MatDatepickerModule,
    MatDialogClose,
  ],
})
export class TicketsFormComponent {
  action: string;
  dialogTitle: string;
  ticketForm: UntypedFormGroup;
  ticket: Tickets;

  constructor(
    public dialogRef: MatDialogRef<TicketsFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public ticketsService: TicketsService,
    private fb: UntypedFormBuilder
  ) {
    // Determine action type (edit or new)
    this.action = data.action;
    this.dialogTitle =
      this.action === 'edit' ? data.ticket.ticket_id : 'New Ticket';
    this.ticket =
      this.action === 'edit' ? data.ticket : new Tickets({} as Tickets);

    // Create form with predefined structure
    this.ticketForm = this.createTicketForm();
  }

  // Helper to create a new form group with controls and validation
  private createTicketForm(): UntypedFormGroup {
    return this.fb.group({
      id: [this.ticket.id],
      ticket_id: [this.ticket.ticket_id, Validators.required],
      createdBy: [this.ticket.createdBy, Validators.required],
      subject: [this.ticket.subject, Validators.required],
      status: [this.ticket.status, Validators.required],
      assignTo: [this.ticket.assignTo],
      date: [this.ticket.date, Validators.required],
      details: [this.ticket.details],
    });
  }

  // Handle error message display
  getErrorMessage(controlName: string): string {
    const control = this.ticketForm.get(controlName);
    return control?.hasError('required') ? 'Required field' : '';
  }

  // Handle form submission
  submit(): void {
    if (this.ticketForm.valid) {
      const ticketData = this.ticketForm.getRawValue();
      if (this.action === 'edit') {
        this.ticketsService.updateTicket(ticketData).subscribe({
          next: (response) => this.dialogRef.close(response),
          error: (error) => console.error('Update Error:', error),
        });
      } else {
        this.ticketsService.addTicket(ticketData).subscribe({
          next: (response) => this.dialogRef.close(response),
          error: (error) => console.error('Add Error:', error),
        });
      }
    }
  }

  // Close the dialog without action
  onNoClick(): void {
    this.dialogRef.close();
  }

  // Trigger form submission when confirmed
  public confirmAdd(): void {
    this.submit();
  }
}
