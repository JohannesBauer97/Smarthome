import {Injectable} from '@angular/core';
import * as io from 'socket.io-client';
import {Observable, Subject} from 'rxjs';
import {LedStripe} from '../models/ledStripe';
import {environment} from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LedService {

  private socket: SocketIOClient.Socket;
  private _stripes: Subject<LedStripe[]> = new Subject<LedStripe[]>();
  public stripes: Observable<LedStripe[]> = this._stripes.asObservable();
  private _connected = false;

  public get connected(): boolean {
    return this._connected;
  }

  constructor() {
    this.socket = io(environment.ledSocketUrl);
    this.socket.on('getStripes', (data: LedStripe[]) => this._stripes.next(data));
    this.socket.on('connect', () => this.socketConnected());
    this.socket.on('disconnect', () => this.socketDisconnected());
  }

  private socketConnected(): void {
    this._connected = true;
  }

  private socketDisconnected(): void {
    this._connected = false;
  }

  public refreshStripes(): void {
    this.socket.emit('getStripes');
  }

  public setStripeColor(stripeName: string, color: string): void {
    this.socket.emit('setStripeColor', stripeName, color);
  }
}
