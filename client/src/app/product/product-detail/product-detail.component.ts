import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductService } from 'src/app/services/product.service';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css'],
})
export class ProductDetailComponent implements OnInit {
  imageUrls: {}[] = [];
  product;
  constructor(
    private route: ActivatedRoute,
    private productService: ProductService
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.getProduct(params['id']);
    });
  }

  getProduct(id: number) {
    this.productService.getProductDetail(id).subscribe((response) => {
      console.log(response);
      this.product = response;
      let urls = [];
      response?.photos.forEach((element) => {
        urls.push({
          small: element.url,
          medium: element.url,
          big: element.url,
        });
      });
      this.imageUrls = urls;
    });
  }
}
