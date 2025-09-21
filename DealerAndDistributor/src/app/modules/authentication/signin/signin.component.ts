// State for OTP flow

import { Component, ChangeDetectionStrategy, OnInit, computed, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { FormBuilder, FormGroup, FormControl, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from '../../../core';
import { UnsubscribeOnDestroyAdapter } from '../../../core/shared';
import { StartupService } from '../../../core/service/startup.service';
import { DASHBOARD_ROUTE, SUPERADMIN_DASHBOARD_ROUTE, USER_DASHBOARD_ROUTE } from '../../../core/helpers/routes/app-routes';
import { catchError, take, tap } from 'rxjs';
import { SigninRequest } from '../../../core/models/interface/SigninRequest';
import { ApiResponse } from '@core/models/interface/ApiResponse';
import { Token } from '@core/models/interface/Token';


@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.scss'],
  imports: [
    CommonModule,
    RouterLink,
    MatButtonModule,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SigninComponent extends UnsubscribeOnDestroyAdapter implements OnInit {
  private formBuilder = inject(FormBuilder);
  private router = inject(Router);
  private authService = inject(AuthService);
  private activatedRoute = inject(ActivatedRoute);
  private startupService = inject(StartupService);
  returnUrl!: string;

  // Signal for selected login option: 'password' or 'otp'
  loginOption = signal<'password' | 'otp'>('password');

  // Password login form
  passwordForm!: FormGroup<{
    emailOrPhone: FormControl<string>;
    password: FormControl<string>;
  }>;

  // OTP login form (phone input)
  otpForm!: FormGroup<{
    phoneNumber: FormControl<string>;
    otp: FormControl<string>;
  }>;

  submitted = signal(false);
  loading = signal(false);
  error = signal('');
  hide = signal(true);
  otpStep = signal<'input' | 'verify'>('input');

  readonly isLoading = computed(() => this.loading());
  readonly isError = computed(() => this.error());
  readonly isHide = computed(() => this.hide());

  ngOnInit() {
    this.returnUrl = this.activatedRoute.snapshot.queryParams['returnUrl'] || USER_DASHBOARD_ROUTE;
    if (this.authService.isAuthenticated) {
      this.router.navigateByUrl(this.returnUrl);
    }

    this.passwordForm = this.formBuilder.group({
      emailOrPhone: this.formBuilder.control('', {
        validators: [Validators.required],
        nonNullable: true,
      }),
      password: this.formBuilder.control('', {
        validators: [Validators.required, Validators.minLength(6)],
        nonNullable: true,
      }),
    });

    this.otpForm = this.formBuilder.group({
      phoneNumber: this.formBuilder.control('', {
        validators: [Validators.required],
        nonNullable: true,
      }),
      otp: this.formBuilder.control('', {
        validators: [Validators.required, Validators.pattern('^[0-9]{6}$')],
        nonNullable: true,
      }),
    });
  }

  setLoginOption(option: 'password' | 'otp') {
    this.loginOption.set(option);
    this.error.set('');
    this.submitted.set(false);
    this.loading.set(false);
  }

  onSubmit() {
    this.submitted.set(true);
    this.loading.set(true);
    this.error.set('');

    if (this.loginOption() === 'password') {
      if (this.passwordForm.invalid) {
        this.error.set('Please enter valid email/phone and password.');
        this.loading.set(false);
        return;
      }
      const emailOrPhone = this.passwordForm.controls.emailOrPhone.value;
      const password = this.passwordForm.controls.password.value;
      const request: SigninRequest = {
        phoneNumber: this.isPhone(emailOrPhone) ? emailOrPhone : '',
        email: this.isEmail(emailOrPhone) ? emailOrPhone : '',
        password,
        otpEnabled: false,
      };
      this.authService.signin(request).pipe(
        tap((response) => {
          this.loading.set(false);
          this.handleLoginSuccess(response);
        }),
        catchError((err) => {
          this.loading.set(false);
          this.error.set('Login failed');
          return [];
        }),
        take(1)
      ).subscribe();
    } else {
      if (this.otpStep() === 'input') {
        // Step 1: Generate OTP
        if (this.otpForm.controls.phoneNumber.invalid) {
          this.error.set('Please enter a valid phone number.');
          this.loading.set(false);
          return;
        }
        const phoneNumber = this.otpForm.controls.phoneNumber.value;
        this.authService.generateOTP(phoneNumber).pipe(
          tap((response) => {
            this.loading.set(false);
            if (response.isSuccess) {
              this.otpStep.set('verify');
              this.error.set('');
              this.otpForm.controls.otp.setValue(response.data || '');
            } else {
              this.error.set('Failed to generate OTP.');
            }
          }),
          catchError(() => {
            this.loading.set(false);
            this.error.set('Failed to generate OTP.');
            return [];
          }),
          take(1)
        ).subscribe();
      } else if (this.otpStep() === 'verify') {
        // Step 2: Confirm OTP
        if (this.otpForm.controls.otp.invalid) {
          this.error.set('Please enter a valid OTP.');
          this.loading.set(false);
          return;
        }
        const phoneNumber = this.otpForm.controls.phoneNumber.value;
        const code = this.otpForm.controls.otp.value;
        this.authService.confirmOTP(phoneNumber, code).pipe(
          tap((response) => {
            this.handleLoginSuccess(response);
          }),
          catchError(() => {
            this.loading.set(false);
            this.error.set('OTP confirmation failed');
            return [];
          }),
          take(1)
        ).subscribe();
      }
    }
  }

  handleLoginSuccess(response: ApiResponse<Token>) {
    if (response.isSuccess && response.data) {
      this.startupService.load()
        .then(() => {
          if (this.authService.isSuperAdmin) {
            return this.router.navigateByUrl(SUPERADMIN_DASHBOARD_ROUTE);
          }
          return this.router.navigateByUrl(this.returnUrl);
        })
        .catch((error) => {
          console.error('OTP confirmation failed:', error);
          this.error.set('OTP confirmation failed. Please try again.');
          // Optionally clear the auth token if permissions failed to load
          this.authService.logout();
        });
    } else {
      this.error.set(response.error?.description || 'Login failed');
    }
  }

  // Utility: check if input is email
  isEmail(value: string): boolean {
    return /^[^@\s]+@[^@\s]+\.[^@\s]+$/.test(value);
  }

  // Utility: check if input is phone
  isPhone(value: string): boolean {
    return /^\d{10}$/.test(value);
  }
}
