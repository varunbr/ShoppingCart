import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BaseListComponent } from 'src/app/base/component';
import { OrderItem } from 'src/app/modal/order';
import { StoreOrder, StoreOrderContext } from 'src/app/modal/store';
import { StoreService } from 'src/app/services/store.service';
import { ToastrService } from 'src/app/services/toastr.service';
import { UtilityService } from 'src/app/services/utility.service';

@Component({
  selector: 'app-store-order-list',
  templateUrl: './store-order-list.component.html',
  styleUrls: ['./store-order-list.component.css'],
})
export class StoreOrderListComponent
  extends BaseListComponent<StoreOrder, StoreOrderContext>
  implements OnInit
{
  orderByValues = [
    { name: 'Latest (Default)', value: null },
    { name: 'Oldest', value: 'Oldest' },
  ];
  statusValues = [
    { name: 'Confirmed (Default)', value: null },
    { name: 'Dispatched', value: 'Dispatched' },
    { name: 'Delivered', value: 'Delivered' },
    { name: 'Created', value: 'Created' },
    { name: 'All', value: 'All' },
  ];
  step = 0;

  constructor(
    private storeService: StoreService,
    private route: ActivatedRoute,
    private router: Router,
    private toastrService: ToastrService,
    public utility: UtilityService
  ) {
    super(storeService);
    utility.setTitle('Store Orders');
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.loadItems(params, false);
      this.step = 0;
    });
  }

  getTotal(items: OrderItem[]) {
    let total = 0;
    for (let item of items) {
      total += item.price * item.count;
    }
    return total;
  }

  clear() {
    this.context.orderBy = '';
    this.context.status = '';
    this.context.storeName = '';
  }

  getParams() {
    let params = {};
    if (this.context.orderBy) params['orderBy'] = this.context.orderBy;
    if (this.context.status) params['status'] = this.context.status;
    if (this.context.storeName) params['storeName'] = this.context.storeName;
    return params;
  }

  apply() {
    this.router.navigate(['/store/order'], {
      queryParams: this.getParams(),
    });
  }

  pageChange(page: number) {
    let params = this.getParams();
    if (page > 1) params['pageNumber'] = page;
    this.router.navigate(['/store/order'], {
      queryParams: params,
    });
  }

  dispatchOrder(index: number) {
    this.storeService
      .dispatchOrder(this.items[index].id)
      .subscribe((response) => {
        response.status = response.status + '*';
        this.items[index] = response;
        this.toastrService.success('Order dispatched');
      });
  }
}
