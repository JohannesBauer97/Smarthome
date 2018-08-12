import {Component, Input, OnInit} from '@angular/core';
import {LedStripe} from '../../models/ledStripe';
import {LedService} from '../../services/led.service';

@Component({
  selector: 'app-led-card',
  templateUrl: './led-card.component.html',
  styleUrls: ['./led-card.component.css']
})
export class LedCardComponent implements OnInit {

  @Input() public led: LedStripe;
  public selectedColor: string;

  constructor(private ledService: LedService) {
  }

  ngOnInit() {
    console.log(this.led);
    this.selectedColor = this.led.color;
  }

  public setColor(hex: string): void {
    this.ledService.setStripeColor(this.led.name, hex);
    this.led.color = hex;
  }

  public refresh(): void {
    this.ledService.refreshStripes();
  }

}
