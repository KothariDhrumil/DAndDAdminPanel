import { ChangeDetectionStrategy, Component, Inject, OnInit, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { PlansService } from '../../../../plans/service/plans.service';
import { Plan } from '../../../../plans/models/plan.model';
import { RolesSelectorComponent } from '../../../../../../core/shared/components/roles-selector/roles-selector.component';
import { TenantDetailService } from '../../../service/tenant-detail.service';
import { TenantPlanItem } from '../../../models/tenant-plan.model';
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";

export interface AddTenantPlanDialogData {
    tenantId: number;
    hasActivePlan?: boolean; // for overwrite confirmation
    // Upsert mode: if provided, dialog edits this active plan
    tenantPlanId?: number;
    activePlan?: TenantPlanItem | null;
}

@Component({
    selector: 'app-add-tenant-plan-dialog',
    standalone: true,
    imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatDialogModule,
    MatButtonModule,
    RolesSelectorComponent,
    MatProgressSpinnerModule
],
    templateUrl: './add-tenant-plan-dialog.component.html',
    styleUrls: ['./add-tenant-plan-dialog.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class AddTenantPlanDialogComponent implements OnInit {
    readonly plans = signal<Plan[]>([]);
    readonly loadingPlans = signal(false);
    readonly errorPlans = signal<string | null>(null);
    readonly saving = signal(false);
    readonly submitError = signal<string | null>(null);

    readonly today = new Date();

    readonly selectedPlanId = signal<number | null>(null);
    readonly selectedPlan = computed<Plan | null>(() => {
        const id = this.selectedPlanId();
        if (id == null) return null;
        return this.plans().find(p => p.id === id) ?? null;
    });

    form = new FormGroup({
        planId: new FormControl<number | null>(null, { validators: [Validators.required] }),
        isActive: new FormControl<boolean>(true, { nonNullable: true }),
        validFrom: new FormControl<Date | null>(new Date(), { validators: [Validators.required] }),
        validTo: new FormControl<Date | null>(null, { validators: [Validators.required] }),
        remarks: new FormControl<string | null>(null),
        roles: new FormControl<number[]>([], { nonNullable: true })
    });

    private isEditModeFlag = false;
    private editingPrefill = false;

    constructor(
        private readonly dialogRef: MatDialogRef<AddTenantPlanDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: AddTenantPlanDialogData,
        private readonly plansService: PlansService,
        private readonly service: TenantDetailService
    ) {
        this.isEditModeFlag = !!(data && (data.tenantPlanId || data.activePlan));
    }

    ngOnInit(): void {
        this.loadPlans();
        this.form.controls['validFrom'].valueChanges.subscribe((date: Date | null) => this.updateValidToFrom(date));
        this.form.controls['planId'].valueChanges.subscribe((pid: number | null) => {
            this.selectedPlanId.set(pid);
            this.onPlanSelected(pid);
        });

        // If in edit mode, prefill form with existing active plan values
        const ap = this.data.activePlan;
        if (ap) {
            // Convert dates from string (yyyy-MM-dd or ISO) to Date
            const from = ap.validFrom ? new Date(ap.validFrom) : new Date();
            const to = ap.validTo ? new Date(ap.validTo) : this.addDays(from, 0);
            // We'll set planId after plans load to ensure it exists in options
            this.form.patchValue({
                isActive: ap.isActive,
                validFrom: from,
                validTo: to,
                remarks: ap.remarks ?? null
            }, { emitEvent: false });
            // Mark that the next plan selection is from edit prefill
            this.editingPrefill = true;
        }
    }

    private loadPlans(): void {
        this.loadingPlans.set(true);
        this.errorPlans.set(null);
        this.plansService.list().subscribe({
            next: (res) => {
                this.plans.set(res.data || []);
                this.loadingPlans.set(false);
                // In edit mode, set the existing plan selection if it matches one of the options
                const ap = this.data.activePlan;
                if (ap) {
                    // Prefer matching by ID if present in the list (some APIs include it)
                    let match = this.plans().find(p => (p as any).tenantPlanId === ap.tenantPlanId || p.id === (ap as any).planId);
                    // Fallback: match by plan name if ID not available in plan model
                    if (!match) match = this.plans().find(p => p.name === ap.planName);
                    if (match) {
                        this.form.patchValue({ planId: match.id }, { emitEvent: true });
                        this.selectedPlanId.set(match.id);
                    }
                }
                // Otherwise pre-select first plan if none selected
                if (!this.form.value.planId && this.plans().length) {
                    const first = this.plans()[0];
                    this.form.patchValue({ planId: first.id });
                    this.selectedPlanId.set(first.id);
                    this.applyPlanDefaults(first);
                }
            },
            error: (_err) => { this.errorPlans.set('Failed to load plans'); this.loadingPlans.set(false); }
        });
    }

    private onPlanSelected(planId: number | null): void {
        const plan = this.plans().find(p => p.id === planId);
        if (plan) this.applyPlanDefaults(plan);
    }

    private applyPlanDefaults(plan: Plan): void {
        // Prefill validity and roles from plan
        const from = this.form.controls['validFrom'].value ?? new Date();
        const to = this.addDays(from, plan.planValidityInDays);
        // In edit-mode initial prefill, avoid overriding existing dates
        if (!(this.isEditModeFlag && this.editingPrefill)) {
            this.form.patchValue({ validFrom: from, validTo: to });
        }
        this.form.controls['roles'].setValue((plan.roleIds ?? []).map(n => Number(n)), { emitEvent: true });
        // After first prefill in edit mode, allow subsequent changes to update dates
        if (this.editingPrefill) {
            this.editingPrefill = false;
        }
    }

    private updateValidToFrom(fromDate: Date | null): void {
        const planId = this.form.value.planId;
        const plan = this.plans().find(p => p.id === planId);
        if (!fromDate || !plan) return;
        const to = this.addDays(fromDate, plan.planValidityInDays);
        this.form.patchValue({ validTo: to }, { emitEvent: false });
    }

    private addDays(date: Date, days: number): Date {
        const d = new Date(date);
        d.setDate(d.getDate() + days);
        return d;
    }

    save(): void {
        this.submitError.set(null);
        if (this.form.invalid) return;

        // If setting active and there's already an active plan, confirm overwrite
        if (!this.data.tenantPlanId && this.data.hasActivePlan && this.form.value.isActive) {
            const confirmed = window.confirm('This tenant already has an active plan. Do you want to overwrite it?');
            if (!confirmed) return;
        }
        this.saving.set(true);
        const v = this.form.getRawValue();
        const payload = {
            planId: v.planId!,
            tenantId: this.data.tenantId,
            isActive: v.isActive,
            validFrom: this.toYmd(v.validFrom!),
            validTo: this.toYmd(v.validTo!),
            remarks: v.remarks ?? undefined,
            roles: (v.roles || []).map(n => Number(n))
        };
        const req$ = this.data.tenantPlanId
            ? this.service.updateTenantPlan(this.data.tenantPlanId, payload)
            : this.service.createTenantPlan(payload);
        req$.subscribe({
            next: res => {
                this.saving.set(false);
                if (res?.isSuccess) this.dialogRef.close(true); else {
                    this.submitError.set(res?.error?.description || 'Failed to save plan');
                }
            },
            error: _ => { this.saving.set(false); this.submitError.set('Failed to save plan'); }
        });
    }

    close(): void {
        this.dialogRef.close(false);
    }

    private toYmd(d: Date): string {
        const pad = (n: number) => n.toString().padStart(2, '0');
        return `${d.getFullYear()}-${pad(d.getMonth() + 1)}-${pad(d.getDate())}`;
    }
}
