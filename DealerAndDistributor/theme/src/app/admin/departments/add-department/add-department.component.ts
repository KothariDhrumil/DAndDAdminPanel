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
import { Department } from '../all-departments/department.model';


@Component({
    selector: 'app-add-department',
    templateUrl: './add-department.component.html',
    styleUrls: ['./add-department.component.scss'],
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
        MatButtonModule
    ]
})
export class AddDepartmentComponent {
  departmentForm: UntypedFormGroup;
  department!: Department;
  breadscrums = [
    {
      title: 'Add Department',
      items: ['Department'],
      active: 'Add',
    },
  ];
  constructor(private fb: UntypedFormBuilder) {
    this.department = new Department({} as Department);

    this.departmentForm = this.fb.group({
      department_name: [this.department.department_name, [Validators.required]],
      hod: [this.department.hod, [Validators.required]],
      phone: [this.department.phone, [Validators.required]],
      email: [
        this.department.email,
        [Validators.required, Validators.email, Validators.minLength(5)],
      ],
      employee_capacity: [
        this.department.employee_capacity,
        [Validators.required],
      ],
      establishedYear: [this.department.establishedYear, [Validators.required]],
      totalEmployees: [this.department.totalEmployees, [Validators.required]],
    });
  }
  onSubmit() {
    console.log('Form Value', this.departmentForm.value);
  }
}
