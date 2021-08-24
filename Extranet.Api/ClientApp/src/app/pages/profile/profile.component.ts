import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { RecipeCard } from '../../models/RecipeCard';
import { User } from '../../models/user';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  public recipes: RecipeCard[];
  public author: User;

  public recipesCount: number;
  public likesCount: number;
  public favoritesCount: number;

  isVisible: boolean = false;
  isEdit: boolean = false;

  constructor(private userService: UserService, private location: Location) { }

  ngOnInit(): void {
    this.userService.getUserInfo().subscribe(data => {
      console.log(data);
      this.author = {
        name: data.name,
        login: data.login,
        password: data.password,
        description: data.description
      };
      this.recipesCount = data.recipesCount;
      this.likesCount = data.likesCount;
      this.favoritesCount = data.favoritesCount;
      this.recipes = data.recipes;
    });
  }

  goBack() {
    this.location.back();
  }

  saveEditing() {
    let name: any = document.getElementById("name-input");
    let login: any = document.getElementById("login-input");
    let password: any = document.getElementById("password-input");
    let description: any = document.getElementById("description-input");

    this.author.name = name.value;
    this.author.login = login.value;
    this.author.description = description.value;
    this.author.password = password.value;

    this.userService.editProfile(this.author).subscribe(data => {
      console.log(this.author);
      if (data.status == 200) {
        this.showEditing();
      }
    });
  }

  cancelEditing() {
    let name: any = document.getElementById("name-input");
    let login: any = document.getElementById("login-input");
    let password: any = document.getElementById("password-input");
    let description: any = document.getElementById("description-input");

    name.value = this.author.name;
    login.value = this.author.login;
    password.value = this.author.password;
    description.value = this.author.description;
    this.showEditing();
  }

  showEditing() {
    let name: any = document.getElementById("name-input");
    let login: any = document.getElementById("login-input");
    let password: any = document.getElementById("password-input");
    this.isEdit = !this.isEdit;
    if (this.isEdit) {
      name.disabled = false;
      login.disabled = false;
      password.disabled = false;
    }
    else {
      name.disabled = true;
      login.disabled = true;
      password.disabled = true;
    }
  }

  showHidePassword() {
    this.isVisible = !this.isVisible;
    let password: any = document.getElementById("password-input");
    if (this.isVisible) {
      password.type = "text";
    }
    else {
      password.type = "password";
    }
  }
}
