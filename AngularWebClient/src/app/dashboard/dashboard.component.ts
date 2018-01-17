import {Component, OnInit} from '@angular/core';
import {WebSocketService} from "../web-socket.service";

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  private LEDSockets = [];

  constructor(private webSocketService: WebSocketService) {
  }

  ngOnInit() {
    this.LEDSockets = this.webSocketService.getStripes();
  }

}
