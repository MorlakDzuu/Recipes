import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { RecipeCard } from '../models/RecipeCard';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RecipeService {

  private recipeOdDayUrl: string = "recipe/feed/recipeOfDay";

  constructor(private http: HttpClient) { }

  getRecipeOfDay(): Observable<RecipeCard> {
    return this.http.get<RecipeCard>(this.recipeOdDayUrl);
  }
}
