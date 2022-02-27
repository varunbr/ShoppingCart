import { BaseContext, BaseModal } from '../base/modal';
import { OrderItem } from './order';

export class StoreOrder extends BaseModal {
  transactionId: number;
  created: Date;
  update: Date;
  totalAmount: number;
  deliveryCharge: number;
  status: string;
  storeName: string;
  storeId: number;
  sourceLocationId: number;
  sourceLocationName: string;
  orderItems: OrderItem[];
}

export class StoreOrderContext extends BaseContext {
  status: string;
  storeName: string;
}

export class StoreInfo {
  id: number;
  name: string;
  location: string;
}
