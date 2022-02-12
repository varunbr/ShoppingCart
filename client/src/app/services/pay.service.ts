import { Injectable } from '@angular/core';
import { Params } from '@angular/router';
import { environment } from 'src/environments/environment';
import { ResponseList } from '../base/modal';
import { BaseListService } from '../base/service';
import { PayOption } from '../modal/payOption';
import { Transaction, TransactionContext } from '../modal/transaction';
import { HttpService } from './http.service';

@Injectable({
  providedIn: 'root',
})
export class PayService extends BaseListService<
  Transaction,
  TransactionContext
> {
  baseUrl = environment.apiUrl + 'payment/';
  constructor(http: HttpService) {
    super(http);
  }

  getModals(params: Params) {
    return this.http.get<ResponseList<Transaction, TransactionContext>>(
      this.baseUrl + 'transactions',
      { params }
    );
  }

  getPayOptions() {
    return this.http.get<PayOption[]>(this.baseUrl + 'pay-option');
  }

  transferAmount(body) {
    return this.http.post<ResponseList<Transaction, TransactionContext>>(
      this.baseUrl + 'transfer',
      body
    );
  }
}
