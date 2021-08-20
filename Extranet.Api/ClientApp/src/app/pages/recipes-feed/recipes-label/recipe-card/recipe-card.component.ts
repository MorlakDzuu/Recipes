import { Component, Input, OnInit } from '@angular/core';
import { RecipeCard } from '../../../../models/RecipeCard';
import { RecipeService } from '../../../../services/recipe.service';
import { UserService } from '../../../../services/user.service';

@Component({
  selector: 'app-recipe-card',
  templateUrl: './recipe-card.component.html',
  styleUrls: ['./recipe-card.component.css']
})
export class RecipeCardComponent implements OnInit {

  @Input() card: RecipeCard;

  isFavourite: boolean;
  isLiked: boolean;

  constructor(private userService: UserService, private recipeService: RecipeService) { }

  ngOnInit() {
    this.isFavourite = this.card.isFavorite;
    this.isLiked = this.card.isLiked;
  }

  selectFavourite() {
    if (this.userService.isLoggedIn()) {
      if (this.isFavourite) {
        this.recipeService.deleteFavoriteToRecipe(this.card.id).then(() => {
          this.isFavourite = false;
          this.card.favoritesCount--;
        });
      } else {
        this.recipeService.addFavoriteToRecipe(this.card.id).then(() => {
          this.isFavourite = true;
          this.card.favoritesCount++;
        });
      }
    }
  }

  selectLiked() {
    if (this.userService.isLoggedIn()) {
      if (this.isLiked) {
        this.recipeService.deleteLikeToRecipe(this.card.id).then(() => {
          this.isLiked = false;
          this.card.likesCount--;
        });
      } else {
        this.recipeService.addLikeToRecipe(this.card.id).then(() => {
          this.isLiked = true;
          this.card.likesCount++;
        });
      }
    }
  }
}
