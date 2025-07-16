import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogContent,
  MatDialogClose,
} from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import { EmployeesService } from '../../employees.service';
import {
  UntypedFormControl,
  Validators,
  UntypedFormGroup,
  UntypedFormBuilder,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { Employees } from '../../employees.model';
import { formatDate } from '@angular/common';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';

export interface DialogData {
  id: number;
  action: string;
  employees: Employees;
}

@Component({
    selector: 'app-all-employees-form',
    templateUrl: './form-dialog.component.html',
    styleUrls: ['./form-dialog.component.scss'],
    imports: [
        MatButtonModule,
        MatIconModule,
        MatDialogContent,
        FormsModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatSelectModule,
        MatInputModule,
        MatDatepickerModule,
        MatDialogClose,
    ]
})
export class AllEmployeesFormComponent {
  action: string;
  dialogTitle: string;
  employeesForm: UntypedFormGroup;
  employees: Employees;

  constructor(
    public dialogRef: MatDialogRef<AllEmployeesFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public employeesService: EmployeesService,
    private fb: UntypedFormBuilder
  ) {
    // Set the defaults based on action type
    this.action = data.action;
    this.dialogTitle =
      this.action === 'edit'
        ? `Edit Employee: ${data.employees.name}`
        : 'New Employee';
    this.employees =
      this.action === 'edit' ? data.employees : new Employees({} as Employees);
    this.employeesForm = this.createEmployeeForm();
  }

  // Create form group for employee details
  createEmployeeForm(): UntypedFormGroup {
    return this.fb.group({
      id: [this.employees.id],
      img: [this.employees.img],
      name: [this.employees.name, [Validators.required]],
      email: [this.employees.email, [Validators.required, Validators.email]],
      birthDate: [
        formatDate(this.employees.birthDate, 'yyyy-MM-dd', 'en'),
        [Validators.required],
      ],
      role: [this.employees.role, [Validators.required]],
      mobile: [this.employees.mobile, [Validators.required]],
      department: [this.employees.department, [Validators.required]],
      degree: [this.employees.degree],
      gender: [this.employees.gender],
      address: [this.employees.address],
      joiningDate: [this.employees.joiningDate],
      salary: [this.employees.salary, [Validators.required]],
      lastPromotionDate: [this.employees.lastPromotionDate],
      employeeStatus: [this.employees.employeeStatus],
      workLocation: [this.employees.workLocation],
    });
  }

  // Dynamic error message retrieval
  getErrorMessage(controlName: string): string {
    const control = this.employeesForm.get(controlName);
    if (control?.hasError('required')) {
      return 'This field is required';
    }
    if (control?.hasError('email')) {
      return 'Not a valid email';
    }
    return ''; // Return empty if no errors
  }

  // Submit form data
  submit() {
    if (this.employeesForm.valid) {
      const employeeData = this.employeesForm.getRawValue();
      if (this.action === 'edit') {
        this.employeesService.updateEmployee(employeeData).subscribe({
          next: (response) => {
            this.dialogRef.close(response);
          },
          error: (error) => {
            console.error('Update Error:', error);
            // Optionally show an error message to the user
          },
        });
      } else {
        this.employeesService.addEmployee(employeeData).subscribe({
          next: (response) => {
            this.dialogRef.close(response);
          },
          error: (error) => {
            console.error('Add Error:', error);
            // Optionally show an error message to the user
          },
        });
      }
    }
  }

  // Close dialog without action
  onNoClick(): void {
    this.dialogRef.close();
  }
}
