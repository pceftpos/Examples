import { Component, OnInit } from '@angular/core';
import { Message } from 'primeng/api';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrls: ['./app.css']
})

export class AppComponent implements OnInit {
  title = 'app';

  msgs: Message[] = [];

  constructor() { }

  ngOnInit() {
  }
}
