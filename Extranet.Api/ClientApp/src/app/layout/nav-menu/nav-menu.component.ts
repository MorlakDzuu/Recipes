import { Component, OnInit } from '@angular/core';
import { UserLogin } from '../../models/user-login';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {

  isLogIn: boolean = false;

  constructor(private userService: UserService) {}

  async toggleLoginStatus(_isLogIn: any) {
    var userLogin = new UserLogin();
    userLogin.login = "danil228";
    userLogin.password = "123456";

    if (!_isLogIn) {
      this.userService.userLogin(userLogin).subscribe(data => {
        localStorage.setItem("userName", data.name);
        localStorage.setItem("token", data.token);
        this.isLogIn = !_isLogIn;
      });
    } else {
      this.userService.logout().subscribe(data => {
        if (data.status == 200) {
          localStorage.removeItem("userName");
          localStorage.removeItem("token");
          this.isLogIn = !_isLogIn;
        }
      });
    }
  }

  ngOnInit(): void {
    console.log(localStorage.getItem("userName"));
    console.log(this.userService.isLoggedIn());
    if (this.userService.isLoggedIn()) {
      this.isLogIn = true;
    } else {
      this.isLogIn = false;
    }
  }

}
