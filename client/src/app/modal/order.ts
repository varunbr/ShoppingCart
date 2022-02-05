import { CheckoutRequestItem } from './checkout';

export class OrderDetail {
  id: number;
  transactionId: number;
  created: Date;
  delivery: Date;
  totalAmount: number;
  deliveryCharge: number;
  status: string;
  orderItems: OrderItem[];
}
export class OrderItem {
  id: number;
  storeItemId: number;
  productId: number;
  name: string;
  photoUrl: string;
  price: number;
  count: number;
}

export class OrderRequest {
  items: CheckoutRequestItem[];
  payOption: string;
  totalAmount: number;
}
