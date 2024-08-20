import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { filter } from 'rxjs/operators';
import { AuthService } from '../../../core/service/auth.service';
import { routes } from '../../../core/helpers/routes/routes';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  public routes = routes;
  public show_password = true;
  loginForm !: FormGroup;
  returnUrl !: string;
  isBeingLoggedIn: boolean = false;
  constructor(@Inject(AuthService) private authService: AuthService, private router: Router, private activatedRoute: ActivatedRoute) {
    this.initializeForm();
  }

  ngOnInit(): void {
    this.returnUrl = this.activatedRoute.snapshot.queryParams['returnUrl'] || routes.dashboard;
    if (this.authService.isAuthenticated) {
      this.router.navigateByUrl(this.returnUrl)
    }

  }

  initializeForm() {
    this.loginForm = new FormGroup({
      email: new FormControl('Super@g1.com', [Validators.required, Validators.email]),
      password: new FormControl('Super@g1.com', Validators.required),
      // tenant: new FormControl('root', Validators.required)
    });
  }

  onSubmit() {
    this.isBeingLoggedIn = true;
    this.loginForm.disable()
    this.authService.login(this.loginForm.value)
      .subscribe(() => this.router.navigateByUrl(this.returnUrl),
        (error: any) => { console.log(error); this.loginForm.enable(); }).add(() => this.isBeingLoggedIn = false);
  }
}
