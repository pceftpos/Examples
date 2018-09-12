import { Component, ViewContainerRef } from '@angular/core';
import { PinpadService } from './pinpad.service';
import { StatusResponse } from '../../model/StatusResponse';
import { LogonResponse } from '../../model/LogonResponse';
import { Key } from '../../model/Key';

import { Observable, Subscription } from 'rxjs';
import { Message } from '../../model/Message';
import { ToastsManager, Toast } from 'ng2-toastr';
import { TimerObservable } from 'rxjs/observable/TimerObservable';
import { CommonService, KeyTypes } from '../helper/common.service';
import { AppConfig } from '../helper/appConfig';
import { ModalService } from '../modal/modal.service';

@Component({
  selector: 'app-pinpad',
  templateUrl: './pinpad.html',
  styleUrls: ['./pinpad.css']
})
export class PinpadComponent {
  statusResponse: StatusResponse;
  logonResponse: LogonResponse;  
  timer: Subscription;
  receipt: string;
  showStatusResp: boolean;
  showLogonResp: boolean;
  authInput: string;
  message: Message;

  constructor(private pinpadService: PinpadService,
    private commonService: CommonService,
    private modalService: ModalService,
    private toastr: ToastsManager, public vcr: ViewContainerRef) {
    this.toastr.setRootViewContainerRef(vcr);
  }

  ngOnInit() {
    this.receipt = ``;
    this.authInput = "";
    this.showStatusResp = false;
    this.showLogonResp = false;
    this.message = null;
  }

  ngOnDestroy() {
    this.toastr.dispose();
    if (this.timer) {
      this.timer.unsubscribe();
    }
  }
  
  onStatus() {
    this.receipt = ``;
    this.showStatusResp = true;
    this.showLogonResp = false;
    this.statusResponse = null;
    this.pinpadService.getStatus()
      .subscribe(
        response => { this.startPulling(); },
        error => console.error(error)
      );
  }

  assignStatusResponse(status: StatusResponse) {
    this.statusResponse = status;
    this.timer.unsubscribe();
  }


  startPulling() {
    this.timer = Observable.interval(1000 * AppConfig.settings.application.timer).subscribe(x => {
      this.commonService.getMessage()
        .subscribe(
          response => this.receiveMessage(response),
          error => this.timer.unsubscribe()
        )
    });
  }

  onLogon() {
    this.receipt = ``;
    this.logonResponse = null;    
    
    this.showStatusResp = false;
    this.showLogonResp = true;
    this.pinpadService.getLogon()
      .subscribe(
        response => { this.startPulling(); },
        error => {
          console.error(error);
          this.closeModal('notification-modal');
        }
      );
  }

  assignReceipt(receipt) {
    this.receipt = ``;
    if (receipt.receiptText && receipt.receiptText.length > 0) {      
      for (var i = 0; i < receipt.receiptText.length; i++) {
        this.receipt = this.receipt + receipt.receiptText[i] + `\n`;
      }
    }
  }

  assignLogonResponse(value: LogonResponse) {
    this.logonResponse = value;
    this.timer.unsubscribe();    
  }

  receiveMessage(message: Message) {
    if (message) {
      switch (message.type) {
        case "display":
          this.showModal(message);
          break;
        case "receipt":
          this.assignReceipt(message.response);
          break;
        case "logon":
          message.text[0] = "LOGON DONE";
          message.text[1] = "PRESS CLOSE";
          message.closeButton = true;
          this.showModal(message);
          this.assignLogonResponse(message.response);
          break;
        case "status":
          this.assignStatusResponse(message.response);          
          break;
        default:
          this.showModal(message);
          break;
      }
    }
  }

  openModal(id: string) {
    this.authInput = "";
    this.modalService.open(id);
  }

  onModalButton(id: string, buttonType: string) {
    var key: Key = {
      data: "",
      sessionId: this.message.sessionId,
      key: ""
    };

    switch (buttonType) {
      case "3":
        key.data = this.authInput;
        key.key = KeyTypes.Auth;
        break;
      case "2":
        key.key = KeyTypes.NoDecline;
        break;
      case "0":
        key.key = KeyTypes.OkCancel;
        break;
      case "1":
        key.key = KeyTypes.YesAccept;
        break;
      case "-1":
        this.closeModal(id);
        return;
    }

    this.commonService.sendKey(key)
      .subscribe(
        response => { this.closeModal(id); },
        error => console.error(error)
      );
  }

  closeModal(id: string) {
    this.modalService.close(id);
  }

  showModal(message: Message) {
    if (message && message.text) {
      this.message = message;

      this.openModal("notification-modal");
    }
  }

  showMessage(message: Message) {    
    if (message && message.text) {
      var str = message.text[0] =`\n`;
      for (var i = 1; i < message.text.length; i++) {
        str = message.text[i] + `\n`;
      }

      this.toastr.success(str, message.type);
    }
  }

}
