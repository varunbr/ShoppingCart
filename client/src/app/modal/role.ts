export class BaseRole {
  constructor() {
    this.role = 'TrackModerator';
  }
  userId: number;
  role: string;
}

export class TrackRole extends BaseRole {
  locationId: number;

  constructor() {
    super();
    this.role = 'TrackAgent';
  }
}

export class StoreRole extends BaseRole {
  storeId: number;

  constructor() {
    super();
    this.role = 'StoreAgent';
  }
}
