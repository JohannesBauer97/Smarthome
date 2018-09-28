import {Component, OnInit} from '@angular/core';
import {LedStripe} from '../models/ledStripe';
import {LedService} from '../services/led.service';

@Component({
  selector: 'app-led',
  templateUrl: './led-board.component.html',
  styleUrls: ['./led-board.component.css']
})
export class LedBoardComponent implements OnInit {
  public stripes: LedStripe[] = [];

  constructor(private ledService: LedService) {
    ledService.stripes.subscribe((stripes: LedStripe[]) => {
      this.stripes = stripes;
    });
  }

  ngOnInit() {
  }
}
