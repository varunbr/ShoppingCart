import { BaseModal } from '../base/modal';
import { CheckoutRequestItem } from './checkout';

export class OrderDetail extends BaseModal {
  transactionId: number;
  created: Date;
  delivery: Date;
  totalAmount: number;
  deliveryCharge: number;
  status: string;
  name: string;
  mobile: string;
  house: string;
  landmark: string;
  postalCode: string;
  locationName: string;
  orderItems: OrderItem[];
  currentEvent: TrackEvent;
  trackEvents: TrackEvent[];
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
