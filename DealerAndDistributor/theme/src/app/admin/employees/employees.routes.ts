import { Route } from '@angular/router';
import { AddEmployeeComponent } from './add-employee/add-employee.component';
import { Page404Component } from '../../authentication/page404/page404.component';
import { AllemployeesComponent } from './allEmployees/allemployees.component';
import { EditEmployeeComponent } from './edit-employee/edit-employee.component';
import { EmployeeProfileComponent } from './employee-profile/employee-profile.component';
import { EmployeeShiftComponent } from './employee-shift/employee-shift.component';
export const ADMIN_EMPLOYEE_ROUTE: Route[] = [
  {
    path: 'allEmployees',
    component: AllemployeesComponent,
  },
  {
    path: 'add-employee',
    component: AddEmployeeComponent,
  },
  {
    path: 'edit-employee',
    component: EditEmployeeComponent,
  },
  {
    path: 'employee-shift',
    component: EmployeeShiftComponent,
  },
  {
    path: 'employee-profile',
    component: EmployeeProfileComponent,
  },
  { path: '**', component: Page404Component },
];
