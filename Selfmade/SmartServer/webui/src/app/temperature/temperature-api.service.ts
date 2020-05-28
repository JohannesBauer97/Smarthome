import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {TemperatureDevice} from './models/temperature-device';
import {environment} from '../../environments/environment';
import {map} from 'rxjs/operators';
import * as moment from 'moment';

@Injectable({
  providedIn: 'root'
})
export class TemperatureApiService {

  constructor(private httpClient: HttpClient) {
  }

  public GetTemperatureDevices(): Observable<TemperatureDevice[]> {
    return this.httpClient.get<TemperatureDevice[]>(`${environment.apiBase}/temperature/devices`).pipe(
      map(value => {
        value.forEach(value1 => {
          value1.lastDataUpdate = moment(value1.lastDataUpdate);
        });
        return value;
      })
    );
  }

  public UpdateTemperatureDevice(updateTemperatureDevice: TemperatureDevice): Observable<TemperatureDevice> {
    return this.httpClient.put<TemperatureDevice>(`${environment.apiBase}/temperature/device`, updateTemperatureDevice);
  }
}
