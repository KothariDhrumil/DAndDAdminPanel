import { ChangeDetectionStrategy, Component, inject, signal, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogRef, MatDialogActions, MatDialogContent, MatDialogTitle, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CustomersService } from '../../service/customers.service';
import { takeUntil, debounceTime, filter, switchMap } from 'rxjs/operators';
import { Subject, of } from 'rxjs';

export interface AddCustomerDialogData { parentGlobalCustomerId?: string | null }

@Component({
  selector: 'app-add-customer-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatDialogContent, MatDialogTitle, MatDialogActions],
  templateUrl: './add-customer-dialog.component.html',
  styleUrls: ['./add-customer-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AddCustomerDialogComponent {
  private fb = inject(FormBuilder);
  private service = inject(CustomersService);
  private dialogRef = inject(MatDialogRef<AddCustomerDialogComponent>);
  constructor(@Inject(MAT_DIALOG_DATA) public data: AddCustomerDialogData) {
    // existing initialization moved to after form definition below
    this.initPhoneWatcher();
  }

  private destroy$ = new Subject<void>();

  searching = signal(false);
  existingCustomerId = signal<string | null>(null);

  form = this.fb.group({
    phoneNumber: this.fb.control<string>('', { nonNullable: true, validators: [Validators.required, Validators.pattern(/^[0-9]{10}$/)] }),
    firstName: this.fb.control<string>({ value: '', disabled: true }, { nonNullable: true, validators: [Validators.required] }),
    lastName: this.fb.control<string>({ value: '', disabled: true }, { nonNullable: true, validators: [Validators.required] }),
  });

  private initPhoneWatcher() {
    this.form.controls.phoneNumber.valueChanges.pipe(
      debounceTime(300),
      filter(v => /^[0-9]{10}$/.test(v)),
      switchMap(phone => {
        this.searching.set(true);
        this.existingCustomerId.set(null);
        this.form.controls.firstName.setValue('');
        this.form.controls.lastName.setValue('');
        return this.service.searchByPhone(phone);
      })
    ).subscribe({
      next: res => {
        this.searching.set(false);
        const customer = res?.data ?? null;
        if (customer) {
          this.existingCustomerId.set(customer.globalCustomerId);
          this.form.controls.firstName.setValue(customer.firstName || '');
          this.form.controls.lastName.setValue(customer.lastName || '');
        } else {
          this.form.controls.firstName.enable();
          this.form.controls.lastName.enable();
        }
      },
      error: () => {
        this.searching.set(false);
        this.form.controls.firstName.enable();
        this.form.controls.lastName.enable();
      }
    });
  }

  submit() {
    if (this.form.invalid) return;
    const phone = this.form.controls.phoneNumber.value;
    const first = this.form.controls.firstName.value;
    const last = this.form.controls.lastName.value;

    const parentId = this.data?.parentGlobalCustomerId || null;
    if (this.existingCustomerId()) {
      this.service.linkExistingCustomer(this.existingCustomerId()!, phone, parentId).subscribe({
        next: () => this.dialogRef.close(true),
        error: () => this.dialogRef.close(false)
      });
    } else {
      this.service.createCustomer({ phoneNumber: phone, firstName: first, lastName: last, parentGlobalCustomerId: parentId || undefined }).subscribe({
        next: () => this.dialogRef.close(true),
        error: () => this.dialogRef.close(false)
      });
    }
  }

  cancel() {
    this.dialogRef.close(false);
  }
}
