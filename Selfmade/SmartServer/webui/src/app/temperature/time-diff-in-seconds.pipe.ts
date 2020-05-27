import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment';

@Pipe({
  name: 'timeDiffInSeconds'
})
export class TimeDiffInSecondsPipe implements PipeTransform {

  transform(value: moment.Moment, postfix: string): unknown {
    return moment().diff(value, 'seconds').toString() + postfix;
  }

}
