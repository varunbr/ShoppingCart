export class Checkout {
  addressName: string;
  price: number;
  deliveryCharge: number;
  total: number;
  storeName: string;
  storeId: number;
  isValid: boolean;
  errorMessage: string;
  items: CheckoutItem[];
}
export class CheckoutItem {
  name: string;
  photoUrl: string;
  amountPerUnit: number;
  total: number;
  storeItemId: number;
  maxPerOrder: number;
  productId: number;
  itemQuantity: number;
  errorMessage: string;
}
export class CheckoutRequestItem {
  storeItemId: number;
  itemQuantity: number;
}
