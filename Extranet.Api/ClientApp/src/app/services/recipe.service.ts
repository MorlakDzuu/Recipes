import { Injectable } from '@angular/core';
import { HttpClient,  HttpParams } from '@angular/common/http';
import { RecipeCard } from '../models/RecipeCard';
import { Observable } from 'rxjs';
import { Recipe } from '../models/Recipe';
import { RecipeAdd } from '../models/recipe-add';

@Injectable({
  providedIn: 'root'
})
export class RecipeService {

  private basePath: string = "recipe/";

  constructor(private http: HttpClient) { }

  addNewRecipe(recipe: RecipeAdd): Observable<any> {
    var path = this.basePath + "add";
    return this.http.post(path, recipe);
  }

  editRecipe(recipe: RecipeAdd, recipeId: number): Observable<any> {
    var path = this.basePath + "edit/" + recipeId;
    return this.http.post(path, recipe);
  }

  deleteRecipe(recipeId: number): Observable<any> {
    var path = this.basePath + "delete/" + recipeId;
    return this.http.get(path);
  }

  getRecipesFeedBySearchString(pageNumber: number, searchString: string): Observable<RecipeCard[]> {
    var path: string = this.basePath + "feed/search/" + pageNumber;
    let params = new HttpParams().set("search", searchString);

    return this.http.get<RecipeCard[]>(path, { params: params });
  }

  getFavorites(pageNumber: number): Observable<RecipeCard[]> {
    var path: string = this.basePath + "feed/favorite/" + pageNumber;
    return this.http.get<RecipeCard[]>(path);
  }

  getRecipeOfDay(): Observable<RecipeCard> {
    var path: string = this.basePath + "feed/recipeOfDay";
    return this.http.get<RecipeCard>(path);
  }

  getRecipeFeed(pageNumber: number): Observable<RecipeCard[]> {
    var path: string = this.basePath + "feed/" + pageNumber;
    return this.http.get<RecipeCard[]>(path);
  }

  getRecipeById(recipeId: number): Observable<Recipe> {
    var path: string = this.basePath + "get/" + recipeId;
    return this.http.get<Recipe>(path);
  }

  async addLikeToRecipe(recipeId: number) {
    var path: string = this.basePath + "like/add/" + recipeId;
    this.http.get(path, { observe: 'response' }).subscribe(data => console.log(data));
  }

  async addFavoriteToRecipe(recipeId: number) {
    var path: string = this.basePath + "favorite/add/" + recipeId;
    this.http.get(path, { observe: 'response' }).subscribe(data => console.log(data));
  }

  async deleteLikeToRecipe(recipeId: number) {
    var path: string = this.basePath + "like/delete/" + recipeId;
    this.http.get(path, { observe: 'response' }).subscribe(data => console.log(data));
  }

  async deleteFavoriteToRecipe(recipeId: number) {
    var path: string = this.basePath + "favorite/delete/" + recipeId;
    this.http.get(path, { observe: 'response' }).subscribe(data => console.log(data));
  }
}
