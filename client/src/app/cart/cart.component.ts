import { Component, OnInit } from '@angular/core';
import { CartItem, CartStore } from '../modal/cart';
import { CartService } from '../services/cart.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css'],
})
export class CartComponent implements OnInit {
  cartStore: CartStore[];
  cartStoreItems: number[];
  constructor(private cartService: CartService) {}

  ngOnInit(): void {
    this.cartService.cartStore$.subscribe((response) => {
      this.cartStore = response;
    });
    this.cartService.cartStoreItems$.subscribe((response) => {
      this.cartStoreItems = response;
    });
    this.cartService.getCart();
  }

  checkout() {}

  removeFromCart(itemIds: number[]) {
    this.cartService.removeFromCart(itemIds).subscribe();
  }

  removeAllFromStore(items: CartItem[]) {
    this.cartService
      .removeFromCart(items.map((i) => i.storeItemId))
      .subscribe();
  }

  removeAllFromCart() {
    this.cartService.removeFromCart(this.cartStoreItems).subscribe();
  }
}
