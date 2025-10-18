import { environment } from "../../../../environments/environment";

// API endpoint constants
const baseUrl = environment.apiUrl;
const apiVersionNeutral = '/api';
const apiVersionV1 = '/api/v1';

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
export const TENANTS_CREATE_API = `${TENANTS_API}/create`;
export const TENANTS_CHILDREN_API = (tenantId: number) => `${TENANTS_API}/childs/${tenantId}`;
export const TENANT_PLAN_API = baseUrl + apiVersionNeutral + '/tenantplan';
export const TENANT_PLAN_ACTIVE_API = TENANT_PLAN_API + '/active';
export const API_SHARDING = baseUrl + apiVersionNeutral + '/sharding';
export const API_GET_DB_DETAILS = API_SHARDING + '/get-db-details';
export const API_PLANS = baseUrl + apiVersionNeutral + '/plan';
export const API_ROLES = baseUrl + apiVersionNeutral + '/roles';
export const API_ROLES_BY_TYPE = API_ROLES + '/get-roles-by-type';

/*/ Auth Users Routes */
export const AUTHUSERS_API = baseUrl + apiVersionNeutral + '/authusers';
export const AUTHUSERS_UPDATE_TENANT_API = baseUrl + apiVersionNeutral + '/authusers/update-tenant-user';
export const AUTHUSERS_LIST_API = AUTHUSERS_API + '/listusers';
// User Types
export const API_USER_TYPES = baseUrl + apiVersionV1 + '/usertypes';

/*/ Todos Routes */
export const TODOS_API = baseUrl + apiVersionV1 + '/todos';

/*/ Customer Routes */
export const CUSTOMERS_API = baseUrl + apiVersionNeutral + '/customers';
export const CUSTOMERS_WITH_TENANTS_API = `${CUSTOMERS_API}/with-tenants`;
export const CUSTOMERS_BY_TENANT_API = `${CUSTOMERS_API}/by-tenant`;
export const CUSTOMERS_SEARCH_BY_PHONE_API = (phone: string) => `${CUSTOMERS_API}/search/by-phone?phone=${encodeURIComponent(phone)}`;
export const CUSTOMERS_LINK_API = `${CUSTOMERS_API}/link`;
export const CUSTOMERS_CHILD_API = `${CUSTOMERS_API}/child`;
export const CUSTOMERS_CHILD_LINK_API = `${CUSTOMERS_CHILD_API}/link`;
export const CUSTOMER_ORDERS_API = baseUrl + apiVersionNeutral + '/customerorders';

export const DOMAIN_CUSTOMERS_API = baseUrl + apiVersionV1 + '/domainCustomer';


/*/ Admin Routes */

export const AUTH_ROLES_API = '/api/admin/auth-roles';
export const LIST_ALL_AUTH_USERS_API = '/api/admin/auth-users';
export const SYNC_AUTH_USER_WITH_CHANGE_LIST_API = '/api/admin/sync-auth-user-with-change-list';

/*/ Support Tickets */
export const SUPPORT_TICKETS_API = baseUrl + apiVersionNeutral + '/support-tickets';
export const SUPPORT_TICKET_BY_ID_API = (id: string | number) => `${SUPPORT_TICKETS_API}/${id}`;
export const SUPPORT_TICKET_COMMENTS_API = (id: string | number) => `${SUPPORT_TICKETS_API}/${id}/comments`;
export const SUPPORT_TICKET_STATUS_API = (id: string | number) => `${SUPPORT_TICKETS_API}/${id}/status`;
export const SUPPORT_TICKET_ASSIGN_API = (id: string | number) => `${SUPPORT_TICKETS_API}/${id}/assign`;
