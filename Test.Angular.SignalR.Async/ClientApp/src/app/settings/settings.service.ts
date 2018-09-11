import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { AppConfig } from '../helper/appConfig';
import { IAppConfig } from '../../model/IAppConfig';


const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable()
export class SettingsService {
  protected apiServer = AppConfig.settings.apiServer.uri;

  constructor(private http: HttpClient) { }

  saveConfig(config: IAppConfig) {
    return this.http.post(this.apiServer + 'settings', config);
  }
}
