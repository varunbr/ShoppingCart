import { Injectable } from '@angular/core';
import { Params } from '@angular/router';
import { environment } from 'src/environments/environment';
import { ResponseList } from '../base/modal';
import { BaseListService } from '../base/service';
import { StoreOrder, StoreOrderContext } from '../modal/store';
import { HttpService } from './http.service';

@Injectable({
  providedIn: 'root',
})
export class StoreService extends BaseListService<
  StoreOrder,
  StoreOrderContext
> {
  baseUrl = environment.apiUrl + 'store/';
  constructor(http: HttpService) {
    super(http);
  }

  getModals(params: Params, cache = true) {
    return this.http.get<ResponseList<StoreOrder, StoreOrderContext>>(
      this.baseUrl + 'order',
      {
        params: params,
        cache: cache,
      }
    );
  }

  dispatchOrder(orderId: number) {
    return this.http.post<StoreOrder>(this.baseUrl + 'dispatch/' + orderId);
  }
}
