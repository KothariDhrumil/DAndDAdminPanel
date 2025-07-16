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

interface AuthForm {
  username: string;
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

  authForm!: FormGroup<{
    username: FormControl<string>;
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
    this.authForm = this.formBuilder.group({
      username: this.formBuilder.control('admin', { validators: [Validators.required], nonNullable: true }),
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
    const { username, password } = this.authForm.getRawValue();
    this.authService
      .login(username, password, false)
      .subscribe({
        next: (response: { user: { roles: { name: string }[] } }) => {
          const role = response.user.roles[0];
          this.loading.set(false);
          if (role.name === Role.Admin) {
            this.router.navigate(['/admin/dashboard/main']);
          } else if (role.name === Role.Employee) {
            this.router.navigate(['/employee/dashboard']);
          } else if (role.name === Role.Client) {
            this.router.navigate(['/client/dashboard']);
          } else {
            this.router.navigate(['/authentication/signin']);
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
