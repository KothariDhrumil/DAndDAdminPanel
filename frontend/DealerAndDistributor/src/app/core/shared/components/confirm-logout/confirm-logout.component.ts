import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { MatDialogActions, MatDialogContent, MatDialogRef, MatDialogTitle } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from '@core/service/auth.service';

@Component({
  selector: 'app-confirm-logout',
  standalone: true,
  imports: [MatDialogContent, MatDialogActions, MatDialogTitle, MatButtonModule],
  templateUrl: './confirm-logout.component.html',
  styleUrls: ['./confirm-logout.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ConfirmLogoutComponent {
  private dialogRef = inject(MatDialogRef<ConfirmLogoutComponent>);
  private auth = inject(AuthService);

  logout() {
    this.auth.logout();
    this.dialogRef.close(true);
  }

  cancel() {
    this.dialogRef.close(false);
  }
}
