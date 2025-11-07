import { ChangeDetectionStrategy, Component, OnInit, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { BreadcrumbComponent } from '../../../../../core/shared/components/breadcrumb/breadcrumb.component';
import { PlansService } from '../../service/plans.service';
import { ActivatedRoute, Router } from '@angular/router';
import { SUPERADMIN_PLANS_ROUTE } from '../../../../../core/helpers/routes/app-routes';
import { Plan, PlanRequest } from '../../models/plan.model';
import { RolesSelectorComponent } from '../../../../../core/shared/components/roles-selector/roles-selector.component';
import { ApiResponse } from '../../../../../core/models/interface/ApiResponse';
import { AuthService, Role } from '@core/index';
import { RoleTypes } from 'src/app/modules/roles-and-permission/models/enums/roletypes.enum';

@Component({
  selector: 'app-plans-upsert',
  templateUrl: './plans-upsert.component.html',
  styleUrls: ['./plans-upsert.component.scss'],
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    BreadcrumbComponent,
    RolesSelectorComponent
  ],
  host: { class: 'plans-upsert-page' }
})
export class PlansUpsertComponent implements OnInit {
  form = new FormGroup({
    name: new FormControl<string>('', { nonNullable: true, validators: [Validators.required] }),
    description: new FormControl<string | null>(null),
    planValidityInDays: new FormControl<number | null>(null, { validators: [Validators.required, Validators.min(1)] }),
    planRate: new FormControl<number | null>(null, { validators: [Validators.required, Validators.min(0)] }),
    isActive: new FormControl<boolean>(true, { nonNullable: true }),
    roleIds: new FormControl<number[]>([], { nonNullable: true })
  });

  loading = signal(false);
  isEdit = signal(false);
  currentId: number | null = null;

  breadcrumbTitle = computed(() => this.isEdit() ? 'Edit Plan' : 'Create Plan');
  roleTypes: number[] = [];

  constructor(
    private readonly plansService: PlansService,
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.loading.set(true);
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      this.isEdit.set(!!id);
      this.currentId = id ? Number(id) : null;

      if (this.currentId) {
        this.loadPlan(this.currentId);
      } else {
        this.loading.set(false);
      }

      if (this.authService.isSuperAdmin) {
        this.roleTypes = [RoleTypes.FeatureRole];
      }
      else {
        this.roleTypes = [];
      }
    });
  }

  private loadPlan(id: number) {
    this.plansService.getById(id).subscribe({
      next: (res: ApiResponse<Plan>) => {
        const p = res.data as Plan;
        this.form.patchValue({
          name: p.name,
          description: p.description ?? null,
          planValidityInDays: p.planValidityInDays,
          planRate: p.planRate,
          isActive: p.isActive,
          roleIds: p.roleIds ?? []
        });
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }

  submit(): void {
    if (this.form.invalid) return;
    this.loading.set(true);
    const raw = this.form.getRawValue();
    const payload: PlanRequest = {
      name: raw.name,
      description: raw.description ?? undefined,
      planValidityInDays: raw.planValidityInDays!,
      planRate: raw.planRate!,
      isActive: raw.isActive,
      roleIds: (raw.roleIds || []).map((id: number | string) => Number(id)),
      planId : this.currentId ?? 0,      
    };

    const obs = this.isEdit() && this.currentId
      ? this.plansService.update(this.currentId, payload)
      : this.plansService.create(payload);

    obs.subscribe({
      next: (res) => {
        this.loading.set(false);
        if (res?.isSuccess) this.router.navigate([SUPERADMIN_PLANS_ROUTE]);
      },
      error: () => this.loading.set(false)
    });
  }

  cancel(): void {
    this.router.navigate([SUPERADMIN_PLANS_ROUTE]);
  }
}
