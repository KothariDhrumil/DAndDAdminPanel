import { Component } from '@angular/core';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { BreadcrumbComponent } from '@shared/components/breadcrumb/breadcrumb.component';

@Component({
    selector: 'app-edit-department',
    templateUrl: './edit-department.component.html',
    styleUrls: ['./edit-department.component.scss'],
    imports: [
        BreadcrumbComponent,
        FormsModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatSelectModule,
        MatOptionModule,
        MatInputModule,
        MatIconModule,
        MatDatepickerModule,
        MatButtonModule,
    ]
})
export class EditDepartmentComponent {
  departmentForm: UntypedFormGroup;
  formdata = {
    id: 1, // Assuming the department has an id
    img: 'path/to/image.jpg', // Assuming image is a string (could be a URL or path)
    department_name: 'Software Development', // Department name
    hod: 'Sanjay Shah', // Head of department
    phone: '123456789', // Phone number
    email: 'test@example.com', // Email address
    employee_capacity: '230', // Employee capacity
    establishedYear: '1987-02-17T14:22:18Z', // Date (ISO format)
    totalEmployees: '50', // Total employees
  };

  breadscrums = [
    {
      title: 'Edit Department',
      items: ['Department'],
      active: 'Edit',
    },
  ];
  constructor(private fb: UntypedFormBuilder) {
    this.departmentForm = this.createContactForm();
  }
  onSubmit() {
    console.log('Form Value', this.departmentForm.value);
  }
  createContactForm(): UntypedFormGroup {
    return this.fb.group({
      id: [this.formdata.id], // The ID is not validated
      img: [this.formdata.img], // The image path doesn't require validation

      // Department Name with required validation
      department_name: [this.formdata.department_name, [Validators.required]],

      // Head of Department (HOD) with required validation
      hod: [this.formdata.hod, [Validators.required]],

      // Phone with required validation
      phone: [this.formdata.phone, [Validators.required]],

      // Email with required, email format, and minimum length validation
      email: [
        this.formdata.email,
        [Validators.required, Validators.email, Validators.minLength(5)],
      ],

      // Employee Capacity with required validation
      employee_capacity: [
        this.formdata.employee_capacity,
        [Validators.required],
      ],

      // Established Year with required validation
      establishedYear: [this.formdata.establishedYear, [Validators.required]],

      // Total Employees with required validation
      totalEmployees: [this.formdata.totalEmployees, [Validators.required]],
    });
  }
}
