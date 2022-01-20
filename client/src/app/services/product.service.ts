import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ResponseList } from '../base/modal';
import { BaseListService } from '../base/service';
import {
  Product,
  ProductContext,
  ProductModel,
  ProductVariant,
} from '../modal/product';
import { HttpService } from './http.service';

@Injectable({
  providedIn: 'root',
})
export class ProductService extends BaseListService<Product, ProductContext> {
  baseUrl = environment.apiUrl + 'product';
  private productDetailCache = new Map<string, ProductModel>();

  constructor(
    http: HttpService<ResponseList<Product, ProductContext>>,
    private httpClient: HttpClient
  ) {
    super(http);
  }

  getProductDetail(id: number) {
    return this.getProductModel(id);
  }

  private getProductModel(id: number) {
    let response = this.productDetailCache.get(id.toString());
    if (response) return of(response);
    return this.httpClient.get<ProductModel>(this.baseUrl + `/${id}`).pipe(
      map((response) => {
        for (var key in response.products) {
          this.productDetailCache.set(key, response);
        }
        return response;
      })
    );
  }

  getTargetVariant(
    name: string,
    value: string,
    variants: ProductVariant[],
    productModel: ProductModel
  ) {
    let targetVariant: { name: string; value: string }[] = [];
    let rankings: { id: string; rank: number }[] = [];
    for (let variant of variants.filter((v) => v.name !== name)) {
      targetVariant.push({ name: variant.name, value: variant.selected });
    }
    for (let key in productModel.products) {
      let product = productModel.products[key];
      if (
        product.properties.some((p) => p.name === name && p.value === value)
      ) {
        let rank = 1;
        for (let variant of targetVariant) {
          if (
            product.properties.some(
              (p) => p.name === variant.name && p.value === variant.value
            )
          ) {
            rank = rank + 1;
          }
        }
        rankings.push({ id: key, rank: rank });
      }
    }
    return rankings.sort((r1, r2) => r2.rank - r1.rank)?.[0].id;
  }
}
