import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment';

@Pipe({
  name: 'timeDiffInSeconds'
})
export class TimeDiffInSecondsPipe implements PipeTransform {

  transform(value: moment.Moment, postfix: string): unknown {
    const diff = moment().diff(value, 'seconds');
    if (diff <= 60){
      return diff + postfix;
    }
    return moment(value).fromNow().toString();
  }

}
