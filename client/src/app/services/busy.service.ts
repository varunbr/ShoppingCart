import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class BusyService {
  requestCount = 0;
  private source = new BehaviorSubject<boolean>(null);
  show$ = this.source.asObservable();

  constructor() {}

  busy() {
    this.requestCount++;
    this.source.next(true);
  }

  idle() {
    this.requestCount--;
    if (this.requestCount <= 0) {
      this.source.next(false);
    }
  }
}
