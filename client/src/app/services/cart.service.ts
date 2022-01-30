import { Injectable } from '@angular/core';
import { BehaviorSubject, take, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { CartItem, CartStore } from '../modal/cart';
import { HttpService } from './http.service';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  baseUrl = environment.apiUrl + 'cart';
  private cartStoreSource = new BehaviorSubject<CartStore[]>([]);
  private cartItemsSource = new BehaviorSubject<number[]>([]);
  private userId = 0;
  cartStore$ = this.cartStoreSource.asObservable();
  cartStoreItems$ = this.cartItemsSource.asObservable();
  constructor(
    private http: HttpService,
    private accountService: AccountService
  ) {
    this.accountService.user$.subscribe((user) => {
      this.userId = user == null ? 0 : user.id;
      this.getLocalCart();
    });
  }

  getCart() {
    return this.http
      .get<CartStore[]>(this.baseUrl)
      .subscribe((response) => this.setCartStore(response));
  }

  getLocalCart() {
    const value = localStorage.getItem(`cart-user-${this.userId}`);
    if (value) {
      const cartStore: CartStore[] = JSON.parse(value);
      this.cartStoreSource.next(cartStore);
      this.setCartItems(cartStore);
    } else {
      this.cartStoreSource.next([]);
      this.setCartItems([]);
    }
  }

  setCartStore(cartStore: CartStore[]) {
    localStorage.setItem(`cart-user-${this.userId}`, JSON.stringify(cartStore));
    this.setCartItems(cartStore);
    this.cartStoreSource.next(cartStore);
  }

  private setCartItems(cartStore: CartStore[]) {
    let itemIds: number[] = [];
    for (let item of cartStore) {
      itemIds = [...itemIds, ...item.cartItems.map((i) => i.storeItemId)];
    }
    this.cartItemsSource.next(itemIds);
  }

  addToCart(storeItemId: number) {
    return this.http.post<CartItem>(this.baseUrl, storeItemId).pipe(
      tap((response) => {
        let cartStores: CartStore[];
        this.cartStore$.pipe(take(1)).subscribe((r) => (cartStores = r));
        if (cartStores.some((i) => i.storeId === response.storeId)) {
          let cartStore = cartStores.find(
            (i) => i.storeId === response.storeId
          );
          cartStore.cartItems = [...cartStore.cartItems, response];
        } else {
          let cartStore = new CartStore();
          cartStore.cartItems = [response];
          cartStore.storeName = response.storeName;
          cartStore.storeId = response.storeId;
          cartStores = [...cartStores, cartStore];
        }
        this.setCartStore(cartStores);
      })
    );
  }

  removeFromCart(storeItemIds: number[]) {
    return this.http.delete(this.baseUrl, storeItemIds).pipe(
      tap(() => {
        let cartStore: CartStore[];
        this.cartStore$.pipe(take(1)).subscribe((r) => (cartStore = r));
        for (let item of cartStore) {
          item.cartItems = item.cartItems.filter(
            (item) => !storeItemIds.some((j) => j === item.storeItemId)
          );
        }
        cartStore = cartStore.filter((item) => item.cartItems.length > 0);
        this.setCartStore(cartStore);
      })
    );
  }
}
