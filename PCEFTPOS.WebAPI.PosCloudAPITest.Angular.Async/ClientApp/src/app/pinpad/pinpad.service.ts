import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { StatusResponse } from '../../model/StatusResponse';
import { LogonResponse } from '../../model/LogonResponse';
import { AppConfig } from '../helper/appConfig';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })//, 'Access-Control-Allow-Origin':'*' })  
};

@Injectable()
export class PinpadService {
  protected apiServer = AppConfig.settings.apiServer.uri + "home/pinpad/";

  constructor(private http: HttpClient) { }
  
  getStatus(): Observable<StatusResponse> {
    return this.http.get<StatusResponse>(this.apiServer + 'status');
  }

  getLogon(): Observable<LogonResponse> {
    return this.http.get<LogonResponse>(this.apiServer + 'logon');
  }
}

