import {Component, OnInit} from '@angular/core';
import {LedService} from '../led.service';
import {LED} from '../led';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  private _LEDs = new Array<LED>();

  constructor(private _LedService: LedService) {
  }

  ngOnInit() {
    this._LedService.LEDs.subscribe((dd: LED[]) => {
      this._LEDs = dd;
    });
  }

  public refresh() {
    this._LedService.refresh();
  }

}
