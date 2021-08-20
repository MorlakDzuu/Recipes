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
import { RecipesFeedComponent } from './pages/recipes-feed/recipes-feed.component';
import { RecipesLabelComponent } from './pages/recipes-feed/recipes-label/recipes-label.component';
import { RecipeCardComponent } from './pages/recipes-feed/recipes-label/recipe-card/recipe-card.component';
import { SearchComponent } from './pages/recipes-feed/search/search.component';
import { RecipePageComponent } from './layout/recipe-page/recipe-page.component';
import { StepComponent } from './layout/recipe-page/step/step.component';
import { RecipeAddButtonComponent } from './pages/recipes-feed/recipe-add-button/recipe-add-button.component';
import { RecipesSortingByTagsComponent } from './pages/recipes-feed/recipes-sorting-by-tags/recipes-sorting-by-tags.component';


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
    LoginButtonComponent,
    RecipesFeedComponent,
    RecipesLabelComponent,
    RecipeCardComponent,
    SearchComponent,
    RecipePageComponent,
    StepComponent,
    RecipeAddButtonComponent,
    RecipesSortingByTagsComponent
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
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'recipe', component: RecipesFeedComponent },
      { path: 'recipe/:id', component: RecipePageComponent }
    ]),
    BrowserAnimationsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
