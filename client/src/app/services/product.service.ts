import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ResponseList } from '../base/modal';
import { BaseListService } from '../base/service';
import { Product, ProductContext } from '../modal/product';
import { HttpService } from './http.service';

@Injectable({
  providedIn: 'root',
})
export class ProductService extends BaseListService<Product, ProductContext> {
  baseUrl = environment.apiUrl + 'product';

  constructor(http: HttpService<ResponseList<Product, ProductContext>>) {
    super(http);
  }

  getProductDetail(id: number) {
    return this.http.get(this.baseUrl + `/${id}`, true);
  }
}
