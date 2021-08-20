import { Component, OnInit, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'logged-in',
  templateUrl: './logged-in.component.html',
  styleUrls: ['./logged-in.component.css']
})

export class LoggedInComponent implements OnInit {

  @Output() myEvent = new EventEmitter<boolean>();
  name: string;

  toggleLoginStatus(_isLogIn: any) {
      this.myEvent.emit(_isLogIn);
  }

  constructor() { }

  ngOnInit(): void {
    this.name = localStorage.getItem("userName");
  }

}
