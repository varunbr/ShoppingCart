import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { take } from 'rxjs';
import { Checkout } from 'src/app/modal/checkout';
import { OrderRequest } from 'src/app/modal/order';
import { PayOption } from 'src/app/modal/payOption';
import { OrderService } from 'src/app/services/order.service';
import { PayService } from 'src/app/services/pay.service';
import { ToastrService } from 'src/app/services/toastr.service';
import { UtilityService } from 'src/app/services/utility.service';

@Component({
  selector: 'app-order-payment',
  templateUrl: './order-payment.component.html',
  styleUrls: ['./order-payment.component.css'],
})
export class OrderPaymentComponent implements OnInit, OnDestroy {
  checkout: Checkout;
  payOptions: PayOption[];
  payMethod: PayOption;
  isValid: boolean;
  constructor(
    private orderService: OrderService,
    private payService: PayService,
    private toastr: ToastrService,
    private router: Router,
    utility: UtilityService
  ) {
    utility.setTitle('Payment');
    orderService.checkoutRequest$
      .pipe(take(1))
      .subscribe((response) => (this.checkout = response));
  }

  ngOnInit(): void {
    if (!this.checkout) {
      this.toastr.warn('Unable to proceed. Please try again.');
    } else {
      this.payService.getPayOptions().subscribe((response) => {
        this.payOptions = response;
        this.payMethod = response.find((r) => r.available);
        this.isValid =
          this.payMethod.available &&
          this.payMethod.balance >= this.checkout.total;
      });
    }
  }

  ngOnDestroy(): void {
    this.orderService.clearRequest();
  }

  getTotal() {
    let total = 0;
    for (let item of this.checkout.items) {
      total += item.amountPerUnit * item.itemQuantity;
    }
    return total;
  }

  onPaySelectionChange() {
    this.isValid =
      this.payMethod.available && this.payMethod.balance >= this.checkout.total;
  }

  pay() {
    let request = new OrderRequest();
    request.items = this.checkout.items.map((i) => {
      return { itemQuantity: i.itemQuantity, storeItemId: i.storeItemId };
    });
    request.payOption = this.payMethod.name;
    request.totalAmount = this.checkout.total;
    this.orderService.order(request).subscribe((response) => {
      this.toastr.success('Order placed successfully.');
      this.router.navigateByUrl('/order/' + response, { replaceUrl: true });
    });
  }
}
