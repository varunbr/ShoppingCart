import { BaseModal } from '../base/modal';

export class CartItem extends BaseModal {
  storeId: number;
  productId: number;
  storeItemId: number;
  name: string;
  amount: number;
  photoUrl: string;
  maxPerOrder: number;
  storeName: string;
  available: number;
}

export class CartStore {
  storeId: number;
  storeName: string;
  cartItems: CartItem[];
}
