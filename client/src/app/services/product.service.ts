import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { BaseListService } from '../base/service';
import { Product, ProductContext, ProductParams } from '../modal/product';

@Injectable({
  providedIn: 'root',
})
export class ProductService extends BaseListService<
  Product,
  ProductParams,
  ProductContext
> {
  baseUrl = environment.apiUrl + 'search';

  constructor(http: HttpClient) {
    super(http, true);
  }

  resetParams() {
    this.params = new ProductParams();
  }

  getQueryString(): string {
    return '';
  }
}
