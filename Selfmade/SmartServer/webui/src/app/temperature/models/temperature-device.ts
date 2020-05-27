import * as moment from 'moment';

export class TemperatureDevice {
  lastDataUpdate: moment.Moment;
  temperature: string;
  humidity: string;
  chipId: string;
}
