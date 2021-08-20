import { Component, OnInit } from '@angular/core';
import { RecipeCard } from '../../../models/RecipeCard';
import { RecipeService } from '../../../services/recipe.service';

@Component({
  selector: 'app-recipes-label',
  templateUrl: './recipes-label.component.html',
  styleUrls: ['./recipes-label.component.css']
})
export class RecipesLabelComponent implements OnInit {

  public cards: RecipeCard[];
  public pageNumber: number = 1;

  constructor(private recipeService: RecipeService) { }

  ngOnInit() {
    this.recipeService.getRecipeFeed(this.pageNumber).subscribe(val => this.cards = val);
  }

  addMore() {
    this.pageNumber++;
    this.recipeService.getRecipeFeed(this.pageNumber).subscribe(val => val.forEach(card => this.cards.push(card)));
  }

}
