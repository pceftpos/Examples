import { Component, ElementRef, ViewChildren, OnInit, AfterViewInit, ViewContainerRef } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators, FormControlName } from '@angular/forms'
import { SettingsService } from './settings.service';

import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/observable/fromEvent';
import 'rxjs/add/observable/merge';
import { Observable, Subscription } from 'rxjs';
import { GenericValidator } from '../helper/generic.validator';

import { AppConfig } from '../helper/appConfig';
import { IAppConfig } from '../../model/IAppConfig';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.html',
  styleUrls: ['./settings.css']
})
export class SettingsComponent {

  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[];
  displayMessage: { [key: string]: string } = {};
  private validationMessages: { [key: string]: { [key: string]: string } };
  private genericValidator: GenericValidator;
  settingsForm: FormGroup;

  settings: IAppConfig;

  constructor(private settingsService: SettingsService,
    private fb: FormBuilder) {
    this.validationMessages = {
      uri: {
        required: 'API server URL is required',
        pattern: 'Use only valid https URL',
      },
      timer: {
        required: 'Timer is required',
        pattern: 'Use only numbers',
      },
      defaultAmount: {
        required: 'Default transaction amount is required',
        pattern: 'Use only valid amount',
      },
      wait: {
        required: 'Timer to continue pulling messages is required',
        pattern: 'Use only numbers',
      },
    };
    this.genericValidator = new GenericValidator(this.validationMessages);
  }
  
  ngOnInit() {
    this.settings = AppConfig.settings;

    this.settingsForm = this.fb.group({
      uri: ['', [Validators.required, Validators.pattern('^https:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#\/=]{2,256}$')]],
      timer: ['', [Validators.required, Validators.pattern('^[1-9][0-9]*$')]],
      defaultAmount: ['', [Validators.required, Validators.pattern(/^\d+.\d{2}$/)]],
      wait: ['', [Validators.required, Validators.pattern('^[1-9][0-9]*$')]]
    });
    this.assignForm();
  }

  assignForm() {
    if (this.settingsForm) {
      this.settingsForm.reset();
    }

    this.settingsForm.patchValue({
      uri: AppConfig.settings.apiServer.uri,
      timer: AppConfig.settings.application.timer,
      defaultAmount: AppConfig.settings.application.defaultAmount,
      wait: AppConfig.settings.application.wait,
    });
  }

  ngAfterViewInit(): void {
    // Watch for the blur event from any input element on the form.
    let controlBlurs: Observable<any>[] = this.formInputElements
      .map((formControl: ElementRef) => Observable.fromEvent(formControl.nativeElement, 'blur'));

    // Merge the blur event observable with the valueChanges observable
    Observable.merge(this.settingsForm.valueChanges, ...controlBlurs).debounceTime(500).subscribe(value => {
      this.displayMessage = this.genericValidator.processMessages(this.settingsForm);
    });
  }

  onUpdate() {
    if (this.settingsForm.valid) {
      // Copy the form values over the contact object values      
      let settings = Object.assign({}, this.settings, this.settingsForm.value);
      this.settings.apiServer.uri = settings.uri;
      this.settings.application.timer = parseInt(settings.timer);
      this.settings.application.defaultAmount = settings.defaultAmount;
      this.settings.application.wait = parseInt(settings.wait);

      this.settingsService.saveConfig(this.settings)
        .subscribe(
            response => { this.assignSettings(this.settings); },
            error => console.error(error)
          );
    }
  }

  assignSettings(settings: IAppConfig) {
    AppConfig.settings = settings;
    this.assignForm();
  }
}
