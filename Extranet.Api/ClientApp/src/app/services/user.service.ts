import { HttpResponse } from '@angular/common/http';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { UserLogin } from '../models/user-login';
import { UserRegister } from '../models/user-register';
import { UserSettings } from '../models/user-settings';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private basePath: string = "user/";

  constructor(private http: HttpClient) { }

  public userLogin(user: UserLogin): Observable<UserSettings> {
    var path: string = this.basePath + "login";
    return this.http.post<UserSettings>(path, user);
  }

  public userRegister(user: UserRegister) {
    var path: string = this.basePath + "register";
    try {
      this.http.post(path, user);
    } catch (error) {
      console.log(error.message);
    }
  }

  public userVerifyToken() {
    var path: string = this.basePath + "verify";
    return this.http.get(path, { observe: 'response' });
  }

  public isLoggedIn(): boolean {
    if (localStorage.getItem("userName") != undefined)
      return true;
    return false;
  }
}
