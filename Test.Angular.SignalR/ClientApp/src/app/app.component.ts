import { Component, OnInit } from '@angular/core';
import { HubConnection } from '@aspnet/signalr';
import { Message } from 'primeng/api';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrls: ['./app.css']
})

export class AppComponent implements OnInit {
  title = 'app';

  private _hubConnection: HubConnection;
  msgs: Message[] = [];

  constructor() { }

  ngOnInit() {
  }
}
