import { ChangeDetectionStrategy, Component, Inject, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_DIALOG_DATA, MatDialogContent, MatDialogTitle } from '@angular/material/dialog';
import { CustomersService } from '../../service/customers.service';
import { MatListModule } from '@angular/material/list';

@Component({
  selector: 'app-view-orders-dialog',
  standalone: true,
  imports: [CommonModule, MatDialogContent, MatDialogTitle, MatListModule],
  templateUrl: './view-orders-dialog.component.html',
  styleUrls: ['./view-orders-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ViewOrdersDialogComponent {
  private service = inject(CustomersService);
  orders = signal<Array<{ total: number; createdAt?: string }>>([]);

  constructor(@Inject(MAT_DIALOG_DATA) public data: { globalCustomerId: string }) {
    this.service.getCustomerOrders(data.globalCustomerId).subscribe(res => {
      this.orders.set((res?.data || []).map((o: any) => ({ total: o.total, createdAt: o.createdAt })));
    });
  }
}
