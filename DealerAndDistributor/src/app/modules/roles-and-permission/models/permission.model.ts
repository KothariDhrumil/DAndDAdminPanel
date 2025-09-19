export interface Permission {
  groupName: string;
  shortName: string;
  description: string;
  permissionName: string;
}

export interface PermissionApiResponse {
  pageNumber: number;
  pageSize: number;
  totalRecords: number;
  data: Permission[];
  isSuccess: boolean;
  isFailure: boolean;
  error: {
    code: string;
    description: string;
    type: number;
  };
}
