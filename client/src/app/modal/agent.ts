import { BaseContext, BaseModal } from '../base/modal';

export class BaseAgent extends BaseModal {
  userId: number;
  userName: string;
  name: string;
  photoUrl: string;
  role: string;
}

export class StoreAgent extends BaseAgent {
  storeId: number;
  storeName: string;
  storeLocation: string;
}

export class TrackAgent extends BaseAgent {
  locationId: number;
  locationName: string;
  locationType: string;
  parentLocationId: number;
  parentLocationName: string;
  parentLocationType: string;
}

export class BaseAgentContext extends BaseContext {
  role: string;
  userName: string;
}

export class TrackAgentContext extends BaseAgentContext {
  location: string;
}

export class StoreAgentContext extends BaseAgentContext {
  storeName: string;
}
