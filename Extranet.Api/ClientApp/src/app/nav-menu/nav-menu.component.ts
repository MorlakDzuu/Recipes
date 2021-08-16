import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {

  isLogIn: boolean = false;

  toggleLoginStatus(_isLogIn: any) {
    this.isLogIn = !_isLogIn;
  }

  constructor() { }

  ngOnInit(): void {
  }

}
