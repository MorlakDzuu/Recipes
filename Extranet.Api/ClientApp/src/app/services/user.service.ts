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

  private userLoginUrl: string = "user/login";
  private userRegisterUrl: string = "user/register";
  private userVerifyUrl: string = "user/verify"

  constructor(private http: HttpClient) { }

  public async userLogin(user: UserLogin) {
    try {
      this.http.post<UserSettings>(this.userLoginUrl, user).subscribe(val => {
        console.log(val);
        localStorage.setItem("userName", val.name);
        localStorage.setItem("token", val.token);
      });

    } catch (error) {
      console.log(error.message);
    }
  }

  public userRegister(user: UserRegister) {
    try {
      this.http.post(this.userRegisterUrl, user);
    } catch (error) {
      console.log(error.message);
    }
  }

  public userVerifyToken() {
    this.http.get(this.userVerifyUrl, { observe: 'response' }).subscribe(data => {
      if (data.status != 500) {
        localStorage.removeItem("userName");
        localStorage.removeItem("token");
      }
    });
  }

  public isLoggedIn(): boolean {
    if (localStorage.getItem("userName") != undefined)
      return true;
    return false;
  }
}
