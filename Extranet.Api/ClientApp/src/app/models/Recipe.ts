import { Ingredient } from "./Ingredient";
import { Stage } from "./stage";

export interface Recipe {
  id: number;
  title: string;
  description: string;
  author: string;
  likesCount: number;
  favoritesCount: number;
  cookingDuration: number;
  portionsCount: number;
  isLiked: boolean;
  isFavorite: boolean;
  isMyRecipe: boolean;
  tags: string[];
  photoUrl: string;
  ingredients: Ingredient[];
  stages: Stage[];
}
