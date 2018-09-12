import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { IAppConfig } from '../../model/IAppConfig';


@Injectable()
export class AppConfig {

  static settings: IAppConfig;

  constructor(private http: HttpClient) { }

  load() {
    const jsonFile = "assets/config.json";
    return new Promise<void>((resolve, reject) => {
      this.http.get(jsonFile).toPromise()
          .then(
          response => {
            AppConfig.settings = <IAppConfig>response;
            resolve();
          },
          error => {
            console.log("Error: " + error);
          }
        )
        .catch((response: any) => {
          reject(`Could not load file '${jsonFile}': ${JSON.stringify(response)}`);
        });
    });
  }
}
