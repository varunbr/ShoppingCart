import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { UtilityService } from 'src/app/services/utility.service';
import { BaseListComponent } from '../../base/component';
import { Product, ProductContext } from '../../modal/product';
import { MediaService } from '../../services/media.service';
import { ProductService } from '../../services/product.service';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css'],
})
export class SearchComponent
  extends BaseListComponent<Product, ProductContext>
  implements OnInit, OnDestroy
{
  search: string;
  lt_md = false;
  queryParams: {};
  private mediaSubscription: Subscription;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    productService: ProductService,
    private mediaObserver: MediaService,
    private utility: UtilityService
  ) {
    super(productService);
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.search = params['q'];
      this.utility.setTitle(this.search);
      this.queryParams = { ...params };
      this.searchProduct(params);
    });

    this.mediaSubscription = this.mediaObserver.mediaChange$.subscribe(
      (changes) => {
        this.lt_md = changes.includes('lt-md');
      }
    );
  }

  searchProduct(params: Params) {
    this.loadItems(params);
  }

  ngOnDestroy(): void {
    this.mediaSubscription.unsubscribe();
  }

  navigateToPage(page: number) {
    window.scroll({
      top: 0,
      left: 0,
      behavior: 'smooth',
    });
    delete this.queryParams['pageNumber'];
    let params =
      page !== 1
        ? { ...this.queryParams, pageNumber: page }
        : { ...this.queryParams };
    this.router.navigate(['/search'], {
      queryParams: params,
    });
  }
}
