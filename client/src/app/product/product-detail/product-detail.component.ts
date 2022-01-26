import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
  ProductDetail,
  ProductModel,
  ProductVariant,
} from 'src/app/modal/product';
import { ProductService } from 'src/app/services/product.service';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css'],
})
export class ProductDetailComponent implements OnInit {
  imageUrls: string[] = [];
  product: ProductDetail;
  variants: ProductVariant[];
  productModel: ProductModel;
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private productService: ProductService
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.getProduct(params['id']);
    });
  }

  getProduct(id: number) {
    this.productService.getProductDetail(id).subscribe((response) => {
      this.productModel = response;
      this.product = response.products[id.toString()];
      this.variants = response.variants;
      this.setVariantSelections(this.product, this.variants);
      let urls = [];
      this.product?.photos.forEach((element) => {
        urls.push(element.url);
      });
      this.imageUrls = urls;
    });
  }

  setVariantSelections(product: ProductDetail, variants: ProductVariant[]) {
    for (let variant of variants) {
      let value = product?.properties.find(
        ({ name }) => name === variant.name
      )?.value;
      variant.selected = value;
    }
  }

  changeVariant(name: string, value: string) {
    let id = this.productService.getTargetVariant(
      name,
      value,
      this.variants,
      this.productModel
    );
    if (id) this.router.navigateByUrl(`/product/${id}`, { replaceUrl: true });
  }
}
