import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { Subscription } from 'rxjs';
import { BaseListComponent } from '../../base/component';
import { Product, ProductContext, ProductParams } from '../../modal/product';
import { MediaService } from '../../services/media.service';
import { ProductService } from '../../services/product.service';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css'],
})
export class SearchComponent
  extends BaseListComponent<Product, ProductParams, ProductContext>
  implements OnInit, OnDestroy
{
  search: string;
  lt_md = false;
  private mediaSubscription: Subscription;

  constructor(
    private route: ActivatedRoute,
    productService: ProductService,
    private mediaObserver: MediaService
  ) {
    super(productService);
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.search = params['q'];
      this.searchProduct(params);
    });

    this.mediaSubscription = this.mediaObserver.mediaChange$.subscribe(
      (changes) => {
        this.lt_md = changes.includes('lt-md');
      }
    );
  }

  searchProduct(params: Params) {
    this.loadModals(params);
  }

  ngOnDestroy(): void {
    this.mediaSubscription.unsubscribe();
  }
}
