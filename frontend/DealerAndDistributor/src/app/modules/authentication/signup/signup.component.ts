import { AuthService } from '../../../core/service/auth.service';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { FormBuilder, FormGroup, FormControl, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { CommonModule } from '@angular/common';
import { UpsertTenantComponent, UpsertTenantFormValue } from '../../../core/shared/components/upsert-tenant/upsert-tenant.component';
@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss'],
  imports: [
    CommonModule, 
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    RouterLink,
    MatButtonModule,
    MatOptionModule,
    MatSelectModule,
    UpsertTenantComponent
  ]
})
export class SignupComponent implements OnInit {
  signupForm!: FormGroup<{
    phoneNumber: FormControl<string>;
    password: FormControl<string>;
    tenantName: FormControl<string>;
    firstName: FormControl<string>;
    lastName: FormControl<string>;
    designationId: FormControl<number>;
  }>;
  submitted = false;
  returnUrl!: string;
  hide = true;
  chide = true;
  designations = [
    { id: 1, name: 'CEO' },
    { id: 2, name: 'Manager' },
    { id: 3, name: 'Sales' },
    { id: 4, name: 'Support' },
    { id: 5, name: 'Other' }
  ];
  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService
  ) { }
  ngOnInit() {
    this.signupForm = this.formBuilder.group({
      phoneNumber: this.formBuilder.control('', { validators: [Validators.required, Validators.pattern(/^\+?[0-9]{10,15}$/)], nonNullable: true }),
      password: this.formBuilder.control('', { validators: [Validators.required, Validators.minLength(8)], nonNullable: true }),
      tenantName: this.formBuilder.control('', { validators: [Validators.required], nonNullable: true }),
      firstName: this.formBuilder.control('', { validators: [Validators.required], nonNullable: true }),
      lastName: this.formBuilder.control('', { validators: [Validators.required], nonNullable: true }),
      designationId: this.formBuilder.control(0, { validators: [Validators.required], nonNullable: true }),
    });
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  onSubmit() {
    this.submitted = true;
    if (this.signupForm.invalid) {
      return;
    }
    const request = this.signupForm.getRawValue();
    this.authService.register(request).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.router.navigate(['/admin/dashboard/main']);
        } else {
          // handle error, e.g. show message
        }
      },
      error: () => {
        // handle error, e.g. show message
      }
    });
  }

  // Handler for the shared upsert-tenant form submission
  onSharedSubmit(val: UpsertTenantFormValue) {
    const request = {
      phoneNumber: val.phoneNumber,
      password: val.password ?? '',
      tenantName: val.tenantName,
      firstName: val.firstName,
      lastName: val.lastName,
      designationId: val.designationId ?? 0,
    };
    this.authService.register(request).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.router.navigate(['/admin/dashboard/main']);
        } else {
          // TODO: show error toast/message
        }
      },
      error: () => {
        // TODO: show error toast/message
      }
    });
  }
}
