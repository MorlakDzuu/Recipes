import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Recipe } from '../../models/Recipe';
import { RecipeService } from '../../services/recipe.service';

@Component({
  selector: 'app-recipe-page',
  templateUrl: './recipe-page.component.html',
  styleUrls: ['./recipe-page.component.css']
})
export class RecipePageComponent implements OnInit {

  private recipeId: number;
  recipe: Recipe;

  constructor(private activateRoute: ActivatedRoute, private recipeService: RecipeService) {
    this.recipeId = activateRoute.snapshot.params['id'];
  }

  ngOnInit() {
    this.recipeService.getRecipeById(this.recipeId).subscribe(val => { this.recipe = val; console.log(val) });
  }

}
