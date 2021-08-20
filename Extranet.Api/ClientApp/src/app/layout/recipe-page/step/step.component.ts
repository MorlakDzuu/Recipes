import { Component, Input, OnInit } from '@angular/core';
import { Step } from '../../../models/Step';

@Component({
  selector: 'app-step',
  templateUrl: './step.component.html',
  styleUrls: ['./step.component.css']
})
export class StepComponent implements OnInit {

  @Input() step: Step;

  constructor() { }

  ngOnInit() {
  }

}
