import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'permissionFormat',
  standalone: true
})
export class PermissionFormatPipe implements PipeTransform {
  transform(permissions: string[] | null | undefined): string {
    if (!permissions || permissions.length === 0) {
      return 'No permissions';
    }
    
    // Return first 3 permissions + a count of remaining ones
    if (permissions.length > 3) {
      const visiblePermissions = permissions.slice(0, 3).join(', ');
      return `${visiblePermissions}... (+${permissions.length - 3} more)`;
    }
    
    return permissions.join(', ');
  }
}