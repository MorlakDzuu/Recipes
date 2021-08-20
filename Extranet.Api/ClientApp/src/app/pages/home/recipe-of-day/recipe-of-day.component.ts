import { Component, Input, OnInit } from '@angular/core';
import { RecipeCard } from '../../../models/RecipeCard';
import { RecipeService } from '../../../services/recipe.service';

@Component({
  selector: 'recipe-of-day',
  templateUrl: './recipe-of-day.component.html',
  styleUrls: ['./recipe-of-day.component.css']
})

export class RecipeOfDayComponent implements OnInit {
  public card: RecipeCard;

  constructor(private recipeService: RecipeService) {
  }

  ngOnInit(): void {
    this.recipeService.getRecipeOfDay().subscribe(val => {
      this.card = val;
    });
  }

}
