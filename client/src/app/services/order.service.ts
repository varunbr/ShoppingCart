import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpService } from './http.service';
import { Checkout, CheckoutRequestItem } from '../modal/checkout';
import { BehaviorSubject, tap } from 'rxjs';
import { Params, Router } from '@angular/router';
import { OrderDetail, OrderRequest } from '../modal/order';
import { CartService } from './cart.service';
import { BaseContext } from '../base/modal';
import { BaseListService } from '../base/service';

@Injectable({
  providedIn: 'root',
})
export class OrderService extends BaseListService<OrderDetail, BaseContext> {
  baseUrl = environment.apiUrl + 'order/';
  private requestSource = new BehaviorSubject<Checkout>(null);
  checkoutRequest$ = this.requestSource.asObservable();
  constructor(
    http: HttpService,
    private cartService: CartService,
    private router: Router
  ) {
    super(http);
  }

  checkOut(checkoutItems: CheckoutRequestItem[]) {
    return this.http.post<Checkout>(this.baseUrl + 'checkout', checkoutItems);
  }

  clearRequest() {
    this.requestSource.next(null);
  }

  payForOrder(checkout: Checkout) {
    this.requestSource.next(checkout);
    this.router.navigateByUrl('/order-payment', { replaceUrl: true });
  }

  getOrder(id: number) {
    return this.http.get<OrderDetail>(this.baseUrl + id);
  }

  getOrders(params: Params) {
    return this.getModals(params, false);
  }

  order(request: OrderRequest) {
    return this.http.post(this.baseUrl, request).pipe(
      tap(() => {
        this.cartService
          .removeFromCart(
            request.items.map((i) => i.storeItemId),
            true
          )
          .subscribe();
      })
    );
  }

  acceptOrder(orderId: number) {
    return this.http.post<OrderDetail>(this.baseUrl + 'accept/' + orderId);
  }
}
