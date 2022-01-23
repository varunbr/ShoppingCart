import { Overlay, OverlayRef } from '@angular/cdk/overlay';
import { ComponentPortal } from '@angular/cdk/portal';
import { Injectable } from '@angular/core';
import { SpinnerComponent } from '../components/spinner/spinner.component';

@Injectable({
  providedIn: 'root',
})
export class BusyService {
  requestCount = 0;
  private spinnerRef: OverlayRef = this.cdkSpinnerCreate();

  constructor(private overlay: Overlay) {}

  busy() {
    this.requestCount++;
    if (!this.spinnerRef.hasAttached())
      this.spinnerRef.attach(new ComponentPortal(SpinnerComponent));
  }

  idle() {
    this.requestCount--;
    if (this.requestCount <= 0) {
      this.spinnerRef.detach();
    }
  }

  private cdkSpinnerCreate() {
    return this.overlay.create({
      hasBackdrop: true,
      positionStrategy: this.overlay
        .position()
        .global()
        .centerHorizontally()
        .centerVertically(),
    });
  }
}
