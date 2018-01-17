import {Injectable} from '@angular/core';
import * as io from 'socket.io-client';

@Injectable()
export class WebSocketService {

  private socket;
  private LEDSockets = [];

  constructor() {
    this.socket = io('http://localhost');

    // first get Stripes
    this.getStripes();

    this.socket.on('getStripes', (data) => {
      console.log('GotStripes:', data);
      this.LEDSockets = data;
    });

    this.socket.on('getStripeColor', (stripe, color) => {
      console.log('getStripeColor:', stripe, color);
    });
  }

  public getStripes() {
    this.socket.emit('getStripes');
    return this.LEDSockets;
  }

  public getStripeColor(stripeName) {
    this.socket.emit('getStripeColor', stripeName);
  }


}
