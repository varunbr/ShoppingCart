export class BaseRole {
  userId: number;
  role: string;
}

export class TrackRole extends BaseRole {
  locationId: number;
}

export class StoreRole extends BaseRole {
  storeId: number;
}
