import { TenantsComponent } from './tenants.component';
import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { of } from 'rxjs';
import { BreadcrumbComponent } from '../../../../../core/shared/components/breadcrumb/breadcrumb.component';
import { ApiResponse, PaginatedApiResponse } from '../../../../../core/models/interface/ApiResponse';
import { GenericTableComponent } from '../../../../../core/shared/components/generic-table/generic-table.component';
import { Tenant } from '../../models/tenant.model';
import { TenantsService } from '../../service/tenants.service';
// TenantsResponse deprecated; service returns PaginatedApiResponse<Tenant[]> now

describe('TenantsComponent', () => {
    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [HttpClientTestingModule, GenericTableComponent,BreadcrumbComponent],
            providers: [TenantsService],
        });
    });

    it('should create TenantsComponent', () => {
        const fixture = TestBed.createComponent(TenantsComponent);
        const component = fixture.componentInstance;
        expect(component).toBeTruthy();
    });

    it('should fetch tenants on init', () => {
        const fixture = TestBed.createComponent(TenantsComponent);
        const component = fixture.componentInstance;
        const service = TestBed.inject(TenantsService);
                        const mockPaginatedResponse: PaginatedApiResponse<Tenant[]> = {
                                data: [] as Tenant[],
                        isSuccess: true,
                        isFailure: false,
                        error: { code: '', description: '', type: 0 },
                        pageNumber: 1,
                        pageSize: 10,
                        totalRecords: 0,
                };
        spyOn(service, 'getTenants').and.returnValue(of(mockPaginatedResponse));
        fixture.detectChanges();
        expect(component.tenants().length).toBe(0);
        fixture.detectChanges();
        expect(component.tenants().length).toBe(0);
    });
});
