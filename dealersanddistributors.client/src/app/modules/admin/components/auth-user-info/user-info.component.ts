import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { AuthUserInfo } from '../../models/authuserinfo.model';
import { LoggedInUserApiService } from '../../../../core/service/logged-in-user.services';

@Component({
  selector: 'app-auth-user-info',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './auth-user-info.component.html',
  styleUrl: './auth-user-info.component.scss'
})
export class AuthUserInfoComponent implements OnInit {
  userInfo !: AuthUserInfo;
  permissions !: string[];
  /**
   *
   */
  constructor(private loggedInUserApiService: LoggedInUserApiService) {


  }

  ngOnInit(): void {
    this.loadUserInfo();
    this.loadPermissions();
  }

  loadUserInfo(): void {
    this.loggedInUserApiService.getAuthUserInfo().subscribe((data: AuthUserInfo) => {
      this.userInfo = data;
    });
  }

  loadPermissions(): void {
    this.loggedInUserApiService.getPermissions().subscribe((data: string[]) => {
      this.permissions = data;
    });
  }
}
