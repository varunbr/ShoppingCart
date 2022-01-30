import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs';
import {
  ProductDetail,
  ProductModel,
  ProductVariant,
} from 'src/app/modal/product';
import { CartService } from 'src/app/services/cart.service';
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
  addedToCart = false;
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private productService: ProductService,
    private cartService: CartService
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.getProduct(params['id']);
    });
    this.cartService.cartStoreItems$.subscribe((items) => {
      this.addedToCart = items.some((i) => i === this.product?.storeItemId);
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
      this.cartService.cartStoreItems$.pipe(take(1)).subscribe((items) => {
        this.addedToCart = items.some((i) => i === this.product?.storeItemId);
      });
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

  onCartClick(storeItemId: number) {
    if (this.addedToCart) {
      this.cartService.removeFromCart([storeItemId]).subscribe();
    } else {
      this.cartService.addToCart(storeItemId).subscribe();
    }
  }
}
