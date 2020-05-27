import {Component, OnInit} from '@angular/core';
import {TemperatureApiService} from './temperature-api.service';
import {TemperatureDevice} from './models/temperature-device';
import {map} from 'rxjs/operators';
import * as moment from 'moment';

@Component({
  selector: 'app-temperature',
  templateUrl: './temperature.component.html',
  styleUrls: ['./temperature.component.scss']
})
export class TemperatureComponent implements OnInit {

  devices: TemperatureDevice[];

  constructor(private temperatureApiService: TemperatureApiService) {
  }

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.temperatureApiService.GetTemperatureDevices().subscribe(value => {
      this.devices = value;
    });
  }

  getFormattedTime(rawTimestamp: string): string{
    return moment().diff(moment(rawTimestamp), 'seconds').toString() + ' seconds ago.';
  }

}
