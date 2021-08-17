import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { RecipeOfDayComponent } from './pages/home/recipe-of-day/recipe-of-day.component';
import { SearchRecipesComponent } from './pages/home/search-recipes/search-recipes.component';
import { SortingByTagsComponent } from './pages/home/sorting-by-tags/sorting-by-tags.component';
import { FooterComponent } from './layout/footer/footer.component';
import { HomeComponent } from './pages/home/home.component';
import { NavMenuComponent } from './layout/nav-menu/nav-menu.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatMenuModule } from '@angular/material/menu';
import { LoggedInComponent } from './layout/nav-menu/logged-in/logged-in.component';
import { LoginButtonComponent } from './layout/nav-menu/login-button/login-button.component';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    FooterComponent,
    SortingByTagsComponent,
    RecipeOfDayComponent,
    SearchRecipesComponent,
    LoggedInComponent,
    LoginButtonComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    BrowserAnimationsModule,
    MatMenuModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' }
    ]),
    BrowserAnimationsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
