import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { RecipeCardDto } from '../../entities/RecipeCardDto';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RecipeService {

  private recipeOdDayUrl = "recipe/feed/recipeOfDay";

  constructor(private http: HttpClient) { }

  getRecipeOfDay(): Observable<RecipeCardDto> {
    return this.http.get<RecipeCardDto>(this.recipeOdDayUrl);
  }
}
