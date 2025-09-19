import { environment } from "../../../../environments/environment";

// API endpoint constants
const baseUrl = environment.apiUrl;
const apiVersionNeutral = '/api';

 /*/ Authentication Routes */
const authRoute = '/account';
export const LOGIN_API = baseUrl + apiVersionNeutral + authRoute + '/authenticate';
export const REFRESH_TOKEN_API = baseUrl + apiVersionNeutral + authRoute + '/refresh';
export const LOGOUT = baseUrl + apiVersionNeutral + authRoute + '/logout';
export const REGISTER_API = baseUrl + apiVersionNeutral + authRoute + '/register';
export const GENERATE_OTP_API = baseUrl + apiVersionNeutral + authRoute + '/generate-otp';
export const CONFIRM_OTP_API = baseUrl + apiVersionNeutral + authRoute + '/confirm-otp';


/*/ User Management Routes */
const userRoute = '/user';

export const USER_INFO_API = baseUrl + apiVersionNeutral + userRoute + '/info';

export const USER_PERMISSIONS_API = baseUrl + apiVersionNeutral + userRoute + '/permissions';

/*/ Superadmin Routes */
export const TENANTS_API = baseUrl + apiVersionNeutral + '/tenants';
export const API_SHARDING = baseUrl + apiVersionNeutral + '/sharding';
export const API_GET_DB_DETAILS = API_SHARDING + '/get-db-details';

/*/ Admin Routes */

export const AUTH_ROLES_API = '/api/admin/auth-roles';
// Shared roles endpoint for roles-and-permissions component
export const API_ROLES = baseUrl + apiVersionNeutral +'/roles';
export const LIST_ALL_AUTH_USERS_API = '/api/admin/auth-users';
export const SYNC_AUTH_USER_WITH_CHANGE_LIST_API = '/api/admin/sync-auth-user-with-change-list';
