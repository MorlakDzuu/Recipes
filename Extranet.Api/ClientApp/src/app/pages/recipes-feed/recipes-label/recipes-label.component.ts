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
  public pageNumber: number = 2;
  public canLoadMore: boolean;

  constructor(private recipeService: RecipeService) { }

  ngOnInit() {
    this.recipeService.getRecipeFeed(this.pageNumber).subscribe(val => this.cards = val);
    this.canLoadMore = true
  }

  addMore() {
    this.recipeService.getRecipeFeed(this.pageNumber).subscribe(val => {
      if (val.length != 0) {
        val.forEach(card => this.cards.push(card));
        this.pageNumber++;
      } else {
        this.canLoadMore = false;
      }
    });
  }

}
