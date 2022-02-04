import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Checkout, CheckoutItem } from 'src/app/modal/checkout';
import { OrderService } from 'src/app/services/order.service';

@Component({
  selector: 'app-product-checkout',
  templateUrl: './product-checkout.component.html',
  styleUrls: ['./product-checkout.component.css'],
})
export class ProductCheckoutComponent implements OnInit {
  checkoutResponse: Checkout;
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private orderService: OrderService
  ) {}

  ngOnInit(): void {
    this.route.queryParamMap.subscribe((params) => {
      let items = [];
      for (let id of params.getAll('storeItem')) {
        items = [...items, { storeItemId: parseInt(id), itemQuantity: 1 }];
      }
      this.checkOut(items);
    });
  }

  checkOut(checkoutItems: { storeItemId: number; itemQuantity: number }[]) {
    this.orderService.checkOut(checkoutItems).subscribe((response) => {
      this.checkoutResponse = response;
    });
  }

  getProductUrl(id: number) {
    return this.router.serializeUrl(
      this.router.createUrlTree([`/product/${id}`])
    );
  }

  changeQuantity(item: CheckoutItem, value: number) {
    item.itemQuantity = item.itemQuantity + value;
    this.checkoutResponse.isValid = false;
  }

  getTotal() {
    let total = 0;
    for (let item of this.checkoutResponse.items) {
      total += item.amountPerUnit * item.itemQuantity;
    }
    return total;
  }

  removeItem(storeItemId: number) {
    this.checkoutResponse.items = this.checkoutResponse.items.filter(
      (i) => i.storeItemId !== storeItemId
    );
    this.checkoutResponse.isValid = false;
  }

  verify() {
    let items = this.checkoutResponse.items.map((i) => {
      return { storeItemId: i.storeItemId, itemQuantity: i.itemQuantity };
    });
    this.checkOut(items);
  }

  order() {
    console.log(this.checkoutResponse);
  }
}
