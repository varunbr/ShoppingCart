import { BaseContext, BaseModal } from '../base/modal';

export class TrackOrder extends BaseModal {
  created: string;
  update: string;
  status: string;
  delivery: Date;
  totalAmount: number;
  deliveryCharge: number;
  name: string;
  mobile: string;
  house: string;
  landmark: string;
  postalCode: string;
  locationName: string;
  currentEvent: TrackEvent;
  trackEvents: TrackEvent[];
}
export class TrackEvent extends BaseModal {
  locationId: number;
  locationName: string;
  locationType: string;
  agentName: string;
  agentUserName: string;
  agentPhotoUrl: string;
  date: Date;
  status: string;
  done: boolean;
}

export class TrackContext extends BaseContext {
  status: string;
  location: string;
}
