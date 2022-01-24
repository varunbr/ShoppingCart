import { BaseContext, BaseModal } from '../base/modal';

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

export class ProductModel {
  products: Map<string, ProductDetail>;
  variants: ProductVariant[];
}

export class ProductVariant {
  name: string;
  selected: string;
  values: string[];
}

export class ProductDetail extends BaseModal {
  name: string;
  brand: string;
  model: string;
  description: string;
  features: string;
  amount: number;
  maxPerOrder: number;
  category: string;
  available: boolean;
  deliveryCharge: number;
  storeName: string;
  storeItemId: number;
  photos: { url: string; isMain: boolean }[];
  properties: { name: string; value: string }[];

  getVariantProperties(variants: string[]) {
    return this.properties.filter(({ name }) =>
      variants.some((v) => v === name)
    );
  }
}

export class HomePage {
  categoryProducts: CategoryProduct[];
  categories: { name: string; photoUrl: string }[];
}
export class CategoryProduct {
  category: string;
  products: {
    id: number;
    name: string;
    amount: number;
    photoUrl: string;
  }[];
}
