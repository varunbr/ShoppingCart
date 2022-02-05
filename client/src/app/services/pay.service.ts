import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { PayOption } from '../modal/payOption';
import { HttpService } from './http.service';

@Injectable({
  providedIn: 'root',
})
export class PayService {
  baseUrl = environment.apiUrl + 'payment/';
  constructor(private http: HttpService) {}

  getPayOptions() {
    return this.http.get<PayOption[]>(this.baseUrl + 'pay-option');
  }
}
