import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { RecipeAdd } from '../../models/recipe-add';
import { MatChipInputEvent } from '@angular/material/chips';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { FileService } from '../../services/file.service';
import { RecipeService } from '../../services/recipe.service';

@Component({
  selector: 'app-recipe-add-page',
  templateUrl: './recipe-add-page.component.html',
  styleUrls: ['./recipe-add-page.component.css']
})
export class RecipeAddPageComponent implements OnInit {

  public recipe: RecipeAdd;
  visible = true;
  selectable = true;
  removable = true;
  separatorKeysCodes: number[] = [ENTER, COMMA];
  file: File = null;
  isImageLoaded: boolean = false;

  constructor(private fileService: FileService, private recipeService: RecipeService, private location: Location) { }

  goBack() {
    this.location.back();
  }

  onChange(event) {
    this.file = event.target.files[0];
    this.fileService.upload(this.file).subscribe(val => {
      this.recipe.photoUrl = val;
      this.isImageLoaded = true;
    });
  }

  public send() {
    console.log(this.recipe);
    this.recipeService.addNewRecipe(this.recipe).subscribe(val => console.log(val));
  }

  public addTitle(): void {
    this.recipe.ingredients.push({
      title: "",
      description: ""
    });
  }

  public deleteTitle(block: number): void {
    this.recipe.ingredients.splice(block, 1);
  }

  public addStage(): void {
    this.recipe.stages.push({
      serialNumber: this.recipe.stages.length,
      description: ""
    });
  }

  public deleteStage(stage: number): void {
    this.recipe.stages.splice(stage, 1);
  }

  addTag(event: MatChipInputEvent): void {
    const tag: string = (event.value || '').trim();
    if (tag) {
      this.recipe.tags.push(tag);
    }
    event.input.value = '';
  }

  removeTag(tag: string): void {
    const index = this.recipe.tags.indexOf(tag);
    if (index >= 0) {
      this.recipe.tags.splice(index, 1);
    }
  }


  ngOnInit(): void {
    this.recipe = {
      title: "",
      description: "",
      cookingDuration: 0,
      portionsCount: 0,
      tags: [],
      photoUrl: "",
      ingredients: [
        {
          title: "",
          description: ""
        }
      ],
      stages: [
        {
          serialNumber: 1,
          description: ""
        }
      ]
    }
  }
}
