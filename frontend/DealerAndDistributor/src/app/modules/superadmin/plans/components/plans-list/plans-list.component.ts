import { Component, ChangeDetectionStrategy, computed, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GenericTableComponent } from '../../../../../core/shared/components/generic-table/generic-table.component';
import { ColumnDefinition, TableConfig, TableEventArgs } from '../../../../../core/shared/components/generic-table/generic-table.model';
import { BreadcrumbComponent } from '../../../../../core/shared/components/breadcrumb/breadcrumb.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTooltipModule } from '@angular/material/tooltip';
import { PlansService } from '../../service/plans.service';
import { Plan } from '../../models/plan.model';
import { ApiResponse } from '../../../../../core/models/interface/ApiResponse';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-plans-list',
  templateUrl: './plans-list.component.html',
  styleUrls: ['./plans-list.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    GenericTableComponent,
    BreadcrumbComponent,
    MatProgressSpinnerModule,
    MatTooltipModule
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PlansListComponent implements OnInit {
  readonly loading = signal<boolean>(false);
  readonly plansResponse = signal<Plan[] | null>(null);
  // no dialog; we navigate to dedicated upsert pages

  readonly columns = signal<ColumnDefinition[]>([
    { def: 'id', label: 'ID', type: 'text', sortable: true },
    { def: 'name', label: 'Name', type: 'text', sortable: true },
    { def: 'description', label: 'Description', type: 'text', sortable: true, tooltip: true },
    { def: 'planValidityInDays', label: 'Validity (days)', type: 'text', sortable: true },
    { def: 'planRate', label: 'Rate', type: 'text', sortable: true },
    { def: 'isActive', label: 'Active', type: 'check', sortable: true },
    { def: 'actions', label: 'Actions', type: 'actionBtn', sortable: false },
  ]);

  readonly tableConfig = signal<TableConfig>({
    enableSearch: true,
    enableRefresh: true,
    pageSizeOptions: [5, 10, 25, 50],
    title: 'Plans',
    enableAdd: true,
    enableEdit: true,
    enableDelete: true,
    pageSize: 10,
  });

  readonly plans = computed(() => this.plansResponse() ?? []);
  readonly totalRecords = computed(() => this.plansResponse()?.length ?? 0);

  constructor(private readonly plansService: PlansService, private router: Router, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.plansService.list().subscribe({
      next: (res: ApiResponse<Plan[]>) => { this.plansResponse.set(res.data ?? []); this.loading.set(false); },
      error: () => this.loading.set(false)
    });
  }

  onTableEvent(event: TableEventArgs): void {
    if (event.type === 'refresh') this.load();
    if (event.type === 'add') this.router.navigate(['add'], { relativeTo: this.route });
    if (event.type === 'edit' && event.data) this.router.navigate(['edit', (event.data as Plan).id], { relativeTo: this.route });
    if (event.type === 'delete' && event.data) this.deletePlan(event.data as Plan);
  }

  deletePlan(plan: Plan) {
    if (!confirm(`Delete plan ${plan.name}?`)) return;
    this.plansService.delete(plan.id).subscribe({ next: () => this.load() });
  }
}
