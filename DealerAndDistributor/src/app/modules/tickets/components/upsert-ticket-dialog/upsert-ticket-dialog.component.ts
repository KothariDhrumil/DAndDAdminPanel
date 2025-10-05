import { ChangeDetectionStrategy, Component, Inject, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { TicketsService } from '../../service/tickets.service';

@Component({
  selector: 'app-upsert-ticket-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatDialogModule, MatButtonModule, MatFormFieldModule, MatInputModule],
  templateUrl: './upsert-ticket-dialog.component.html',
  styleUrls: ['./upsert-ticket-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UpsertTicketDialogComponent {
  private fb = inject(FormBuilder);
  private service = inject(TicketsService);
  private dialogRef = inject(MatDialogRef<UpsertTicketDialogComponent>);

  form = this.fb.nonNullable.group({
    title: ['', [Validators.required, Validators.maxLength(200)]],
    description: ['']
  });
  loading = false;

  constructor(@Inject(MAT_DIALOG_DATA) public data: { id?: string }) {
    if (data?.id) {
      this.service.getById(data.id).subscribe(res => {
        const t = res.data;
        this.form.patchValue({ title: t.message });
      });
    }
  }

  submit() {
    if (this.form.invalid) return;
    this.loading = true;
    const value = this.form.getRawValue();
    // const req$ = this.data?.id
    //   ? this.service.update(this.data.id!, value)
    //   : this.service.create(value);
    // req$.subscribe({
    //   next: () => { this.loading = false; this.dialogRef.close(true); },
    //   error: () => { this.loading = false; }
    // });
  }

  cancel() {
    this.dialogRef.close(false);
  }
}
