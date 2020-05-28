import {Component, OnInit} from '@angular/core';
import {TemperatureApiService} from './temperature-api.service';
import {TemperatureDevice} from './models/temperature-device';
import {HttpErrorResponse} from '@angular/common/http';

@Component({
  selector: 'app-temperature',
  templateUrl: './temperature.component.html',
  styleUrls: ['./temperature.component.scss']
})
export class TemperatureComponent implements OnInit {

  devices: TemperatureDevice[];
  error: HttpErrorResponse;
  editMode: { enabled: boolean, chipId: string } = {enabled: false, chipId: null};

  constructor(private temperatureApiService: TemperatureApiService) {
  }

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.temperatureApiService.GetTemperatureDevices().subscribe(value => {
      this.devices = value;
    }, (error: HttpErrorResponse) => this.error = error);
  }

  startEdit(chipId: string): void {
    this.editMode = {enabled: true, chipId};
  }

  stopEdit(newName: string): void {
    const updatedDevice = {...this.devices.find(x => x.chipId === this.editMode.chipId)};
    updatedDevice.name = newName;
    this.temperatureApiService.UpdateTemperatureDevice(updatedDevice).subscribe(value => {
      if (value) {
        this.devices.find(x => x.chipId === value.chipId).name = value.name;
      }
    }, error1 => this.error = error1);
    this.editMode = {enabled: false, chipId: null};
  }

}
