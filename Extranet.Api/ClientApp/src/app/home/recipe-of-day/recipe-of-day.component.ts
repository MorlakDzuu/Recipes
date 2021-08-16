import { Component, Input, OnInit } from '@angular/core';
import { RecipeCardDto } from '../../../entities/RecipeCardDto';
import { RecipeService } from '../../recipe-service/recipe.service';

@Component({
  selector: 'recipe-of-day',
  templateUrl: './recipe-of-day.component.html',
  styleUrls: ['./recipe-of-day.component.css']
})

export class RecipeOfDayComponent implements OnInit {
  public card!: RecipeCardDto;
  constructor(private recipeService: RecipeService) {
  }

  ngOnInit(): void {
    this.recipeService.getRecipeOfDay().subscribe(val => {
      this.card = val;
      console.log(val);
    });
  }

}
