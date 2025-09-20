import { ChangeDetectionStrategy, Component, OnInit, signal } from '@angular/core';
import { CommonModule, NgOptimizedImage } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatTabsModule } from '@angular/material/tabs';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatDividerModule } from '@angular/material/divider';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { BreadcrumbComponent } from '../../../../../core/shared/components/breadcrumb/breadcrumb.component';
import { ActivatedRoute } from '@angular/router';
import { TenantDetailService, TenantPlanItem } from './service/tenant-detail.service';
import { Tenant } from '../../models/tenant.model';
import { MatCheckbox } from "@angular/material/checkbox";
import { MatInputModule } from "@angular/material/input";
import { MatFormFieldModule } from "@angular/material/form-field";

@Component({
    selector: 'app-tenant-detail',
    templateUrl: './tenant-detail.component.html',
    styleUrls: ['./tenant-detail.component.scss'],
    standalone: true,
    imports: [
        CommonModule,
        MatCardModule,
        MatTabsModule,
        MatIconModule,
        MatListModule,
        MatDividerModule,
        MatButtonModule,
        MatChipsModule,
        BreadcrumbComponent,
        MatCheckbox,
        MatInputModule,
        MatFormFieldModule
    ],
    changeDetection: ChangeDetectionStrategy.OnPush,
    host: {
        class: 'tenant-detail-page'
    }
})
export class TenantDetailComponent implements OnInit {
    // Data signals
    readonly tenant = signal<Tenant | null>(null);
    readonly activePlan = signal<TenantPlanItem | null>(null);
    readonly planHistory = signal<TenantPlanItem[]>([]);

    constructor(
        private readonly route: ActivatedRoute,
        private readonly service: TenantDetailService
    ) {}

    ngOnInit(): void {
        this.route.paramMap.subscribe(pm => {
            const idParam = pm.get('id');
            if (!idParam) return; // no id provided
            const tenantId = Number(idParam);
            if (Number.isNaN(tenantId)) return;

            // Load tenant
            this.service.getTenantById(tenantId).subscribe(res => {
                if (res?.isSuccess && res.data) {
                    this.tenant.set(res.data);
                }
            });

            // Load active plan, then plan history
            this.service.getActivePlan(tenantId).subscribe(ap => {
                if (ap?.isSuccess && ap.data) {
                    this.activePlan.set(ap.data);
                    const histKey = ap.data.tenantPlanId ?? tenantId;
                    this.loadPlanHistory(histKey);
                } else {
                    // Fallback to using tenantId if active plan not found
                    this.loadPlanHistory(tenantId);
                }
            }, _ => this.loadPlanHistory(tenantId));
        });
    }

    private loadPlanHistory(key: number) {
        this.service.getPlanHistory(key).subscribe(ph => {
            const data = ph?.data as any;
            if (!data) { this.planHistory.set([]); return; }
            // Handle both array or single object responses gracefully
            this.planHistory.set(Array.isArray(data) ? data as TenantPlanItem[] : [data as TenantPlanItem]);
        });
    }
}
