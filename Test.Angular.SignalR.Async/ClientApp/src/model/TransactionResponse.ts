export class TransactionResponse {
  txnType: string;
  merchant: string;
  cardType: string;
  cardName: string;
  rrn: string;
  settlementDate: Date;
  amtCash: number;
  amtPurchase: number;
  amtTip: number;
  authCode: number;
  txnRef: string;
  pan: string;
  dateExpiry: string;
  track2: string;
  cardAccountType: string;
  //TODO
  //TxnFlags: string;
  balanceReceived: boolean;
  availableBalance: number;
  clearedFundsBalance: number;
  success: boolean;
  responseCode: string;
  responseText: string;
  dateTime: Date;
  catid: string;
  caid: string;
  stan: number;
  purchaseAnalysisData: string;
}
