import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { BaseListService } from '../base/service';
import { TrackContext, TrackOrder } from '../modal/orderTracking';
import { HttpService } from './http.service';

@Injectable({
  providedIn: 'root',
})
export class TrackService extends BaseListService<TrackOrder, TrackContext> {
  baseUrl = environment.apiUrl + 'track/';

  constructor(http: HttpService) {
    super(http);
  }

  receiveOrder(orderId: number, locationId: number) {
    return this.http.post<TrackOrder>(this.baseUrl + 'receive', {
      orderId,
      locationId,
    });
  }

  departOrder(orderId: number, locationId: number) {
    return this.http.post<TrackOrder>(this.baseUrl + 'dispatch', {
      orderId,
      locationId,
    });
  }

  startDelivery(orderId: number, locationId: number) {
    return this.http.post<TrackOrder>(this.baseUrl + 'deliver', {
      orderId,
      locationId,
    });
  }
}
