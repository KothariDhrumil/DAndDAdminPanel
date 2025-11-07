# Copilot Instructions for DealerAndDistributor Angular Application

## Project Overview
- This is an Angular 20+ monorepo for a Dealer & Distributor management application.
- The main app is in `src/app/` with core logic in `core/`, UI in `layout/`, and feature modules in `modules/`.
- Configuration, enums, guards, interceptors, models, and services are organized under `src/app/core/`.
- Assets (images, fonts, i18n, etc.) are in `src/assets/`.

## Developer Workflows
- **Start Dev Server:** `ng serve` or `npm start` (see tasks.json)
- **Build:** `ng build` (outputs to `dist/`)
- **Unit Tests:** `ng test` (Karma)
- **E2E Tests:** `ng e2e` (framework not included by default)
- **Scaffold Components:** `ng generate component <name>`

## Key Architectural Patterns
- **Core Services:** Auth, config, language, and startup logic are in `core/service/`.
- **Guards & Interceptors:** Route guards and HTTP interceptors are in `core/guard/` and `core/interceptor/`.
- **Enums & Models:** Domain enums and interfaces are in `core/enums/` and `core/models/`.
- **Shared Utilities:** Common helpers and adapters are in `core/shared/`.
- **Feature Modules:** Each business area (authentication, superadmin, user) is a module under `modules/`.
- **Layout:** UI structure (header, sidebar, loader) is in `layout/`.

## Project-Specific Conventions
- **SCSS Structure:** Global styles in `src/scss/`, component styles colocated.
- **i18n:** Language files in `assets/i18n/` (JSON per language).
- **Routing:** Centralized in `app.routes.ts` and feature module routes.
- **Permissions:** Enum-based permissions in `core/enums/permissions.enum.ts`.
- **Startup Logic:** App initialization in `core/service/startup.service.ts` and `core/initializers.ts`.

## Integration Points
- **JWT Auth:** See `core/interceptor/jwt.interceptor.ts` and `core/service/auth.service.ts`.
- **Error Handling:** See `core/interceptor/error.interceptor.ts`.
- **Right Sidebar:** Managed by `core/service/rightsidebar.service.ts`.
- **Language Switching:** Managed by `core/service/language.service.ts`.

## Examples
- To add a new feature module: create under `src/app/modules/`, update `app.routes.ts`.
- To add a new permission: update `core/enums/permissions.enum.ts` and reference in guards/services.
- To add a new language: add JSON to `assets/i18n/`, update `language.service.ts`.

## References
- See `README.md` for CLI commands and build/test instructions.
- See `angular.json` for workspace configuration.
- See `tsconfig*.json` for TypeScript settings.

---
For questions or unclear conventions, ask for clarification or review the referenced files above.


## TypeScript Best Practices
- Use strict type checking
- Prefer type inference when the type is obvious
- Avoid the `any` type; use `unknown` when type is uncertain

## Angular Best Practices
- Always use standalone components over NgModules
- Do NOT set `standalone: true` inside the `@Component`, `@Directive` and `@Pipe` decorators
- Use signals for state management
- Implement lazy loading for feature routes
- Use `NgOptimizedImage` for all static images
- Always create full components for routes, even if they are just a single component
- Do NOT use the `@HostBinding` and `@HostListener` decorators. Put host bindings inside the `host` object of the `@Component` or `@Directive` decorator instead
- Always use reactive forms over template-driven forms
- Use `ng-container` for structural directives to avoid unnecessary DOM elements
- Use `ng-template` for reusable templates
- Always create forms with strongly typed form controls
- Always use Angular Material and its states
- Always use Angular CDK for drag and drop, virtual scrolling, and other advanced features
- Use Angular CLI for generating components, services, and other code scaffolding
- Always use `HttpClient` for HTTP requests
- Use interceptors for handling authentication and error handling
- Always unsubscribe from observables to prevent memory leaks, use the `async` pipe or `takeUntil` operator
- Use Angular's built-in internationalization (i18n) for multi-language support
- Always write unit tests for components, services, and other code
- Use Jasmine and Karma for unit testing

## Components
- Keep components small and focused on a single responsibility
- Use `input()` and `output()` functions instead of decorators
- Use `computed()` for derived state
- Set `changeDetection: ChangeDetectionStrategy.OnPush` in `@Component` decorator
- Prefer inline templates for small components
- Prefer Reactive forms instead of Template-driven ones
- Do NOT use `ngClass`, use `class` bindings instead
- DO NOT use `ngStyle`, use `style` bindings instead
- Always use formly typed forms with `FormGroup` and `FormControl`
- Any form control should be strongly typed

## State Management
- Use signals for local component state
- Use `computed()` for derived state
- Keep state transformations pure and predictable
- Do NOT use `mutate` on signals, use `update` or `set` instead

## Templates
- Keep templates simple and avoid complex logic
- Use native control flow (`@if`, `@for`, `@switch`) instead of `*ngIf`, `*ngFor`, `*ngSwitch`
- Use the async pipe to handle observables

## Project-Specific Instructions
1. Use `core/shared/components/generic-table/generic-table.component.html` for displaying lists and tables throughout the application. This is the standard table UI and should be reused for all list views unless a special case is required.
2. When using `core/shared/components/generic-table/generic-table.component.html` to display lists, follow the implementation pattern used in the tenants component (HTML and TypeScript), including event handling and structure.
3. Store all API endpoint constants in `core/helpers/routes/api-endpoints.ts`. Always import and use these constants for HTTP requests to ensure consistency and maintainability.
4. Store all application route constants in `core/helpers/routes/app-routes.ts`. Always import and use these constants for navigation and route definitions to avoid hardcoding paths.
5. All API responses must follow one of two formats:
	- Non-paginated: Use the `ApiResponse<T>` interface from `core/models/interface/ApiResponse.ts`.
	- Paginated: Use the `PaginatedApiResponse<T>` interface from `core/models/interface/ApiResponse.ts` (includes pagination fields).
	If you are implementing a new API and have not specified the response format, clarify whether it should be paginated or non-paginated and ensure the correct interface is used.
6. All models and interfaces for a component must be placed in a `models` folder inside that component's folder (e.g., `modules/superadmin/sharding/models`).
7. All services for a component must be placed in a `service` folder inside that component's folder (e.g., `modules/superadmin/sharding/service`).
8. When creating components, always generate all files separately: TypeScript (`.ts`), HTML (`.html`), SCSS (`.scss`), and spec (`.spec.ts`). Write basic unit tests for each component during creation. Do not combine files; keep each file focused and in its respective folder.
9. Always create a folder inside the module folder for each component, and add all related component files there.
10. Do not use `form.get('name')` or similar string-based access for form controls. Always use strongly typed form controls for type safety and maintainability.
11. If `generic-table` is used, always implement all its events in the parent component.
12. If dialog-based edit/update/delete is required, follow the implementation and design pattern of the sharding and sharding-dialog components. Match the UI for header, footer, and overall design; only change the body fields as needed, but follow the same structure and approach.
13. Dialog service-call rule: For any Angular Material dialog, perform all API/service calls inside the dialog component itself. The parent component must not call services in `afterClosed()` to perform create/update/delete. Parents should:
    - Pass required context via `MAT_DIALOG_DATA` (e.g., IDs like `tenantId`).
    - Open the dialog and, after it closes, only refresh/reload data if the dialog indicates a change.
    - Follow consolidated handling patterns like `modules/roles-and-permission/component/add-role/add-role.component.ts` and ensure dialogs like `modules/user/todos/components/todo-dialog/todo-dialog.component.ts` own their service calls.
