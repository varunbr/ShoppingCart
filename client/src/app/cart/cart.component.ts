import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { CartItem, CartStore } from '../modal/cart';
import { CartService } from '../services/cart.service';
import { UtilityService } from '../services/utility.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css'],
})
export class CartComponent implements OnInit {
  cartStore: CartStore[];
  cartStoreItems: { storeItemId: number; productId: number }[];
  productUrl = environment.apiUrl + 'product/';
  constructor(
    public utility: UtilityService,
    private cartService: CartService,
    private router: Router
  ) {
    utility.setTitle('Cart');
  }

  ngOnInit(): void {
    this.cartService.cartStore$.subscribe((response) => {
      this.cartStore = response;
    });
    this.cartService.cartStoreItems$.subscribe((response) => {
      this.cartStoreItems = response;
    });
    this.cartService.getCart();
  }

  removeFromCart(itemIds: number[]) {
    this.cartService.removeFromCart(itemIds).subscribe();
  }

  removeAllFromStore(items: CartItem[]) {
    this.cartService
      .removeFromCart(items.map((i) => i.storeItemId))
      .subscribe();
  }

  removeAllFromCart() {
    this.cartService
      .removeFromCart(this.cartStoreItems.map((i) => i.storeItemId))
      .subscribe();
  }

  isItemAvailable(items: CartItem[]) {
    return items.some((i) => i.available > 0);
  }

  checkoutStoreItems(items: CartItem[]) {
    this.router.navigate(['/checkout'], {
      queryParams: {
        storeItem: items
          .filter((i) => i.available > 0)
          .map((i) => i.storeItemId),
      },
    });
  }

  getTotal(items: CartItem[]) {
    return items.map((s) => s.amount).reduce((a, b) => a + b);
  }
}
