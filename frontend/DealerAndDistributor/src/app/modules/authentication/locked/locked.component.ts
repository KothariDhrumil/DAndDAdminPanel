import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';

import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { AuthService, Role } from '../../../core';
import { LocalStorageService } from '../../../core/shared/services';


@Component({
  selector: 'app-locked',
  templateUrl: './locked.component.html',
  styleUrls: ['./locked.component.scss'],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatButtonModule,
    RouterLink,
  ],
})
export class LockedComponent implements OnInit {
  authForm!: UntypedFormGroup;
  submitted = false;
  userImg!: string;
  userFullName!: string;
  hide = true;
  constructor(
    private formBuilder: UntypedFormBuilder,
    private router: Router,
    private authService: AuthService,
    private store: LocalStorageService
  ) {}
  ngOnInit() {
    this.authForm = this.formBuilder.group({
      password: ['', Validators.required],
    });
    this.userImg = this.authService.currentUserValue.avatar || '';
    this.userFullName = this.authService.currentUserValue.name || '';
  }
  get f() {
    return this.authForm.controls;
  }
  onSubmit() {
    this.submitted = true;
    // stop here if form is invalid
    if (this.authForm.invalid) {
      return;
    } else {
      const role = this.store.get('currentUser').role?.[0]?.name;
      if (role === Role.Admin) {
        this.router.navigate(['/admin/dashboard/main']);
      } else if (role === Role.Employee) {
        this.router.navigate(['/employee/dashboard']);
      } else if (role === Role.Client) {
        this.router.navigate(['/client/dashboard']);
      } else {
        this.router.navigate(['/authentication/signin']);
      }
    }
  }
}
