import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

export interface UpsertTenantFormValue {
    tenantName: string;
    firstName: string;
    lastName: string;
    phoneNumber: string;
    password: string; // optional for edit
    designationId: number;
}

@Component({
    selector: 'app-upsert-tenant',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatButtonModule, MatIconModule],
    templateUrl: './upsert-tenant.component.html',
    styleUrls: ['./upsert-tenant.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UpsertTenantComponent implements OnInit {
    @Input() title = 'Tenant';
    @Input() submitText = 'Save';
    @Input() showPassword = true; // in signup true, in edit could be false
    @Input() designations: Array<{ id: number; name: string }> = [];
    @Input() initialValue?: Partial<UpsertTenantFormValue>;

    @Output() submitted = new EventEmitter<UpsertTenantFormValue>();

    form!: FormGroup<{
        tenantName: FormControl<string>;
        firstName: FormControl<string>;
        lastName: FormControl<string>;
        phoneNumber: FormControl<string>;
        password: FormControl<string>;
        designationId: FormControl<number>;
    }>;

    hide = signal(true);

    constructor(private fb: FormBuilder) { }

    ngOnInit(): void {
        this.form = this.fb.group({
            tenantName: this.fb.control(this.initialValue?.tenantName ?? '', { validators: [Validators.required], nonNullable: true }),
            firstName: this.fb.control(this.initialValue?.firstName ?? '', { validators: [Validators.required], nonNullable: true }),
            lastName: this.fb.control(this.initialValue?.lastName ?? '', { validators: [Validators.required], nonNullable: true }),
            phoneNumber: this.fb.control(this.initialValue?.phoneNumber ?? '', { validators: [Validators.required, Validators.pattern(/^\+?[0-9]{10,15}$/)], nonNullable: true }),
            password: this.fb.control(this.initialValue?.password ?? '', { validators: this.showPassword ? [Validators.required, Validators.minLength(8)] : [], nonNullable: true }),
            designationId: this.fb.control(this.initialValue?.designationId ?? 0, { validators: [], nonNullable: true }),
        });
    }

    onSubmit(): void {
        if (this.form.invalid) return;
        const raw = this.form.getRawValue();
        const payload: UpsertTenantFormValue = {
            tenantName: raw.tenantName,
            firstName: raw.firstName,
            lastName: raw.lastName,
            phoneNumber: raw.phoneNumber,
            designationId: raw.designationId,
            password: raw.password,
        };
        this.submitted.emit(payload);
    }
}
