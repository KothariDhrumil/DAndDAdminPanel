import { Tenant } from './tenant.model';
import { ApiResponse } from '../../../../core/models/interface/ApiResponse';

export type TenantsResponse = ApiResponse<Tenant[]>;
