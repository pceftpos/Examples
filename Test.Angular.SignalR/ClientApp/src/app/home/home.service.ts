import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { TransactionResponse } from '../../model/TransactionResponse';
import { AppConfig } from '../helper/appConfig';
import { TransactionRequest } from '../../model/TransactionRequest';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })//, 'Access-Control-Allow-Origin':'*' })  
};

export enum Application {
  Eftpos = "00",
  Oxipay = "67"
}

@Injectable()
export class HomeService {
  protected apiServer = AppConfig.settings.apiServer.uri + "home/";

  constructor(private http: HttpClient) { }

  makeTransaction(txn: TransactionRequest): Observable<TransactionResponse> {
    return this.http.post<TransactionResponse>(this.apiServer + 'transaction', txn);
  }
}
