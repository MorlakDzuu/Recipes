import { Ingredient } from "./Ingredient";
import { Step } from "./Step";

export interface Recipe {
  id: number;
  name: string;
  description: string;
  author: string;
  likes: number;
  favourites: number;
  timeMinutes: number;
  numberOfPersons: number;
  tags: string[];
  photo: string;
  ingredients: Ingredient[];
  steps: Step[];
}
