import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { RecipeCard } from '../models/RecipeCard';
import { Observable } from 'rxjs';
import { Recipe } from '../models/Recipe';

@Injectable({
  providedIn: 'root'
})
export class RecipeService {

  private recipeOdDayUrl: string = "recipe/feed/recipeOfDay";
  private recipeFeedUrl: string = "recipe/feed/";
  private recipePageUrl: string = "recipe/get/";

  private recipeAddLikeUrl: string = "recipe/like/add/";
  private recipeAddFavoriteUrl: string = "recipe/favorite/add/";
  private recipeDeleteLikeUrl: string = "recipe/like/delete/";
  private recipeDeleteFavoriteUrl: string = "recipe/favorite/delete/";

  constructor(private http: HttpClient) { }

  getRecipeOfDay(): Observable<RecipeCard> {
    return this.http.get<RecipeCard>(this.recipeOdDayUrl);
  }

  getRecipeFeed(pageNumber: number): Observable<RecipeCard[]> {
    return this.http.get<RecipeCard[]>(this.recipeFeedUrl + pageNumber);
  }

  getRecipeById(recipeId: number): Observable<Recipe> {
    return this.http.get<Recipe>(this.recipePageUrl + recipeId);
  }

  async addLikeToRecipe(recipeId: number) {
    this.http.get(this.recipeAddLikeUrl + recipeId, { observe: 'response' }).subscribe(data => console.log(data));
  }

  async addFavoriteToRecipe(recipeId: number) {
    this.http.get(this.recipeAddFavoriteUrl + recipeId, { observe: 'response' }).subscribe(data => console.log(data));
  }

  async deleteLikeToRecipe(recipeId: number) {
    this.http.get(this.recipeDeleteLikeUrl + recipeId, { observe: 'response' }).subscribe(data => console.log(data));
  }

  async deleteFavoriteToRecipe(recipeId: number) {
    this.http.get(this.recipeDeleteFavoriteUrl + recipeId, { observe: 'response' }).subscribe(data => console.log(data));
  }
}
