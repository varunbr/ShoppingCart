import { BaseContext, BaseModal, BaseParams } from '../base/modal';

export class ProductParams extends BaseParams {
  searchText = '';
  price = '';
  category = '';
  brand = '';
  filters = new Map<string, string>();

  getIdentifier(): string {
    return '';
  }
}

export interface Product extends BaseModal {
  name: string;
  features: string;
  available: boolean;
  amount: number;
  photoUrl: string;
}

export interface ProductContext extends BaseContext {
  searchText: string;
  price: string;
  category: string;
  brand: string;
  filters: Map<string, string>;
  properties: CategoryProperty[];
}

export class CategoryProperty {
  name: string;
  type: string;
  values: string;
  unit: string;
}
