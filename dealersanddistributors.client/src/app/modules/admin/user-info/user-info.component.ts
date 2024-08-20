import { AuthService } from './../../../core/service/auth.service';
import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-user-info',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-info.component.html',
  styleUrl: './user-info.component.scss'
})
export class UserInfoComponent implements OnInit {
  userInfo: any;
  /**
   *
   */
  constructor(private authService: AuthService) {


  }

  ngOnInit(): void {
    this.loadUserInfo();
  }

  loadUserInfo(): void {
    this.userInfo = 'User Name ' + this.authService.getFullName + ', user id : ' + this.authService.getUserId + ', Email :' + this.authService.getEmail;
  }
}
