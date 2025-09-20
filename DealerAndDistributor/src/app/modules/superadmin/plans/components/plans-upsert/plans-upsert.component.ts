import { ChangeDetectionStrategy, Component, OnInit, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { BreadcrumbComponent } from '../../../../../core/shared/components/breadcrumb/breadcrumb.component';
import { PlansService } from '../../service/plans.service';
import { RolesService } from '../../../../roles-and-permission/service/roles.service';
import { ActivatedRoute, Router } from '@angular/router';
import { SUPERADMIN_PLANS_ROUTE } from '../../../../../core/helpers/routes/app-routes';
import { PaginatedApiResponse, ApiResponse } from '../../../../../core/models/interface/ApiResponse';
import { Role } from '../../../../roles-and-permission/models/role.model';
import { Plan, PlanRequest } from '../../models/plan.model';

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
    BreadcrumbComponent
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

  roles = signal<Role[]>([]);
  breadcrumbTitle = computed(() => this.isEdit() ? 'Edit Plan' : 'Create Plan');

  constructor(
    private readonly plansService: PlansService,
    private readonly rolesService: RolesService,
    private readonly route: ActivatedRoute,
    private readonly router: Router
  ) {}

  ngOnInit(): void {
    this.loading.set(true);
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      this.isEdit.set(!!id);
      this.currentId = id ? Number(id) : null;

      // Load roles first (roleTypes=0)
      this.rolesService.getRolesByType([0]).subscribe({
        next: (res: ApiResponse<Role[]>) => {
          this.roles.set(res.data ?? []);
          if (this.currentId) {
            this.loadPlan(this.currentId);
          } else {
            this.loading.set(false);
          }
        },
        error: () => {
          this.roles.set([]);
          if (this.currentId) this.loadPlan(this.currentId); else this.loading.set(false);
        }
      });
    });
  }

  private loadPlan(id: number) {
    this.plansService.getById(id).subscribe({
      next: (res: ApiResponse<Plan & { roleIds: number[] }>) => {
        const p = res.data as Plan & { roleIds: number[] };
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
      roleIds: (raw.roleIds || []).map((id: number | string) => Number(id))
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
