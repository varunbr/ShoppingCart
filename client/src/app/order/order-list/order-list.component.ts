import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { BaseListComponent } from 'src/app/base/component';
import { BaseContext } from 'src/app/base/modal';
import { OrderDetail, OrderItem } from 'src/app/modal/order';
import { OrderService } from 'src/app/services/order.service';
import { UtilityService } from 'src/app/services/utility.service';

@Component({
  selector: 'app-order-list',
  templateUrl: './order-list.component.html',
  styleUrls: ['./order-list.component.css'],
})
export class OrderListComponent
  extends BaseListComponent<OrderDetail, BaseContext>
  implements OnInit
{
  constructor(
    public utility: UtilityService,
    private route: ActivatedRoute,
    private router: Router,
    orderService: OrderService
  ) {
    super(orderService);
    utility.setTitle('Orders');
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.loadItems(params, false);
    });
  }

  getTotal(items: OrderItem[]) {
    let total = 0;
    for (let item of items) {
      total += item.price * item.count;
    }
    return total;
  }

  pageChange(event) {
    window.scroll({
      top: 0,
      left: 0,
      behavior: 'smooth',
    });
    let params = {};
    if (event !== 1) {
      params = { pageNumber: event };
    }
    this.router.navigate(['order'], { queryParams: params });
  }
}
