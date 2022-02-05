import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpService } from './http.service';
import { Checkout, CheckoutRequestItem } from '../modal/checkout';
import { BehaviorSubject, tap } from 'rxjs';
import { Router } from '@angular/router';
import { OrderDetail, OrderRequest } from '../modal/order';
import { CartService } from './cart.service';

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  baseUrl = environment.apiUrl + 'order/';
  private requestSource = new BehaviorSubject<Checkout>(null);
  checkoutRequest$ = this.requestSource.asObservable();
  constructor(
    private http: HttpService,
    private cartService: CartService,
    private router: Router
  ) {}

  checkOut(checkoutItems: CheckoutRequestItem[]) {
    return this.http.post<Checkout>(this.baseUrl + 'checkout', checkoutItems);
  }

  clearRequest() {
    this.requestSource.next(null);
  }

  payForOrder(checkout: Checkout) {
    this.requestSource.next(checkout);
    this.router.navigateByUrl('/payment', { replaceUrl: true });
  }

  getOrder(id: number) {
    return this.http.get<OrderDetail>(this.baseUrl + id);
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
}
