import {Injectable} from '@angular/core';
import * as io from 'socket.io-client';
import {LED} from './led';
import {Subject} from 'rxjs/Subject';

@Injectable()
export class LedService {
  private _io;
  private _LEDs = new Subject<LED[]>();
  public LEDs = this._LEDs.asObservable();

  constructor() {
    this._io = io('http://localhost:80');

    this._io.on('getStripes', this._getStripes.bind(this));
    this._io.on('disconnect', error => {
      console.log('Disconnect', error);
    });

    this._io.on('connect_error', error => {
      console.log('Connect Error', error);
    });

    // Initial Request Stripes from Server
    this._io.emit('getStripes');
  }

  private _getStripes(data: LED[]): void {
    this._LEDs.next(data);
    console.log(data);
  }

  public refresh(): void {
    this._io.emit('getStripes');
  }

  public setStripeColor(name: string, color: string) {
    this._io.emit('setStripeColor', name, color);
  }

  public rgbToHex(red: number, green: number, blue: number) {
    let rHex = red.toString(16);
    rHex = (rHex.length === 1 ? '0' + rHex : rHex);

    let gHex = green.toString(16);
    gHex = (gHex.length === 1 ? '0' + gHex : gHex);

    let bHex = blue.toString(16);
    bHex = (bHex.length === 1 ? '0' + bHex : bHex);

    return '#' + rHex + gHex + bHex;
  }

  public hexToRgb(hexVal: string) {
    const result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hexVal);
    return result ? {
      r: parseInt(result[1], 16),
      g: parseInt(result[2], 16),
      b: parseInt(result[3], 16)
    } : null;
  }

}
