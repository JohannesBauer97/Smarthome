import {Component, Input, OnInit} from '@angular/core';
import {LED} from '../led';
import {LedService} from '../led.service';

@Component({
  selector: 'app-led-card',
  templateUrl: './led-card.component.html',
  styleUrls: ['./led-card.component.css']
})
export class LedCardComponent implements OnInit {

  @Input() led: LED;
  public _red: number;
  public _green: number;
  public _blue: number;
  private _hexVal: string;

  constructor(private _LedService: LedService) {
  }

  ngOnInit() {
    this._hexVal = this.led.color;
    const rgb = this._LedService.hexToRgb(this.led.color);
    this._red = rgb.r;
    this._green = rgb.g;
    this._blue = rgb.b;
  }

  private _sendColor(): void {
    this._hexVal = this._LedService.rgbToHex(this._red, this._green, this._blue);
    this._LedService.setStripeColor(this.led.name, this._hexVal);
  }

}
