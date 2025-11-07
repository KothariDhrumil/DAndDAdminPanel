import { Pipe, PipeTransform } from '@angular/core';
import { RoleTypes } from "../models/enums/roletypes.enum";

@Pipe({
  name: 'roleTypeFormat',
  standalone: true
})
export class RoleTypeFormatPipe implements PipeTransform {
  transform(roleType: RoleTypes | number | undefined): string {
    if (roleType === undefined || roleType === null) {
      return 'Unknown';
    }

    switch (roleType) {
      case RoleTypes.Normal:
        return 'Normal';
      case RoleTypes.TenantAutoAdd:
        return 'Tenant Auto Add';
      case RoleTypes.TenantAdminAdd:
        return 'Tenant Admin Add';
      case RoleTypes.TenantCreated:
        return 'Tenant Created';
      case RoleTypes.FeatureRole:
        return 'Feature Role';
      case RoleTypes.HiddenFromTenant:
        return 'Hidden From Tenant';
      default:
        return `Role Type ${roleType}`;
    }
  }
}