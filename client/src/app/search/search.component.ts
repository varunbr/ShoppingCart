import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { MediaService } from '../services/media.service';
import { SearchService } from '../services/search.service';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css'],
})
export class SearchComponent implements OnInit, OnDestroy {
  search: string;
  products: any;
  lt_md = false;
  open = false;
  private mediaSubscription: Subscription;

  constructor(
    private route: ActivatedRoute,
    private searchService: SearchService,
    private mediaObserver: MediaService
  ) {}

  ngOnInit(): void {
    this.route.queryParamMap.subscribe((params) => {
      this.search = params.get('q');
      console.log(params);
      this.searchProduct();
    });

    this.mediaSubscription = this.mediaObserver.mediaChange$.subscribe(
      (changes) => {
        this.lt_md = changes.includes('lt-md');
        this.open = false;
      }
    );
  }

  searchProduct() {
    this.searchService.searchProducts(this.search).subscribe((response) => {
      this.products = response;
      console.log(response);
    });
  }

  ngOnDestroy(): void {
    this.mediaSubscription.unsubscribe();
  }
}
