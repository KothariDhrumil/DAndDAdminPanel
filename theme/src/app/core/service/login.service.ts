import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '@core/models/interface';
import { of } from 'rxjs';
import { LocalStorageService } from '@shared/services';
import { JWT } from './JWT';
const jwt = new JWT();

@Injectable({
  providedIn: 'root',
})
export class LoginService {
  private users: User[] = [
    {
      id: 1,
      username: 'admin',
      password: 'admin@123',
      name: 'Sarah Smith',
      email: 'admin@company.org',
      roles: [
        {
          name: 'ADMIN',
          priority: 1,
        },
      ],
      permissions: ['canAdd', 'canDelete', 'canEdit', 'canRead'],
      avatar: 'admin.jpg',
    },
    {
      id: 2,
      username: 'employee',
      password: 'employee@123',
      name: 'Ashton Cox',
      email: 'employee@company.org',
      roles: [
        {
          name: 'EMPLOYEE',
          priority: 2,
        },
      ],
      permissions: ['canAdd', 'canEdit', 'canRead'],
      avatar: 'employee.jpg',
      refresh_token: true,
    },
    {
      id: 3,
      username: 'client',
      password: 'client@123',
      name: 'Cara Stevens',
      email: 'client@company.org',
      roles: [
        {
          name: 'CLIENT',
          priority: 3,
        },
      ],
      permissions: ['canRead'],
      avatar: 'client.jpg',
      refresh_token: true,
    },
  ];
  constructor(protected http: HttpClient, private store: LocalStorageService) {}

  login(username: string, password: string, rememberMe = false) {
    // Simulate a login API call
    const user = this.users.find(
      (u) => u['username'] === username && u['password'] === password
    );
    if (!user) {
      return of({ status: 401, body: {} });
    }

    if (user['password'] !== password) {
      const result = {
        status: 422,
        error: {
          errors: { password: ['The provided password is incorrect.'] },
        },
      };
      return of(Object.assign(result));
    }

    const currentUser = Object.assign({}, user);
    delete currentUser['password'];

    if (user) {
      const userResponse = {
        user: currentUser,
        token: jwt.generate(currentUser),
        status: 200,
      };

      return of(userResponse);
    } else {
      return of({ error: 'Invalid credentials' });
    }
  }

  refresh() {
    const user = Object.assign({}, this.store.get('currentUser'));

    const result = user
      ? { status: 200, body: jwt.generate(user) }
      : { status: 401, body: {} };

    return of(result);
  }

  logout() {
    this.store.clear();
    return of({ success: false });
  }

  user() {
    return this.http.get<User>('/user');
  }
}
