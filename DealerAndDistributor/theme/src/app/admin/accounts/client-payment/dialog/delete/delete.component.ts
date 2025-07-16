import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogTitle,
  MatDialogContent,
  MatDialogActions,
  MatDialogClose,
} from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { ClientPaymentService } from '../../client-payment.service';
import { MatButtonModule } from '@angular/material/button';

export interface DialogData {
  payment_id: number;
  client_name: string;
  payment_date: string;
  payment_amount: string;
}

@Component({
    selector: 'app-client-payment-delete',
    templateUrl: './delete.component.html',
    styleUrls: ['./delete.component.scss'],
    imports: [
        MatDialogTitle,
        MatDialogContent,
        MatDialogActions,
        MatButtonModule,
        MatDialogClose,
    ]
})
export class ClientPaymentDeleteComponent {
  constructor(
    public dialogRef: MatDialogRef<ClientPaymentDeleteComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public clientPaymentService: ClientPaymentService
  ) {}
  confirmDelete(): void {
    this.clientPaymentService.deletePayment(this.data.payment_id).subscribe({
      next: (response) => {
        this.dialogRef.close(response); // Close with the response data
        // Handle successful deletion, e.g., refresh the table or show a notification
      },
      error: (error) => {
        console.error('Delete Error:', error);
        // Handle the error appropriately
      },
    });
  }
}
