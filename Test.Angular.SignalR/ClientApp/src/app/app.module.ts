import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER } from '@angular/core';
import { FormsModule, ReactiveFormsModule  } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import 'rxjs/add/operator/map';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';

import { ToastModule, ToastsManager, ToastOptions } from 'ng2-toastr/ng2-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { GrowlModule } from 'primeng/primeng';

import { AppConfig } from './helper/appConfig';
import { CommonService } from './helper/common.service';
import { HomeComponent } from './home/home.component';
import { HomeService } from './home/home.service';
import { SettingsComponent } from './settings/settings.component';
import { SettingsService } from './settings/settings.service';
import { PinpadService } from './pinpad/pinpad.service';
import { PinpadComponent } from './pinpad/pinpad.component';
import { ModalComponent } from './modal/modal.component';
import { ModalService } from './modal/modal.service';

// Global settings for toast messages
export class CustomOptions extends ToastOptions {
  animate = 'fade';
  dismiss = 'auto';
  newestOnTop = true;
  enableHTML = true;
  positionClass = 'toast-top-right';
  showCloseButton = true;
}

export function initializeApp(appConfig: AppConfig) {
  return () => appConfig.load();
}

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    SettingsComponent,
    PinpadComponent,
    ModalComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    GrowlModule,
    BrowserAnimationsModule,
    ToastModule.forRoot(),
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'settings', component: SettingsComponent },
      { path: 'pinpad', component: PinpadComponent },
    ])
  ],
  providers: [
    HomeService,
    CommonService,
    SettingsService,
    PinpadService,
    ModalService,
    { provide: ToastOptions, useClass: CustomOptions },
    AppConfig,
    { provide: APP_INITIALIZER,
      useFactory: initializeApp,
      deps: [AppConfig], multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
