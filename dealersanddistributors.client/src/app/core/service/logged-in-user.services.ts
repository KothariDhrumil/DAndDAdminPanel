
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { AuthUserInfo } from "../../modules/admin/models/authuserinfo.model";

@Injectable()
export class LoggedInUserApiService {

  baseUrl = environment.apiUrl + 'loggedinuser/';

  constructor(private http: HttpClient) {
  }

  getAuthUserInfo() {
    return this.http.get<AuthUserInfo>(this.baseUrl + 'authuserinfo')
  }

  getPermissions() {
    return this.http.get<string[]>(this.baseUrl + 'permissions');
  }
}
