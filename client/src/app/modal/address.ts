export class Address {
  addressName: string;
  house: string;
  landmark: string;
  postalCode: string;
  areaId: number;
  cityId: number;
  stateId: number;
  countryId: number;
  locations: {
    areas: LocationInfo[];
    cities: LocationInfo[];
    states: LocationInfo[];
  };
}

export class LocationInfo {
  id: number;
  name: string;
  parentName: string;
}
