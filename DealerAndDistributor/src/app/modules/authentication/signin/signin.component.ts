import { Component, ChangeDetectionStrategy, OnInit, computed, signal, inject } from '@angular/core';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import {
  FormBuilder,
  FormGroup,
  Validators,
  FormsModule,
  ReactiveFormsModule,
  FormControl,
} from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { AuthService, Role } from '../../../core';
import { UnsubscribeOnDestroyAdapter } from '../../../core/shared';
import { StartupService } from '../../../core/service/startup.service';
import { DASHBOARD_ROUTE } from '../../../core/helpers/routes/app-routes';

interface AuthForm {
  email: string;
  password: string;
}

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.scss'],
  imports: [
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
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private authService = inject(AuthService);
  private activatedRoute = inject(ActivatedRoute);
  private startupService = inject(StartupService);
  returnUrl !: string;

  authForm!: FormGroup<{
    email: FormControl<string>;
    password: FormControl<string>;
  }>;
  submitted = signal(false);
  loading = signal(false);
  error = signal('');
  hide = signal(true);

  // Use computed for template binding
  readonly isLoading = computed(() => this.loading());
  readonly isError = computed(() => this.error());
  readonly isHide = computed(() => this.hide());

  ngOnInit() {
    this.returnUrl = this.activatedRoute.snapshot.queryParams['returnUrl'] || DASHBOARD_ROUTE;
    if (this.authService.isAuthenticated) {
      this.router.navigateByUrl(this.returnUrl)
    }

    this.authForm = this.formBuilder.group({
      email: this.formBuilder.control('admin', { validators: [Validators.required], nonNullable: true }),
      password: this.formBuilder.control('admin@123', { validators: [Validators.required], nonNullable: true }),
    });
  }

  get f() {
    return this.authForm.controls;
  }

  onSubmit() {
    this.submitted.set(true);
    this.loading.set(true);
    this.error.set('');
    if (this.authForm.invalid) {
      this.error.set('Username and Password not valid !');
      this.loading.set(false);
      return;
    }
    const { email, password } = this.authForm.getRawValue();
    this.authService
      .login(email, password, false)
      .subscribe({
        next: (response) => {
          this.submitted.set(false);
          this.loading.set(false);
          if (response) {
            this.startupService.load().subscribe({
              next: () => {
                this.router.navigateByUrl(this.returnUrl);
              },
              error: (error: unknown) => {
                this.error.set('Failed to load roles/permissions');
              }
            });
          } else {
            this.error.set('Login failed');
          }
        },
        error: (error: unknown) => {
          this.error.set(String(error));
          this.submitted.set(false);
          this.loading.set(false);
        },
      });
  }
}
