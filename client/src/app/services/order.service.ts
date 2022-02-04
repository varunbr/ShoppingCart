import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpService } from './http.service';
import { Checkout } from '../modal/checkout';

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  baseUrl = environment.apiUrl + 'order/';
  constructor(private http: HttpService) {}

  checkOut(checkoutItems: { storeItemId: number; itemQuantity: number }[]) {
    return this.http.post<Checkout>(this.baseUrl + 'checkout', checkoutItems);
  }
}
