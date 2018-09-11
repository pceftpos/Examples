import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { TransactionResponse } from '../../model/TransactionResponse';
import { AppConfig } from '../helper/appConfig';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })//, 'Access-Control-Allow-Origin':'*' })  
};

@Injectable()
export class HomeService {
  protected apiServer = AppConfig.settings.apiServer.uri + "home/";

  constructor(private http: HttpClient) { }
  
  makeTransaction(amount: number): Observable<string> {
    return this.http.post<string>(this.apiServer + 'transaction', amount);
  }
}
