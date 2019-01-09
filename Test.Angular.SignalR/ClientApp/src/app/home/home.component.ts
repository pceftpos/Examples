import { Component, ElementRef, ViewChildren, OnInit, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators, FormControlName } from '@angular/forms';

import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/observable/fromEvent';
import 'rxjs/add/observable/merge';

import { HomeService } from './home.service';
import { GenericValidator } from '../helper/generic.validator';
import { TransactionRequest } from '../../model/TransactionRequest';
import { TransactionResponse } from '../../model/TransactionResponse';
import { Observable, Subscription } from 'rxjs';
import { Message } from '../../model/Message';
import { Key } from '../../model/Key';
import { CommonService, KeyTypes } from '../helper/common.service';
import { AppConfig } from '../helper/appConfig';
import { ModalService } from '../modal/modal.service';
import { HubConnection, HubConnectionBuilder  } from '@aspnet/signalr';

@Component({
  selector: 'app-home',
  templateUrl: './home.html',
  styleUrls: ['./home.css']
})
export class HomeComponent implements OnInit, AfterViewInit {  
  transaction: TransactionRequest;
  txnResponse: TransactionResponse;  
  receipt: string;
  message: Message;
  authInput: string;
  isRefund: boolean;

  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[];
  displayMessage: { [key: string]: string } = {};
  private validationMessages: { [key: string]: { [key: string]: string } };
  private genericValidator: GenericValidator;
  transactionForm: FormGroup;

  public hubConnection: HubConnection;
  
  constructor(private fb: FormBuilder,
    private homeService: HomeService,
    private commonService: CommonService,
    private modalService: ModalService) {  
    this.validationMessages = {
      txnAmount: {
        required: 'Transaction amount is required',
        pattern: 'Use only valid numbers',        
      }
    };
    this.genericValidator = new GenericValidator(this.validationMessages);
  }
  
  ngOnInit() {
    this.receipt = ``;
    this.authInput = "";
    this.message = null;
    this.isRefund = false;
    this.transactionForm = this.fb.group({
      txnAmount: ['', [Validators.required, Validators.pattern(/^\d+.\d{2}$/)]],
      txnRefundRef: ['']
    });
    this.assignForm();

    //SignalR
    let builder = new HubConnectionBuilder();    
    this.hubConnection = builder.withUrl('/notify').build();  // as per setup in the startup.cs    
    this.hubConnection.on("Message", (message) => {  // message coming from the server      
      this.receiveMessage(message);      
    });    
    this.hubConnection.start();  // starting the connection
  }

  assignForm() {
    if (this.transactionForm) {
      this.transactionForm.reset();
    }

    this.transactionForm.patchValue({
      txnAmount: AppConfig.settings.application.defaultAmount,
      txnRefundRef: "",
    });
  }

  ngOnDestroy() {
  }

  ngAfterViewInit(): void {
    // Watch for the blur event from any input element on the form.
    let controlBlurs: Observable<any>[] = this.formInputElements
      .map((formControl: ElementRef) => Observable.fromEvent(formControl.nativeElement, 'blur'));

    // Merge the blur event observable with the valueChanges observable
    Observable.merge(this.transactionForm.valueChanges, ...controlBlurs).debounceTime(500).subscribe(value => {
      this.displayMessage = this.genericValidator.processMessages(this.transactionForm);
    });
  }
    
  onTransaction(type: string) {
    this.transaction = null;
    this.txnResponse = null;
    this.receipt = ``;
    if (this.transactionForm.valid) {
      // Copy the form values over the contact object values
      var str = "";
      var txnRef = "";
      let txn = Object.assign({}, this.transactionForm.value);
      str = txn.txnAmount;
      txnRef = txn.txnRefundRef;

      var amount = parseFloat(str);
      
      if (amount > 0) {
        if (txnRef.length > 0 || this.isRefund) {
          //Refund
          this.transaction = <TransactionRequest>{
            Amount: amount,
            Merchant: type,
            RefundReference: txnRef,
            TxnType: "R"
          }
        }
        else {
          //Purchace
          this.transaction = <TransactionRequest>{
            Amount: amount,
            Merchant: type,
            TxnType: "P"
          }
        }

        this.homeService.makeTransaction(this.transaction)
          .subscribe(
          response => {            
            this.assignTransactionResponse(response);
          },
          error => {
            this.closeModal('notification-modal');
            console.error(error);
          }
          );
      }
    }
  }

  isRefundChanged() {
    this.isRefund = !this.isRefund;
  }

  assignReceipt(message: Message) {
    this.receipt = ``;
    if (message.text && message.text.length > 0) {
      for (var i = 0; i < message.text.length; i++) {
        this.receipt = this.receipt + message.text[i] + `\n`;
      }
    }
  }

  assignTransactionResponse(value: TransactionResponse) {
    this.txnResponse = value;
  }

  receiveMessage(message: Message) {
    if (message) {
      switch (message.type) {
        case "display":
          this.showModal(message);
          break;
        case "receipt":
          this.assignReceipt(message);
          break;
        case "transaction":
          message.text[0] = "TXN FINISHED";
          message.text[1] = "PRESS CLOSE";
          message.closeButton = true;
          this.showModal(message);
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
        error => {
          this.closeModal(id);
          console.error(error);
        }
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
}
