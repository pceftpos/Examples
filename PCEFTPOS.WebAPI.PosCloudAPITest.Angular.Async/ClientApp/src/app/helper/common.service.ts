import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Message } from '../../model/Message';
import { Key } from '../../model/Key';
import { AppConfig } from './appConfig';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })//, 'Access-Control-Allow-Origin':'*' })  
};

export enum KeyTypes {
  OkCancel = "0",
  YesAccept = "1",
  NoDecline = "2",
  Auth = "3",
}

@Injectable()
export class CommonService {
  serverBase: string = AppConfig.settings.apiServer.uri;

  constructor(private http: HttpClient) { }
    
  getMessage(): Observable<Message> {
    return this.http.get<Message>(this.serverBase + 'pceftposnotify/message');
  }

  sendKey(key: Key) {
    var s = this.serverBase + 'home/sendkey';
    return this.http.post(this.serverBase + 'home/sendkey', key, httpOptions);
  }
}
